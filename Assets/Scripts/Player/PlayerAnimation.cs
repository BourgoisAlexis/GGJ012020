using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public GameObject Weapon;
    
    private Animator animator;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.x != 0)
        {
            if (rb.velocity.y == 0)
                animator.SetBool("Running", true);
        }
        else
            animator.SetBool("Running", false);
    }

    public void Jump()
    {
        animator.SetTrigger("Jump");
    }

    public void Death()
    {
        animator.SetTrigger("Death");
        Weapon.SetActive(false);
    }
}
