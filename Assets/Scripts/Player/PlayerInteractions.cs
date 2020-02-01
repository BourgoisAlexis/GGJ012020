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

    public float interactionRadius = 1;
    #endregion


    private void Awake()
    {
        _transform = transform;
        buffer = GetComponent<InputBuffer>();
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (controller.canInput)
        {
            if (Input.GetButtonDown("Shoot"))
                buffer.AddInput("Shoot");

            Interact();
        }
        else
            Skip();
    }


    private void Interact()
    {
        if(buffer.CheckButton("Shoot"))
        {
            Collider2D col = Physics2D.OverlapCircle(transform.position, interactionRadius, Interactable);

            if (col != null)
            {
                buffer.Executed("Shoot");
                CameraManager.Instance.CineMode(_transform);
                col.GetComponent<Interactable>().InteractStart();
            }
        }
    }

    private void Skip()
    {
        if (Input.GetButtonDown("Shoot"))
        {
            if(UIManager.Instance.DialogueBox.isActiveAndEnabled)
                UIManager.Instance.DialogueBox.SkipPressed();
        }
    }
}
