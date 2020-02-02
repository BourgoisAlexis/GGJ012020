using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    private int index = -1;

    public int Index
    {
        get
        {
            return index;
        }
        set
        {
            if (index < 0)
                index = value;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Respawner.Instance.UpdateIndex(index);
        }
    }
}
