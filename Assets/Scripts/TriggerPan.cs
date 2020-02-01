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
            CameraManager.Instance.Pan(target);
            UIManager.Instance.Cinemode();
            gameObject.SetActive(false);
        }
    }
}
