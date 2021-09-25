using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public PlayerController controller;

    public Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
    }

    public void SetAnimatorState(int state)
    {
        animator.SetInteger("PlayerState", state);
    }
    public int GetAnimatorState()
    {
        return animator.GetInteger("PlayerState");
    }
}
