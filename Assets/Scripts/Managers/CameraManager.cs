using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    #region Variables
    public Transform Player;
    public Transform CinematicTarget;

    private Transform _transform;

    private bool moving;
    private int margin = 10;
    private int panSpeed = 30;

    //Accessors
    public int PanSpeed => panSpeed;
    #endregion


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(gameObject);

        _transform = transform;
        moving = true;
    }

    private void FixedUpdate()
    {
        if (moving)
            FollowPlayer();
    }


    private void FollowPlayer()
    {
        _transform.position = Vector3.Lerp(_transform.position, Player.position, 0.4f);
        _transform.position -= new Vector3(0, 0, margin);
    }


    public void CineMode(Transform _target)
    {
        StartCoroutine(CineModing((Vector2)(_target.position - _transform.position)));
        UIManager.Instance.Cinemode(true);
    }

    public void CineModeEnd()
    {
        moving = true;
        Player.GetComponent<PlayerController>().canInput = true;
        UIManager.Instance.Cinemode(false);
    }

    public void Shake()
    {
        StartCoroutine(Shaking());
    }


    private IEnumerator Shaking()
    {
        int n = 0;
        float intensity = 0.15f;

        while (n < 3)
        {
            yield return new WaitForSeconds(0.03f);
            float x = Random.Range(-intensity, intensity);
            float y = Random.Range(-intensity, intensity);
            _transform.position += new Vector3(x, y, 0);
            n++;
        }
    }

    private IEnumerator CineModing(Vector2 _direction)
    {
        moving = false;
        Player.GetComponent<PlayerController>().canInput = false;
        int n = 0;

        while (n < panSpeed)
        {
            yield return new WaitForFixedUpdate();
            _transform.position += (Vector3)_direction / panSpeed;
            n++;
        }
    }
}
