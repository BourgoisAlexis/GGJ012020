using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPan : MonoBehaviour
{
    public Transform target;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraManager.Instance.CineMode(true, target);
            StartCoroutine(End());
        }
    }

    private IEnumerator End()
    {
        yield return new WaitForSeconds(2f);
        CameraManager.Instance.CineMode(false, null);
        gameObject.SetActive(false);
    }
}
