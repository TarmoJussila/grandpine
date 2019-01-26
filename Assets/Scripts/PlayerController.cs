using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform PlayerTransform { get { return player.transform; } }

    [Header("References")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GameObject player;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Tree tree;

    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float treeRange = 3f;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + Time.deltaTime * rotationSpeed, 0);

            playerAnimator.SetTrigger("Walk");
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y - Time.deltaTime * rotationSpeed, 0);

            playerAnimator.SetTrigger("Walk");
        }
        else
        {
            playerAnimator.ResetTrigger("Walk");
            playerAnimator.SetTrigger("Idle");
        }

        if (IsCollectableInRange())
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                playerAnimator.SetTrigger("Collect");
            }
        }
        else if (IsTreeInRange())
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                bool hasFallen = tree.Hit();

                if (!hasFallen)
                {
                    playerAnimator.SetTrigger("Hit");
                    cameraController.Shake(10, 0.5f);
                }
            }
        }


    }

    private bool IsTreeInRange()
    {
        return treeRange > Vector3.Distance(tree.transform.position, player.transform.position);
    }

    private bool IsCollectableInRange()
    {
        return false;
    }
}
