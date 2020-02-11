using UnityEngine;

public class Activater : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ToActivate;

    public void Activation()
    {
        for (int i = 0; i < ToActivate.Length; i++)
            ToActivate[i].SetActive(true);
    }
}
