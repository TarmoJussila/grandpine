using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : Singleton<FadeController>
{
    [SerializeField] private List<Image> memoryImages = new List<Image>();

    private Image fadeImage;

    private float fadeValue = 1f;
    private float targetFadeValue = 0f;
    private float fadeSpeed = 0.5f;
    private float memoryFadeSpeed = 0.25f;
    private float memoryTime = 3f;

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

    private IEnumerator FadeIn()
    {
        fadeValue = 0f;
        targetFadeValue = 1f;

        while (fadeValue < targetFadeValue)
        {
            fadeValue = Mathf.Min(fadeValue + Time.deltaTime * fadeSpeed, targetFadeValue);

            fadeImage.color = new Color(0, 0, 0, fadeValue);

            yield return null;
        }

        fadeImage.color = Color.clear;
    }

    public void ShowMemories()
    {
        StartCoroutine(ShowMemoriesCoroutine());
    }

    private IEnumerator ShowMemoriesCoroutine()
    {
        float fadeValue = 0f;
        float fadeTargetValue = 1f;

        while (fadeValue < fadeTargetValue)
        {
            fadeValue += Time.deltaTime * memoryFadeSpeed;
            memoryImages[0].color = new Color(1, 1, 1, fadeValue);

            yield return null;
        }

        yield return new WaitForSeconds(memoryTime);

        fadeValue = 0f;
        fadeTargetValue = 1f;

        while (fadeValue < fadeTargetValue)
        {
            fadeValue += Time.deltaTime * memoryFadeSpeed;
            memoryImages[1].color = new Color(1, 1, 1, fadeValue);
            memoryImages[0].color = new Color(1, 1, 1, 1 - fadeValue);

            yield return null;
        }

        yield return new WaitForSeconds(memoryTime);

        fadeValue = 0f;
        fadeTargetValue = 1f;

        while (fadeValue < fadeTargetValue)
        {
            fadeValue += Time.deltaTime * memoryFadeSpeed;
            memoryImages[2].color = new Color(1, 1, 1, fadeValue);
            memoryImages[1].color = new Color(1, 1, 1, 1 - fadeValue);

            yield return null;
        }

        yield return new WaitForSeconds(memoryTime);

        fadeValue = 0f;
        fadeTargetValue = 1f;

        while (fadeValue < fadeTargetValue)
        {
            fadeValue += Time.deltaTime * memoryFadeSpeed;
            memoryImages[3].color = new Color(1, 1, 1, fadeValue);
            memoryImages[2].color = new Color(1, 1, 1, 1 - fadeValue);

            yield return null;
        }

        yield return new WaitForSeconds(memoryTime);

        while (fadeValue < fadeTargetValue)
        {
            fadeValue += Time.deltaTime * memoryFadeSpeed;
            memoryImages[3].color = new Color(1, 1, 1, 1 - fadeValue);

            yield return null;
        }

        StartCoroutine(FadeIn());

        AudioController.Instance.PlaySound(SoundType.Sob);
    }
}
