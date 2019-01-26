using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void TreeHit()
    {
        PlayerController.Instance.Tree.Hit();
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
