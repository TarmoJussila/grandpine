using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    void Start()
    {
    }

    void Update()
    {
        transform.rotation = playerController.transform.rotation;
    }
}
