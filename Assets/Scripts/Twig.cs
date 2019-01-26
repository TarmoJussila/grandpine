using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twig : MonoBehaviour
{
    public bool IsCollected { get; private set; }

    public void Collect()
    {
        if (IsCollected)
        {
            return;
        }

        IsCollected = true;
        PlayerController.Instance.EnableTwig();
        gameObject.SetActive(false);
    }
}
