using UnityEngine;

public enum ScaleType
{
    Up,
    Side,
    Both,
    SideClamped,
    UpClamped,
    BothClamped
};

public class ScaleEffects : MonoBehaviour
{
    public float speed;
    public float range;
    [Tooltip("This is used only when you select clamped scale")]
    public Vector2 clampMinMax;
    public ScaleType scaleType = ScaleType.Side;
    float timecounter = 0;

    private void Update()
    {
        timecounter += Time.deltaTime * speed;

        if (scaleType == ScaleType.Up)
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Cos(timecounter) * range);
        else if (scaleType == ScaleType.Side)
            transform.localScale = new Vector3(Mathf.Cos(timecounter) * range, transform.localScale.y);
        else if (scaleType == ScaleType.Both)
            transform.localScale = new Vector3(Mathf.Sin(timecounter) * range, Mathf.Sin(timecounter) * range);
        else if (scaleType == ScaleType.BothClamped)
            transform.localScale = new Vector3(Mathf.Clamp(timecounter, clampMinMax.x, clampMinMax.y) * range, Mathf.Clamp(timecounter, clampMinMax.x, clampMinMax.y) * range);
        else if (scaleType == ScaleType.SideClamped)
            transform.localScale = new Vector3(Mathf.Clamp(timecounter, clampMinMax.x, clampMinMax.y) * range, 0f);
        else if (scaleType == ScaleType.UpClamped)
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Clamp(timecounter, clampMinMax.x, clampMinMax.y) * range);
    }
}
