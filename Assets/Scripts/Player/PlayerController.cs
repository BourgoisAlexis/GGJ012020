using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    public Transform Visual;
    public Transform Weapon;
    public GameObject BulletPrefab;

    private Rigidbody2D rb;
    private SurfaceDetection surf;
    private InputBuffer buffer;
    private PlayerAnimation playerAnim;

    private float gravityMultiplier = 0.5f;
    private float accel = 0.6f;
    private float maxHSpeed = 7;
    private float maxVSpeed = 30;
    private float jumpHeight = 9;

    public bool canInput = true;
    private bool canShoot = false;
    private bool dead = false;

    private float InputH;
    private float currentHSpeed = 0;
    private float speedValue;
    private float currentVspeed = 0;
    #endregion


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        surf = GetComponent<SurfaceDetection>();
        buffer = GetComponent<InputBuffer>();
        playerAnim = GetComponent<PlayerAnimation>();

        Weapon.gameObject.SetActive(false);
    }

    private void Start()
    {
        Respawner.Instance.Respawn(transform);
    }

    private void Update()
    {
        if (canInput)
        {
            if (Input.GetButtonDown("Jump"))
                buffer.AddInput("Jump");

            if (Input.GetButtonDown("Shoot"))
                buffer.AddInput("Shoot");

            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1)
                InputH = Input.GetAxisRaw("Horizontal");
            else
                InputH = 0;
        }
        else
            InputH = 0;
    }

    private void FixedUpdate()
    {
        CustomGravity();
        ApplyPhysic();
        HMove();

        if(canInput)
        {
            Jump();
            Shoot();
        }

    }

    public void AllowShoot()
    {
        Weapon.gameObject.SetActive(true);
        canShoot = true;
    }


    private void CustomGravity()
    {
        if(rb.velocity.y != 0)
            currentVspeed -= gravityMultiplier;
        if (rb.velocity.y < 0)
            currentVspeed -= gravityMultiplier * 1.2f;
        if (currentVspeed > 0)
            if (!Input.GetButton("Jump"))
                currentVspeed -= gravityMultiplier * 0.8f;

        if (surf.GroundDetection() && currentVspeed < 0)
            currentVspeed = 0;
    }

    private void ApplyPhysic()
    {
        speedValue = Mathf.Clamp(speedValue, 0, maxHSpeed);
        currentHSpeed = speedValue * Mathf.Sign(currentHSpeed);

        currentVspeed = Mathf.Clamp(currentVspeed, -maxVSpeed, maxVSpeed);

        rb.velocity = new Vector2(speedValue * Mathf.Sign(currentHSpeed), currentVspeed);
    }

    private void Jump()
    {
        if (buffer.CheckButton("Jump"))
            if (surf.GroundDetection())
            {
                currentVspeed = jumpHeight;
                buffer.Executed("Jump");
                playerAnim.Jump();
            }
    }

    private void HMove()
    {
        if (InputH != 0)
        {
            if (Mathf.Sign(InputH) == Mathf.Sign(currentHSpeed))
                currentHSpeed += Mathf.Sign(InputH) * accel;
            else
                currentHSpeed += Mathf.Sign(InputH) * accel * 2;

            speedValue = Mathf.Abs(currentHSpeed);

            float direction = currentHSpeed < 0 ? 1 : 0;
            Visual.localEulerAngles = new Vector3(0, 180 * direction, 0);
        }
        else if(speedValue > 0)
                speedValue -= accel;
    }

    private void Shoot()
    {
        if (buffer.CheckButton("Shoot"))
            if(canShoot)
            {
                Instantiate(BulletPrefab, Weapon.position, Weapon.rotation);
                StartCoroutine(ResetShoot());
                CameraManager.Instance.Shake();
                buffer.Executed("Shoot");
            }
    }

    private IEnumerator ResetShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(0.2f);
        canShoot = true;
    }

    public void Death()
    {
        if(dead == false)
        {
            dead = true;
            canInput = false;
            UIManager.Instance.BlackFade();
            playerAnim.Death();
        }
    }
}
