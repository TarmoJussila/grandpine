using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public Transform PlayerTransform { get { return player.transform; } }
    public Twig Twig { get { return GetClosestTwig(); } }
    public Axe Axe { get { return axe; } }
    public Tree Tree { get { return tree; } }

    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private PlayerEmotes playerEmotes;
    [SerializeField] private GameObject playerTwig;
    [SerializeField] private GameObject playerAxe;
    [SerializeField] private List<Twig> twigs;
    [SerializeField] private GameObject houseDoor;
    [SerializeField] private Axe axe;
    [SerializeField] private Tree tree;
    [SerializeField] private HouseLight houseInsideLight;
    [SerializeField] private HouseLight chimneyInsideLight;

    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float twigRange = 2f;
    [SerializeField] private float houseDoorRange = 3f;
    [SerializeField] private float axeRange = 2f;
    [SerializeField] private float treeRange = 4f;
    [SerializeField] private float actionDelay = 1f;

    private bool isHoldingTwig;
    private bool isHoldingAxe;
    private int twigsCollected;
    private float currentActionTimer;
    private bool treeWasInRange;
    private bool isPlayerInputEnabled;

    private readonly int targetTwigAmount = 2;
    private readonly float playerInputDelay = 1f;

    private void Start()
    {
        StartCoroutine(EnablePlayerInput());
    }

    private IEnumerator EnablePlayerInput()
    {
        yield return new WaitForSeconds(playerInputDelay);

        isPlayerInputEnabled = true;

        playerEmotes.ShowEmote(Emote.Twig);
    }

    private void Update()
    {
        if (!isPlayerInputEnabled)
        {
            return;
        }

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

        if (!tree.HasFallen)
        {
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
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (currentActionTimer > 0f)
            {
                return;
            }

            if (IsTwigInRange() && !isHoldingTwig)
            {
                if (!GetClosestTwig().IsCollected)
                {
                    playerAnimator.SetTrigger("CollectTwig");
                    currentActionTimer = actionDelay;
                    playerEmotes.ShowEmote(Emote.House);
                }
            }
            else if (IsHouseDoorInRange() && isHoldingTwig)
            {
                DisableTwig();
                twigsCollected++;
                currentActionTimer = actionDelay;

                if (twigsCollected >= targetTwigAmount)
                {
                    playerEmotes.ShowEmote(Emote.Axe);
                }
                else
                {
                    playerEmotes.ShowEmote(Emote.Twig);
                }
                houseInsideLight.AddLight(5.5f);
                chimneyInsideLight.AddLight(5.5f);
            }
            else if (IsAxeInRange() && !isHoldingAxe && twigsCollected >= targetTwigAmount)
            {
                if (!axe.IsCollected)
                {
                    playerAnimator.SetTrigger("CollectAxe");
                    currentActionTimer = actionDelay;
                    playerEmotes.ShowEmote(Emote.Tree);
                }
            }
            else if (IsTreeInRange() && isHoldingAxe)
            {
                if (!tree.HasFallen)
                {
                    playerAnimator.SetTrigger("Hit");
                    currentActionTimer = actionDelay;
                    playerEmotes.ShowEmote(Emote.Heart);
                }
            }
        }

        if (tree.HasFallen)
        {
            playerEmotes.ShowEmote(Emote.HeartBroken);
            DisablePlayerControl();
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

        AudioController.Instance.PlaySound(SoundType.Fire);
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

    public void DisablePlayerControl()
    {
        isPlayerInputEnabled = false;

        GameController.Instance.SetGameState(GameState.GameOver);

        playerAnimator.ResetTrigger("Walk");
        playerAnimator.SetTrigger("StopWalk");
    }

    private Twig GetClosestTwig()
    {
        var orderedTwigs = twigs.OrderBy(x => Vector3.Distance(player.transform.position, x.transform.position)).ToList();

        var closestTwig = orderedTwigs.First();

        return closestTwig;
    }

    private bool IsTwigInRange()
    {
        return twigRange > Vector3.Distance(GetClosestTwig().transform.position, player.transform.position);
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