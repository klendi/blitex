using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScaleType
{
    Up,
    Side,
    Both
};

public class ScaleEffects : MonoBehaviour
{
    public float speed;
    public float range;
    float timecounter = 0;
    public ScaleType scaleType = ScaleType.Side;

    private void Update()
    {
        timecounter += Time.deltaTime * speed;
        
        if (scaleType == ScaleType.Up)
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Cos(timecounter) * range);
        else if (scaleType == ScaleType.Side)
            transform.localScale = new Vector3(Mathf.Cos(timecounter) * range, transform.localScale.y);
        else if (scaleType == ScaleType.Both)
            transform.localScale = new Vector3(Mathf.Sin(timecounter) * range, Mathf.Sin(timecounter) * range);
    }
}
