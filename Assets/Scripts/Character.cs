using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Interactable
{
    public string Name;

    public override void InteractStart()
    {
        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(0.4f);
        UIManager.Instance.DialogueBox.gameObject.SetActive(true);
        UIManager.Instance.DialogueBox.Setup(Name, 0);
    }
}
