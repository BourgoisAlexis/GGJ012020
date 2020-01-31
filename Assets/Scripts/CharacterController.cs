using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    #region Variables
    private Rigidbody2D rb;
    private SurfaceDetection surf;
    private InputBuffer buffer;

    private float gravityMultiplier = 0.2f;
    private float maxHSpeed = 10;
    private float maxVSpeed = 10;
    private float jumpHeight = 7;

    private float currentHspeed;
    private float currentVspeed;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        surf = GetComponent<SurfaceDetection>();
        buffer = GetComponent<InputBuffer>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
            buffer.AddInput("Jump");
    }

    private void FixedUpdate()
    {
        CustomGravity();
        ApplyPhysic();
        Jump();
        HMove();
    }

    private void CustomGravity()
    {
        if(currentVspeed != 0)
            currentVspeed -= gravityMultiplier;

        if (rb.velocity.y < 0)
        {
            currentVspeed -= gravityMultiplier * 1.5f;

            if (surf.GroundDetection())
                currentVspeed = 0;
        }
    }

    private void ApplyPhysic()
    {
        rb.velocity = new Vector2(currentHspeed, currentVspeed);
    }

    private void Jump()
    {
        if (buffer.CheckButton("Jump"))
            if (surf.GroundDetection())
                currentVspeed = jumpHeight;
    }

    private void HMove()
    {

    }
}
