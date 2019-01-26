using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public bool HasFallen { get { return hasFallen; } }

    [SerializeField] private int hitAmountMax = 3;

    private int hitAmount;
    private bool hasFallen = false;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool Hit()
    {
        if (!hasFallen)
        {
            if (hitAmount < hitAmountMax)
            {
                hitAmount++;
                animator.SetTrigger("Hit");
                bool randomSound = (Random.Range(0, 2) == 0);
                if (randomSound)
                {
                    AudioController.Instance.PlaySound(SoundType.TreeHit1);
                }
                else
                {
                    AudioController.Instance.PlaySound(SoundType.TreeHit2);
                }
            }
            else
            {
                hasFallen = true;
                animator.SetTrigger("Fall");
                AudioController.Instance.PlaySound(SoundType.TreeFall);
            }
            return false;
        }
        return hasFallen;
    }
}
