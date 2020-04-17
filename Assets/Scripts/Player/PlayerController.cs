using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Transform Visual;
    [SerializeField]
    private Transform Weapon;
    [SerializeField]
    private GameObject BulletPrefab;

    private PlayerAnimation playerAnim;
    private SurfaceDetection surf;
    private InputBuffer buffer;
    private Rigidbody2D rb;

    private float gravityMultiplier = 0.5f;
    private float accel = 0.6f;
    private float maxHSpeed = 4;
    private float maxVSpeed = 30;
    private float jumpHeight = 7;

    private float InputH;
    private float speedValue;
    private float currentHSpeed = 0;
    private float currentVspeed = 0;

    public bool canInput = true;
    private bool canShoot = false;
    private bool dead = false;
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
        Inputs();
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


    private void Inputs()
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

    public void AllowShoot()
    {
        Weapon.gameObject.SetActive(true);
        canShoot = true;
    }

    private void CustomGravity()
    {
        if(rb.velocity.y != 0)
        {
            currentVspeed -= gravityMultiplier;

            if (rb.velocity.y < 0)
                currentVspeed -= gravityMultiplier * 1.2f;

            else if (currentVspeed > 0)
                if (!Input.GetButton("Jump"))
                    currentVspeed -= gravityMultiplier * 0.8f;
        }

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
                AudioManager.Instance.PlaySound("Jump", 1);
            }
    }

    private void HMove()
    {
        if (InputH != 0)
        {
            float inputSign = Mathf.Sign(InputH);

            if (inputSign == Mathf.Sign(currentHSpeed))
                currentHSpeed += inputSign * accel;
            else
                currentHSpeed += inputSign * accel * 2;

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
                StartCoroutine(ShootDelay());
                CameraManager.Instance.Shake();
                buffer.Executed("Shoot");
                AudioManager.Instance.PlaySound("Shoot", 1);
            }
    }

    public void Death()
    {
        if(!dead)
        {
            dead = true;
            canInput = false;
            UIManager.Instance.BlackFade(true);
            playerAnim.Death();
            AudioManager.Instance.PlaySound("GameOver", 1);
        }
    }


    //Coroutines
    private IEnumerator ShootDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(0.2f);
        canShoot = true;
    }
}
