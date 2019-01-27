using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Emote { Twig, Tree, House, Axe, Heart, HeartBroken }

public class PlayerEmotes : MonoBehaviour
{
    [SerializeField] private float duration = 5f;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float bubbleDelay = 0.1f;
    [SerializeField] private GameObject container;
    [SerializeField] private SpriteRenderer emoteRenderer;
    [SerializeField] private SpriteRenderer[] bubbleRenderers;
    [SerializeField] private Sprite twigSprite;
    [SerializeField] private Sprite treeSprite;
    [SerializeField] private Sprite houseSprite;
    [SerializeField] private Sprite axeSprite;
    [SerializeField] private Sprite heartSprite;
    [SerializeField] private Sprite heartBrokenSprite;

    private Vector3 playerDirection;
    private Vector3 originalEmoteRendererPosition;

    private float animationTime = 0;

    private void Start()
    {
        originalEmoteRendererPosition = emoteRenderer.transform.localPosition;
    }

    private void Update()
    {
        Vector3 scale = container.transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        container.transform.localScale = scale;
    }

    public void ShowEmote(Emote emote)
    {
        Sprite sprite = null;
        Vector3 offset = Vector3.zero;
        Quaternion rotation = Quaternion.identity;
        switch (emote)
        {
            case Emote.Twig:
                sprite = twigSprite;
                offset = new Vector3(-0.5f, -0.5f, 0);
                rotation = Quaternion.Euler(0, 0, -45f);
                break;
            case Emote.Tree:
                sprite = treeSprite;
                break;
            case Emote.House:
                sprite = houseSprite;
                break;
            case Emote.Axe:
                sprite = axeSprite;
                break;
            case Emote.Heart:
                sprite = heartSprite;
                break;
            case Emote.HeartBroken:
                sprite = heartBrokenSprite;
                break;
        }
        if (sprite != null)
        {
            emoteRenderer.sprite = sprite;
            emoteRenderer.gameObject.SetActive(false);
            emoteRenderer.color = new Color(emoteRenderer.color.r, emoteRenderer.color.g, emoteRenderer.color.b, 1);
            foreach (SpriteRenderer bubbleRenderer in bubbleRenderers)
            {
                bubbleRenderer.gameObject.SetActive(false);
                bubbleRenderer.color = new Color(bubbleRenderer.color.r, bubbleRenderer.color.g, bubbleRenderer.color.b, 1);
            }
            StopAllCoroutines();
            StartCoroutine(AnimateBubbles(sprite));
        }
    }

    private IEnumerator AnimateBubbles(Sprite emoteSprite)
    {
        container.SetActive(true);
        for (int i = 0; i < bubbleRenderers.Length; i++)
        {
            while (animationTime < bubbleDelay)
            {
                animationTime += Time.deltaTime;
                yield return null;
            }
            bubbleRenderers[i].gameObject.SetActive(true);
            animationTime = 0;
        }
        emoteRenderer.gameObject.SetActive(true);
        while (animationTime < duration)
        {
            animationTime += Time.deltaTime;
            yield return null;
        }
        for (float time = 0; time < fadeDuration; time += Time.deltaTime)
        {
            foreach (SpriteRenderer spriteRenderer in bubbleRenderers)
            {
                Color color = spriteRenderer.color;
                color.a = 1 - time / fadeDuration;
                spriteRenderer.color = color;
            }
            Color empteColor = emoteRenderer.color;
            empteColor.a = 1 - time / fadeDuration;
            emoteRenderer.color = empteColor;
            yield return null;
        }
        container.SetActive(false);
    }
}
