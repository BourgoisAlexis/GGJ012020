using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    private Transform _transform;
    private Rigidbody2D rb;
    #endregion


    private float ejectionForce = 500;

    private void Awake()
    {
        _transform = transform;
        rb = GetComponent<Rigidbody2D>();
    }

    public void Killed()
    {
        rb.AddForce(_transform.up * ejectionForce);
    }
}
