using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceDetection : MonoBehaviour
{
    public LayerMask Ground;
    private float distance = 0.4f;

    public bool GroundDetection()
    {
        if (Physics2D.Raycast(transform.position, -Vector2.up, distance, Ground))
            return true;

        return false;
    }


    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, -Vector2.up * distance, Color.red);
    }
}
