/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
================================================================
    Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using UnityEngine;

/// <summary>
/// Keeps this (full screen) panel inside the screen's safe area so the UI never sits under
/// the notch, the rounded corners or the home indicator. Put the screen's UI under it.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class SafeAreaFitter : MonoBehaviour
{
    RectTransform rect;
    Rect lastSafeArea;
    Vector2Int lastScreenSize;

    private void OnEnable()
    {
        rect = (RectTransform)transform;
        Apply();
    }

    //the safe area changes when the device rotates (or the simulator device changes)
    private void Update()
    {
        if (Screen.safeArea != lastSafeArea || Screen.width != lastScreenSize.x || Screen.height != lastScreenSize.y)
            Apply();
    }

    private void Apply()
    {
        if (Screen.width <= 0 || Screen.height <= 0)
            return;

        lastSafeArea = Screen.safeArea;
        lastScreenSize = new Vector2Int(Screen.width, Screen.height);

        Vector2 min = lastSafeArea.position;
        Vector2 max = lastSafeArea.position + lastSafeArea.size;
        min.x /= Screen.width;
        min.y /= Screen.height;
        max.x /= Screen.width;
        max.y /= Screen.height;

        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
}
