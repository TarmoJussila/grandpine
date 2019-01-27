using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : Singleton<FadeController>
{
    private Image fadeImage;

    private float fadeValue = 1f;
    private float targetFadeValue = 0f;
    private float fadeSpeed = 0.5f;

    private void Start()
    {
        fadeImage = GetComponentInChildren<Image>();

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        fadeValue = 1f;
        targetFadeValue = 0f;

        while (fadeValue > targetFadeValue)
        {
            fadeValue = Mathf.Max(fadeValue - Time.deltaTime * fadeSpeed, targetFadeValue);

            fadeImage.color = new Color(0, 0, 0, fadeValue);

            yield return null;
        }

        fadeImage.color = Color.clear;
    }
}
