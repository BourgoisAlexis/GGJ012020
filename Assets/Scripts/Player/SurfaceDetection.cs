using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceDetection : MonoBehaviour
{
    public LayerMask Ground;

    private float distance = 0.4f;
    private float width = 0.1f;
    private float step = 3;
    private float increment;

    private void Awake()
    {
        increment = width / step;
    }


    public bool GroundDetection()
    {
        for (float i = -width; i < width; i += increment)
        {
            //Debug
            Debug.DrawRay(transform.position + (Vector3.right * i), -Vector2.up * distance, Color.red);

            if (Physics2D.Raycast(transform.position + (Vector3.right * i), -Vector2.up, distance, Ground))
                return true;
        }

        return false;
    }
}
