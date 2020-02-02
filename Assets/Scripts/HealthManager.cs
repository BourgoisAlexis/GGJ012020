using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public GameObject[] hearts;

    public void ChangeState(int _HP)
    {
        _HP += 2;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < _HP)
                hearts[i].SetActive(true);
            else
                hearts[i].SetActive(false);
        }

        if (_HP <= 0)
            CameraManager.Instance.Player.GetComponent<PlayerController>().Death();
    }
}
