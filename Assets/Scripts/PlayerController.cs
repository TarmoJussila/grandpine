using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public Transform PlayerTransform { get { return player.transform; } }
    public Twig Twig { get { return twig; } }
    public Axe Axe { get { return axe; } }
    public Tree Tree { get { return tree; } }

    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GameObject playerTwig;
    [SerializeField] private GameObject playerAxe;
    [SerializeField] private Twig twig;
    [SerializeField] private GameObject houseDoor;
    [SerializeField] private Axe axe;
    [SerializeField] private Tree tree;

    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float twigRange = 2f;
    [SerializeField] private float houseDoorRange = 3f;
    [SerializeField] private float axeRange = 2f;
    [SerializeField] private float treeRange = 4f;
    [SerializeField] private float actionDelay = 1f;

    private bool isHoldingTwig;
    private bool isHoldingAxe;
    private float currentActionTimer;
    private bool treeWasInRange;

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

        bool treeInRange = IsTreeInRange();
        if (treeInRange && !treeWasInRange)
        {
            tree.PlayHeartParticles();
            treeWasInRange = true;
        }
        else if (!treeInRange)
        {
            treeWasInRange = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (currentActionTimer > 0f)
            {
                return;
            }
            
            if (IsTwigInRange() && !isHoldingTwig)
            {
                if (!twig.IsCollected)
                {
                    playerAnimator.SetTrigger("CollectTwig");
                    currentActionTimer = actionDelay;
                }
            }
            else if (IsHouseDoorInRange() && isHoldingTwig)
            {
                DisableTwig();
                currentActionTimer = actionDelay;
            }
            else if (IsAxeInRange() && !isHoldingAxe)
            {
                if (!axe.IsCollected)
                {
                    playerAnimator.SetTrigger("CollectAxe");
                    currentActionTimer = actionDelay;
                }
            }
            else if (IsTreeInRange() && isHoldingAxe)
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
        isHoldingTwig = true;
    }

    public void DisableTwig()
    {
        playerTwig.SetActive(false);
        isHoldingTwig = false;
    }

    public void EnableAxe()
    {
        playerAxe.SetActive(true);
        isHoldingAxe = true;
    }

    public void DisableAxe()
    {
        playerAxe.SetActive(false);
        isHoldingAxe = false;
    }

    private bool IsTwigInRange()
    {
        return twigRange > Vector3.Distance(twig.transform.position, player.transform.position);
    }

    private bool IsHouseDoorInRange()
    {
        return houseDoorRange > Vector3.Distance(houseDoor.transform.position, player.transform.position);
    }

    private bool IsAxeInRange()
    {
        return axeRange > Vector3.Distance(axe.transform.position, player.transform.position);
    }

    private bool IsTreeInRange()
    {
        return treeRange > Vector3.Distance(tree.transform.position, player.transform.position);
    }
}
