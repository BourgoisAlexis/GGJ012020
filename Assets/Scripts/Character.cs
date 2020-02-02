using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Interactable
{
    public string Name;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            InteractStart(other.transform);
    }

    public override void InteractStart(Transform _transform)
    {
        CameraManager.Instance.CineMode(_transform);
        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.DialogueBox.gameObject.SetActive(true);
        UIManager.Instance.DialogueBox.Setup(Name, 0);
    }
}
