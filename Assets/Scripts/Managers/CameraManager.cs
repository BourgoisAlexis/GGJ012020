using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    #region Variables
    public Transform Player;

    private PlayerController playerController;
    private Transform _transform;
    private bool followPlayer;
    private int zOffset = -10;
    private float smoothingFactor = 0.4f;
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
        playerController = Player.GetComponent<PlayerController>();
        followPlayer = true;
    }

    private void FixedUpdate()
    {
        if (followPlayer)
            FollowPlayer();

        AdjustZ();
    }


    private void FollowPlayer()
    {
        _transform.position = Vector3.Lerp(_transform.position, Player.position, smoothingFactor);
    }

    private void AdjustZ()
    {
        _transform.position = new Vector3(_transform.position.x, _transform.position.y, zOffset);
    }

    public void CineMode(bool _cine, Transform _target)
    {
        if(_cine)
        {
            StartCoroutine(CineModing(_target.position - _transform.position));
            UIManager.Instance.Cinemode(true);
        }
        else
        {
            StartCoroutine(CineModingEnd());
            UIManager.Instance.Cinemode(false);
        }
    }

    public void Shake()
    {
        StartCoroutine(Shaking());
    }


    //Coroutines
    private IEnumerator CineModing(Vector3 _direction)
    {
        followPlayer = false;
        playerController.canInput = false;
        int n = 0;

        while (n < panSpeed)
        {
            yield return new WaitForFixedUpdate();
            _transform.position += _direction / panSpeed;
            n++;
        }
    }

    private IEnumerator CineModingEnd()
    {
        yield return new WaitForSeconds(0.1f);
        followPlayer = true;
        playerController.canInput = true;
    }

    private IEnumerator Shaking()
    {
        Vector3 initPos = _transform.position;

        int n = 0;
        float intensity = 0.08f;

        while (n < 3)
        {
            yield return new WaitForSeconds(0.03f);
            float x = Random.Range(-intensity, intensity);
            float y = Random.Range(-intensity, intensity);
            _transform.position += new Vector3(x, y, 0);
            n++;
        }

        if (!followPlayer)
            _transform.position = initPos;
    }
}
