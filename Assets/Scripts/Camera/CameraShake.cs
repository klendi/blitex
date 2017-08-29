using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    Transform camTransform;

    // How long the object should shake for.
    public float shakeDuration = .5f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.15f;
    public float decreaseFactor = .1f;

    public bool shaked = false;

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = transform;
        }
    }

    void Update()
    {
        if (shaked)
        {
            if (shakeDuration > 0)
            {
                camTransform.position = originalPos + Random.insideUnitSphere * shakeAmount;

                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                shakeDuration = 1f;
                camTransform.position = originalPos;
                shaked = false;
            }
        }
    }

    public void ShakeCamera()
    {
        originalPos = camTransform.position;
        shaked = true;
    }
}
