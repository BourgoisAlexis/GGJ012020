using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    #region Variables
    public RectTransform Up;
    public RectTransform Down;

    private int speed;
    #endregion

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(gameObject);
    }

    private void Start()
    {
        speed = CameraManager.Instance.PanSpeed;
    }

    public void Cinemode()
    {
        StartCoroutine(CineModing());
    }

    public IEnumerator CineModing()
    {
        int n = 0;

        while (n < speed)
        {
            yield return new WaitForFixedUpdate();
            Up.localPosition -= Vector3.up * 500 / speed;
            Down.localPosition += Vector3.up * 500 / speed;
            n++;
        }

        yield return new WaitForSeconds(1);

        n = 0;

        while (n < speed/2)
        {
            yield return new WaitForFixedUpdate();
            Up.localPosition += Vector3.up * 1000 / speed;
            Down.localPosition -= Vector3.up * 1000 / speed;
            n++;
        }
    }
}
