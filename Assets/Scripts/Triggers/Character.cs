using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Interactable
{
    public string Name;
    private bool canTalk = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(canTalk)
            if (other.CompareTag("Player"))
                InteractStart(other.transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            gameObject.SetActive(false);
    }

    public override void InteractStart(Transform _transform)
    {
        CameraManager.Instance.CineMode(_transform);
        StartCoroutine(StartDialogue());

        canTalk = false;
    }

    private IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.DialogueBox.gameObject.SetActive(true);
        UIManager.Instance.DialogueBox.Use(Name, 0);
    }
}
