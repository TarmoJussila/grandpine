using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void WalkSound()
    {
        bool randomSound = (Random.Range(0, 2) == 0);
        if (randomSound)
        {
            AudioController.Instance.PlaySound(SoundType.Walk1);
        }
        else
        {
            AudioController.Instance.PlaySound(SoundType.Walk2);
        }
    }

    private void TwigCollect()
    {
        PlayerController.Instance.Twig.Collect();
        AudioController.Instance.PlaySound(SoundType.TwigCollect);
        AudioController.Instance.RaiseMusicVolume();
    }

    private void AxeCollect()
    {
        PlayerController.Instance.Axe.Collect();
        AudioController.Instance.PlaySound(SoundType.TwigCollect);
        AudioController.Instance.RaiseMusicVolume();
    }

    private void TreeHit()
    {
        PlayerController.Instance.Tree.Hit();
        AudioController.Instance.PlaySound(SoundType.AxeHit);
        AudioController.Instance.RaiseMusicVolume();
    }

    private void CameraShake()
    {
        CameraController.Instance.Shake(10, 0.5f);
    }

    private void SnowEffect()
    {
        SnowController.Instance.PlayStormParticles();
    }
}
