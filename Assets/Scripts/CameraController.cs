using System.Collections;
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

    private readonly float cameraSmoothing = 5f;

    private void Start()
    {
        startPosition = transform.position;
        cam = GetComponentInChildren<Camera>();
        if (shakeOnStart)
        {
            Shake(6, 5);
        }
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, PlayerController.Instance.transform.rotation, cameraSmoothing * Time.deltaTime);
    }

    public void Shake(float shakeSpeed, float shakeForce)
    {
        shakeTime = 0;
        shakeAmount = new Vector2(Random.Range(0.5f, 1), Random.Range(0.5f, 1));
        StopAllCoroutines();
        StartCoroutine(ProcessShake(shakeSpeed, shakeForce));
    }

    public void ZoomIn(float speed, float amount)
    {
        StopAllCoroutines();
        StartCoroutine(Zoom(1, speed, amount));
    }

    public void ZoomOut(float speed, float amount)
    {
        StopAllCoroutines();
        StartCoroutine(Zoom(-1, speed, amount));
    }

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

    private IEnumerator Zoom(int direction, float speed, float amount)
    {
        Vector3 originalPosition = cam.transform.position;
        Vector3 forward = cam.transform.forward * direction;
        for (float time = 0; time < 1; time += Time.deltaTime * speed)
        {
            Vector3 position = originalPosition + forward * amount * time;
            cam.transform.position = position;

            if (direction < 0)
            {
                RenderSettings.fogEndDistance = 7f + Vector3.Distance(PlayerController.Instance.transform.position, cam.transform.position);

                cam.transform.localRotation = Quaternion.Lerp(cam.transform.localRotation, Quaternion.Euler(new Vector3(15f, 0, 0f)), time);
            }

            yield return null;
        }
        cam.transform.position = originalPosition + forward * amount;
    }
}