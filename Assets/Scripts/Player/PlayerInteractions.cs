using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    #region Variables
    public LayerMask Interactable;

    private Transform _transform;
    private InputBuffer buffer;
    private PlayerController controller;

    public float interactionRadius = 0.6f;
    #endregion


    private void Awake()
    {
        _transform = transform;
        buffer = GetComponent<InputBuffer>();
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        Skip();
    }

    private void Skip()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if(UIManager.Instance.DialogueBox.isActiveAndEnabled)
                UIManager.Instance.DialogueBox.SkipPressed();
        }
    }
}
