using UnityEngine;
using Pixeye.Unity;
using System.Collections;

public class Enemy : MonoBehaviour
{
    #region Variables
    public LayerMask Player;
    public LayerMask Wall;
    public SpriteRenderer Visual;

    private Transform _transform;
    private Rigidbody2D rb;

    private float ejectionForce = 500;
    private bool dead;
    private bool canAttack = true;
    private float wallDetection = 0.6f;
    private float detectionRadius = 1.2f;
    private float speed = 2;
    private Transform target;

    [Foldout("Sprites", true)]
    public Sprite Idle, Death, Atk;
    #endregion


    private void Awake()
    {
        _transform = transform;
        rb = GetComponent<Rigidbody2D>();
        Visual.sprite = Idle;
    }

    private void FixedUpdate()
    {
        if (!dead)
        {
            if(DetectPlayer())
                Aggro();
            else
                Patrol();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(!dead)
            other.gameObject.GetComponent<PlayerController>()?.Death();
    }


    public void Killed()
    {
        rb.AddForce(_transform.up * ejectionForce);
        tag = "Player";
        dead = true;
        Visual.sprite = Death;
    }

    private void Patrol()
    {
        Visual.sprite = Idle;
        rb.velocity = new Vector2(_transform.right.x * speed, rb.velocity.y);

        if(Physics2D.Raycast(_transform.position, _transform.right, wallDetection, Wall))
        {
            _transform.eulerAngles += new Vector3(0, 180, 0);
        }
    }

    private bool DetectPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(_transform.position, detectionRadius, Player);

        if (player != null)
        {
            target = player.transform;
            return true;
        }

        target = null;
        return false;
    }

    private void Aggro()
    {
        if (canAttack)
            StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        Visual.sprite = Atk;
        Vector2 dif = target.position - _transform.position;

        if (Mathf.Sign(_transform.right.x) != Mathf.Sign(dif.x))
            _transform.eulerAngles += new Vector3(0, 180, 0);

        Vector2 direction = new Vector2(1 * Mathf.Sign(dif.x), 1);

        yield return new WaitForSeconds(0.1f);
        if(DetectPlayer())
            rb.AddForce(direction * ejectionForce * 0.8f);


        yield return new WaitForSeconds(0.2f);
        Visual.sprite = Idle;
        yield return new WaitForSeconds(0.3f);
        canAttack = true;
    }
}
