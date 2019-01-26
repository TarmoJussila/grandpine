using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [Header("Shake")]
    [SerializeField] private float minShake = 0.5f;
    [SerializeField] private float maxShake = 1f;
    [SerializeField] private bool shakeHorizontally = true;
    [SerializeField] private bool shakeVertically = true;
    [SerializeField] private bool shakeOnStart = false;

    private Camera cam;
    private Vector3 startPosition;
    private float shakeTime = 0;
    private Vector3 shakeOffset = Vector3.zero;
    private Vector2 shakeAmount;

    void Start()
    {
        startPosition = transform.position;
        cam = GetComponentInChildren<Camera>();
        if (shakeOnStart)
        {
            Shake(6, 5);
        }
    }

    void LateUpdate()
    {
        transform.rotation = PlayerController.Instance.transform.rotation;
    }

    public void Shake(float shakeSpeed, float shakeForce)
    {
        shakeTime = 0;
        shakeAmount = new Vector2(Random.Range(0.5f, 1), Random.Range(0.5f, 1));
        StopAllCoroutines();
        StartCoroutine(ProcessShake(shakeSpeed, shakeForce));
    }

    // https://en.wikipedia.org/wiki/Damped_sine_wave
    private IEnumerator ProcessShake(float shakeSpeed, float shakeForce)
    {
        while (shakeTime < 10)
        {
            shakeTime += Time.deltaTime;
            float rightShake = Mathf.Exp(-shakeTime) * Mathf.Cos(shakeSpeed * Mathf.PI * shakeTime + shakeSpeed);
            float upShake = Mathf.Exp(-shakeTime) * Mathf.Cos(shakeSpeed * Mathf.PI * shakeTime + shakeSpeed);
            Vector3 right = shakeHorizontally ? cam.transform.right * rightShake * shakeForce : Vector3.zero;
            Vector3 up = shakeVertically ? cam.transform.up * upShake * shakeForce : Vector3.zero;
            transform.position = startPosition + right + up;
            yield return null;
        }
    }
}
