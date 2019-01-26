using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightController : Singleton<LightController>
{
    [SerializeField] private float dayLength = 120f;
    [SerializeField] private float maxIntensity = 1f;
    [SerializeField] private float minIntensity = 0f;
    private Light light;
    private float currentDayTime = 0;

    private void Awake()
    {
        light = GetComponent<Light>();
    }

    private void Update()
    {
        currentDayTime += Time.deltaTime;
        if (currentDayTime < dayLength / 2)
        {
            light.intensity = Mathf.Lerp(minIntensity, maxIntensity, currentDayTime * 2 / dayLength);
        }
        else
        {
            light.intensity = Mathf.Lerp(maxIntensity, minIntensity, Mathf.InverseLerp(maxIntensity / 2, maxIntensity, currentDayTime / dayLength));
        }

        transform.rotation = Quaternion.Euler(50, Mathf.Lerp(0, 360, currentDayTime / dayLength), 0);

        if (currentDayTime > dayLength)
        {
            currentDayTime = 0;
        }
    }
}
