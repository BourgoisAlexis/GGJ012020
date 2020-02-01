using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    #region Variables
    public Transform Visual;

    private Transform _transform;
    private Rigidbody2D rb;

    private float maxSpeed = 30;
    private float accel = 5;
    private float currentSpeed = 0;
    #endregion


    private void Awake()
    {
        _transform = transform;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    private void FixedUpdate()
    {
        if (currentSpeed < maxSpeed)
            currentSpeed += accel;

        rb.velocity = new Vector2(_transform.right.x * currentSpeed, 0);
        float scale = currentSpeed / 8;
        Visual.localScale = new Vector3(scale * 2, 1 / scale, 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
            other.GetComponent<Enemy>().Killed();
    }
}
