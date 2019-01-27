using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class HouseLight : MonoBehaviour
{
    [SerializeField] private float decaySpeed = 1;
    [SerializeField] private float fullIntensity = 5.5f;
    private float targetIntensity = 0;
    private float currentIntensity = 1;
    private Light light;

    private void Awake()
    {
        light = GetComponent<Light>();
        currentIntensity = fullIntensity;
    }

    private void Update()
    {
        if (targetIntensity < currentIntensity)
        {
            currentIntensity = Mathf.MoveTowards(currentIntensity, targetIntensity, Time.deltaTime / 10 * decaySpeed);
        }
        else
        {
            currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime / 10 * decaySpeed);
            if (currentIntensity > targetIntensity - 0.1f)
            {
                targetIntensity = 0;
            }
        }
        light.intensity = currentIntensity;
    }

    public void AddLight(float intensity)
    {
        targetIntensity = fullIntensity;
    }
}
