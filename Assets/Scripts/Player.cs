using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    public SpriteRenderer playerSprite;
    public GameObject deadPrefab;
    public AudioSource landingAudio;

    [Header("Skills")]
    public bool jumpSkill = false;
    public bool skillDash = false;

    [Header("Movement")]
    public float speedUpTime = 0.2F;
    public float jumpMaxAccumulate = 0.1F;
    public float jumpForce   = 200.0F;
    public float jumpCooldown   = 0.1F; 
    public float runningVelocity = 1.0F;

    public float dashCharge = 0.5F;
    public float dashSpeed = 20.0F;
    public float dashFlightTime = 0.5F;
    public bool canDash = false;

    public float elevateGravityRatio = 0.8F;
    public float fallGravityRatio = 1.25F;


    [Header("Jump Debug")]
    public bool grounded = false;
    bool groundedBefore = false;

    [SerializeField] bool canJump = true;
    [SerializeField] float jumpCooldownTimer = -1.0F;
    [SerializeField] float jumpAccumulateTimer = -1.0F;

    private Rigidbody2D _rb;
    private Animator _anim;
    private Transform _contacts;

    private float x_vel = 0.0F;

    float restartTimer = -1.0F;
    float dashTimer = 0.0F;
    float dashFlightTimer = 0.0F;

    private void Awake()
    {
        _rb = this.GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        _contacts = transform.Find("Contacts");
    }

    // Start is called before the first frame update
    void Start()
    {
        restartTimer = -1.0F;
    }

    private void OnEnable()
    {
        Permanent[] ps = FindObjectsOfType<Permanent>();
        Permanent p = FindObjectOfType<Permanent>();
        foreach (Permanent.DeathData d in p.deathPositions)
        {
            Debug.Log("IN");
            GameObject go = (GameObject)Instantiate(deadPrefab, d.position, d.rotation, null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (restartTimer > 0.0F)
        {
            restartTimer -= Time.deltaTime;
            if (restartTimer < 0.0F)
            {
                FindObjectOfType<Permanent>().AddDeath(playerSprite.gameObject.transform);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            return;
        }

        if (!groundedBefore && grounded) 
        {
            landingAudio.time = 0;
            landingAudio.pitch = 0.9F + 0.2F * Random.value;
            landingAudio.Play();
        }
        groundedBefore = grounded;

        if (!groundedBefore && grounded && dashFlightTimer > 0) dashFlightTimer = 0.0F;
        if (grounded && !canDash) canDash = true;
        if (grounded && jumpCooldownTimer > 0.0F) jumpCooldownTimer -= Time.deltaTime;
        if (grounded && !canJump && jumpCooldownTimer <= 0.0F) canJump = true;
        CheckJump();
        CheckDash();

        // Update Animator
        _anim.SetBool("Moving", Mathf.Abs(_rb.velocity.x) > 0.01);
        _anim.SetBool("Grounded", grounded);
    }

    void ComputeVelocityX()
    {
        if (dashFlightTimer > 0)
        {
            dashFlightTimer -= Time.fixedDeltaTime;
            return;
        }


        float target = Input.GetAxis("Horizontal") * runningVelocity;

        if (target > x_vel)
        {
            x_vel += runningVelocity * Time.fixedDeltaTime / speedUpTime;
            if (target < x_vel) x_vel = target;
        }
        else if (target < x_vel)
        {
            x_vel -= runningVelocity * Time.fixedDeltaTime / speedUpTime;
            if (target > x_vel) x_vel = target;
        }

        if (x_vel < 0)
        {
            _contacts.localScale = new Vector3(-1.0F, 1.0F, 1.0F);
            playerSprite.flipX = true;
        }
        else if (x_vel > 0)
        {
            _contacts.localScale = new Vector3(1.0F, 1.0F, 1.0F);
            playerSprite.flipX = false;
        }
    }

    void CheckJump()
    {
        if (!jumpSkill || !canJump) return;

        if (Input.GetButtonDown("Jump")) { jumpAccumulateTimer = 0.0F; }

        if (jumpAccumulateTimer > -1.0F)
        {
            if (jumpAccumulateTimer > jumpMaxAccumulate) Jump(jumpForce);
            else if (Input.GetButtonUp("Jump")) Jump(jumpForce * jumpAccumulateTimer / jumpMaxAccumulate);
            
            jumpAccumulateTimer += Time.deltaTime;
        }


    }

    void Jump(float value)
    {
        _rb.velocity = new Vector2(_rb.velocity.x, value);

        canJump = false;
        jumpCooldownTimer = jumpCooldown;
        jumpAccumulateTimer = -2.0F;
    }

    protected override void FixedUpdate() 
    {
        ComputeVelocityX();

        float y_vel = _rb.velocity.y;

        if (y_vel > 2) _rb.gravityScale = elevateGravityRatio;
        else if (y_vel < -2) _rb.gravityScale = fallGravityRatio;
        else _rb.gravityScale = 1.0F;

        _rb.velocity = new Vector2(x_vel, y_vel);
    }

    public void Die()
    {
        restartTimer = 1.0F;
        _anim.SetTrigger("Die");
    }

    void CheckDash()
    {
        if (!skillDash || !canDash) return;

        if (Input.GetButtonDown("Fire1")) dashTimer = dashCharge;
        else if (Input.GetButtonUp("Fire1")) Dash();
        else if (Input.GetButton("Fire1")) dashTimer = Mathf.Max(dashTimer - Time.deltaTime, 0.0F);


    }

    void Dash()
    {
        Vector3 diff = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position);

        Vector2 v = new Vector2(diff.x, diff.y);

        if (v.magnitude > 1.0F)
            v = v.normalized * dashSpeed * (1 - dashTimer) / dashCharge;
        else
            v = v * dashSpeed * (1 - dashTimer) / dashCharge;

        x_vel = v.x;
        _rb.velocity = new Vector2(v.x, v.y);
        dashFlightTimer = dashFlightTime;
        canDash = false;
    }
}
