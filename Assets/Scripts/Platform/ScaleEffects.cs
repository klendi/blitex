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
    public ScaleType scaleType = ScaleType.Side;
    public float max, min;
    float timecounter = 0;
    Vector3 startScale;

    private void Start()
    {
        startScale = transform.localScale;
    }

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
            transform.localScale = startScale + Vector3.one * ((Mathf.Cos(timecounter) + 1) * .5f * (max - min) + min);

        else if (scaleType == ScaleType.SideClamped)
            transform.localScale = startScale + Vector3.right * ((Mathf.Cos(timecounter) + 1) * .5f * (max - min) + min);

        else if (scaleType == ScaleType.UpClamped)
            transform.localScale = startScale + Vector3.up * ((Mathf.Cos(timecounter) + 1) * .5f * (max - min) + min);
    }
}
