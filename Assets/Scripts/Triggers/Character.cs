using System.Collections;
using UnityEngine;

public class Character : Interactable
{
    #region Variables
    [SerializeField]
    private string Name;

    private bool canTalk = true;
    private Animator animator;
    #endregion


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(canTalk)
            if (other.CompareTag("Player"))
            {
                CameraManager.Instance.CineMode(true, other.transform);
                StartCoroutine(StartDialogue());
                animator.SetTrigger("Trigger");
                canTalk = false;
            }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(Disappear());
    }


    private IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.DialogueBox.gameObject.SetActive(true);
        UIManager.Instance.DialogueBox.Use(Name, 0);
    }

    private IEnumerator Disappear()
    {
        animator.SetTrigger("Trigger");

        yield return new WaitForSeconds(0.2f);

        gameObject.SetActive(false);
    }
}
