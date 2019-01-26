using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public Transform PlayerTransform { get { return player.transform; } }
    public Tree Tree { get { return tree; } }
    public Twig Twig { get { return twig; } }

    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GameObject playerTwig;
    [SerializeField] private GameObject playerAxe;
    [SerializeField] private Tree tree;
    [SerializeField] private Twig twig;

    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float treeRange = 4f;
    [SerializeField] private float twigRange = 2f;
    [SerializeField] private float actionDelay = 1f;

    private float currentActionTimer;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + Time.deltaTime * rotationSpeed, 0);

            if (player.transform.localScale.x > 0f)
            {
                player.transform.localScale = new Vector3(-player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z);
            }

            playerAnimator.SetTrigger("Walk");
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y - Time.deltaTime * rotationSpeed, 0);

            if (player.transform.localScale.x < 0f)
            {
                player.transform.localScale = new Vector3(-player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z);
            }

            playerAnimator.SetTrigger("Walk");
        }
        else
        {
            playerAnimator.ResetTrigger("Walk");
            playerAnimator.SetTrigger("StopWalk");
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (currentActionTimer > 0f)
            {
                return;
            }

            if (IsTwigInRange())
            {
                if (!twig.IsCollected)
                {
                    playerAnimator.SetTrigger("Collect");
                    currentActionTimer = actionDelay;
                }
            }
            else if (IsTreeInRange())
            {
                if (!tree.HasFallen)
                {
                    playerAnimator.SetTrigger("Hit");
                    currentActionTimer = actionDelay;
                }
            }
        }

        if (currentActionTimer > 0f)
        {
            currentActionTimer = Mathf.Max(currentActionTimer - Time.deltaTime, 0f);
        }
    }

    public void EnableTwig()
    {
        playerTwig.SetActive(true);
    }

    public void DisableTwig()
    {
        playerTwig.SetActive(false);
    }

    public void EnableAxe()
    {
        playerAxe.SetActive(true);
    }

    public void DisableAxe()
    {
        playerAxe.SetActive(false);
    }

    private bool IsTreeInRange()
    {
        return treeRange > Vector3.Distance(tree.transform.position, player.transform.position);
    }

    private bool IsTwigInRange()
    {
        return twigRange > Vector3.Distance(twig.transform.position, player.transform.position);
    }
}
