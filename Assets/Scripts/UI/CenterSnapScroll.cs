/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
================================================================
    Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Vertical ScrollRect helper: the item closest to a center line settles on it
/// once the player lets go. Can also shrink items the further they are from the
/// center (level menu) and lock the off-center ones so only the focused one is clickable.
/// </summary>
[RequireComponent(typeof(ScrollRect))]
public class CenterSnapScroll : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [Tooltip("Where the focused item settles. Defaults to the ScrollRect's viewport.")]
    public RectTransform center;
    [Tooltip("Item to focus on start (-1 keeps the layout as it is)")]
    public int startingIndex = -1;

    [Header("Scale effect")]
    public bool scaleItems = false;
    [Tooltip("Scale of the focused item")]
    public float maxScale = 1f;
    [Tooltip("Scale of items far from the center")]
    public float minScale = 0.7f;
    [Tooltip("Distance from the center (canvas units) at which an item reaches the minimum scale")]
    public float scaleRange = 300f;
    [Tooltip("Only the focused item is interactable (needs a CanvasGroup on each item)")]
    public bool lockOffCenterItems = false;

    [Header("Tint effect")]
    [Tooltip("Blend each item's Image from the far color to the focused color as it nears the center")]
    public bool tintItems = false;
    [Tooltip("Distance from the center (canvas units) at which an item has the far color. Smaller than the scale range keeps the accent on the focused item only")]
    public float tintRange = 160f;
    public Color focusedTint = Color.white;
    public Color farTint = Color.white;

    [Header("Edge fade")]
    [Tooltip("Items fade out as they get close to the top and bottom of this area (needs a CanvasGroup on each item)")]
    public RectTransform fadeArea;
    [Tooltip("How far from the edge (canvas units) an item starts to fade")]
    public float fadeLength = 100f;

    [Header("Focus glow")]
    [Tooltip("A soft halo that follows the focused item. Put it behind the items (first child of this scroller)")]
    public RectTransform focusGlow;
    public Vector2 glowPadding = new Vector2(56f, 56f);

    [Header("Snapping")]
    public float snapSpeed = 8f;
    [Tooltip("The scroll must slow below this speed before it snaps")]
    public float snapVelocity = 80f;

    /// <summary>Fired when a different item becomes the focused one.</summary>
    public event Action<int> IndexChanged;

    public int CurrentIndex { get; private set; } = -1;

    ScrollRect scrollRect;
    readonly List<RectTransform> items = new List<RectTransform>();
    readonly List<CanvasGroup> groups = new List<CanvasGroup>();
    readonly List<Image> images = new List<Image>();
    readonly List<Color> baseColors = new List<Color>();   //the colors other scripts gave the items (progress states), the tint multiplies them
    bool baseColorsCaptured = false;
    readonly Vector3[] areaCorners = new Vector3[4];
    bool dragging;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        if (center == null)
            center = scrollRect.viewport != null ? scrollRect.viewport : (RectTransform)transform;
        RefreshItems();
    }

    private void Start()
    {
        if (startingIndex >= 0)
            SnapTo(startingIndex);
    }

    /// <summary>Re-reads the content's children, call after adding or removing items at runtime.</summary>
    public void RefreshItems()
    {
        items.Clear();
        groups.Clear();
        images.Clear();
        baseColors.Clear();
        baseColorsCaptured = false;
        foreach (Transform child in scrollRect.content)
        {
            if (!child.gameObject.activeSelf)
                continue;
            items.Add((RectTransform)child);
            groups.Add(child.GetComponent<CanvasGroup>());
            images.Add(child.GetComponent<Image>());
        }
    }

    /// <summary>Jumps straight to an item, without easing.</summary>
    public void SnapTo(int index)
    {
        if (items.Count == 0)
            return;

        Canvas.ForceUpdateCanvases();
        index = Mathf.Clamp(index, 0, items.Count - 1);
        MoveContent(OffsetTo(items[index]));
        scrollRect.StopMovement();
        UpdateItems();
    }

    public void OnBeginDrag(PointerEventData eventData) { dragging = true; }

    public void OnEndDrag(PointerEventData eventData) { dragging = false; }

    private void LateUpdate()
    {
        if (items.Count == 0)
            return;

        UpdateItems();

        bool settled = !dragging && Mathf.Abs(scrollRect.velocity.y) < snapVelocity;
        if (!settled)
            return;

        float offset = OffsetTo(items[CurrentIndex]);
        if (Mathf.Abs(offset) < 0.01f)
            return;

        scrollRect.StopMovement();
        MoveContent(offset * Mathf.Clamp01(snapSpeed * Time.unscaledDeltaTime));
    }

    /// <summary>Picks the focused item, then scales and locks the items around it.</summary>
    void UpdateItems()
    {
        //other scripts color the items in their Start, so we read the colors on the first update
        if (!baseColorsCaptured)
        {
            baseColors.Clear();
            foreach (Image image in images)
                baseColors.Add(image != null ? image.color : Color.white);
            baseColorsCaptured = true;
        }

        float canvasScale = Mathf.Max(0.0001f, transform.lossyScale.y);
        int closest = 0;
        float closestDistance = float.MaxValue;

        float areaBottom = 0f, areaTop = 0f;
        if (fadeArea != null)
        {
            fadeArea.GetWorldCorners(areaCorners);
            areaBottom = areaCorners[0].y;
            areaTop = areaCorners[1].y;
        }

        for (int i = 0; i < items.Count; i++)
        {
            float distance = Mathf.Abs(OffsetTo(items[i])) / canvasScale;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = i;
            }

            // 0 at the center line, 1 once the item is scaleRange away (eased, so the change is gradual)
            float far = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(distance / scaleRange));

            if (scaleItems)
            {
                float scale = Mathf.Lerp(maxScale, minScale, far);
                items[i].localScale = new Vector3(scale, scale, 1f);
            }

            if (fadeArea != null && groups[i] != null)
            {
                float edge = Mathf.Min(items[i].position.y - areaBottom, areaTop - items[i].position.y) / canvasScale;
                groups[i].alpha = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(edge / fadeLength));
            }

            if (tintItems && images[i] != null)
            {
                float farTintAmount = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(distance / tintRange));
                images[i].color = baseColors[i] * Color.Lerp(focusedTint, farTint, farTintAmount);
            }
        }

        if (focusGlow != null)
        {
            RectTransform focused = items[closest];
            focusGlow.position = focused.position;
            focusGlow.sizeDelta = new Vector2(focused.rect.width * focused.localScale.x, focused.rect.height * focused.localScale.y) + glowPadding;
        }

        if (lockOffCenterItems)
        {
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups[i] != null)
                    groups[i].interactable = i == closest;
            }
        }

        if (closest != CurrentIndex)
        {
            CurrentIndex = closest;
            IndexChanged?.Invoke(CurrentIndex);
        }
    }

    // World-space distance the content has to move for the item to reach the center line
    float OffsetTo(RectTransform item)
    {
        return center.position.y - item.position.y;
    }

    void MoveContent(float worldOffsetY)
    {
        scrollRect.content.position += new Vector3(0f, worldOffsetY, 0f);
    }
}
