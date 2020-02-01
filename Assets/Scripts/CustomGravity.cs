using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CustomGravity : MonoBehaviour
{
    #region Variables
    private Rigidbody2D rb;

    private float grav = 1.5f;
    #endregion


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y != 0)
            rb.velocity -= new Vector2(0, grav);
    }
}
