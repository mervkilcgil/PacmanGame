using System;
using UnityEngine;

public class PackMan : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void Start()
    {
        animator.SetFloat("DirX", 0);
        animator.SetFloat("DirY", 0);
    }

    void FixedUpdate() 
    {

    }
}