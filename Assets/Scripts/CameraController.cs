using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [Header("Shake")]
    [SerializeField] private float shakeSpeed = 1;
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
            Shake();
        }
    }

    void LateUpdate()
    {
        transform.rotation = playerController.transform.rotation;
    }

    public void Shake()
    {
        shakeTime = 0;
        shakeAmount = new Vector2(Random.Range(minShake, maxShake), Random.Range(minShake, maxShake));
        StartCoroutine(ProcessShake());
    }

    // https://en.wikipedia.org/wiki/Damped_sine_wave
    private IEnumerator ProcessShake()
    {
        while (shakeTime < 10)
        {
            shakeTime += Time.deltaTime;
            float rightShake = shakeAmount.x * Mathf.Exp(-shakeTime) * Mathf.Cos(shakeSpeed * Mathf.PI * shakeTime);
            float upShake = shakeAmount.y * 1 - (Mathf.Exp(-shakeTime) * Mathf.Cos(shakeSpeed * Mathf.PI * shakeTime));
            Vector3 right = shakeHorizontally ? cam.transform.right * rightShake : Vector3.zero;
            Vector3 up = shakeVertically ? cam.transform.up * upShake : Vector3.zero;
            transform.position = startPosition + right + up;
            yield return null;
        }
    }
}
