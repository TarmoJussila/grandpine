using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
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
                animator.SetTrigger("Hit");
                hitAmount++;
            }
            else
            {
                animator.SetTrigger("Fall");
                hasFallen = true;
            }
        }
        return hasFallen;
    }
}
