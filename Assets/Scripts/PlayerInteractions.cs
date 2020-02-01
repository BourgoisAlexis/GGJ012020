using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    #region Variables
    public LayerMask Interactable;

    private Transform _transform;
    private InputBuffer buffer;
    private PlayerController controller;

    private float interactionRadius = 2;
    #endregion


    private void Awake()
    {
        _transform = transform;
        buffer = GetComponent<InputBuffer>();
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Shoot"))
            buffer.AddInput("Shoot");

        Interact();
    }

    private void Interact()
    {
        if(buffer.CheckButton("Shoot"))
        {
            Collider2D col = Physics2D.OverlapCircle(transform.position, interactionRadius, Interactable);

            if (col != null)
            {
                buffer.Executed("Shoot");
                CameraManager.Instance.Pan(_transform);
                UIManager.Instance.Cinemode();
                col.GetComponent<Interactable>().InteractStart();
            }
        }
    }
}
