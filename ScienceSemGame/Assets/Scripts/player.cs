using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;


public class player : MonoBehaviour
{
    private float speed = 2f;
    private float horizontal;
    private float jumpingpower = 21f;
    private bool isfacingright = true;
    private int doublejump = 0;

    private bool iswallsliding;
    private float wallslidingspeed = 2f;

    private bool isdashing;
    private float dashSpeed = 35f;
    private float dashduration = 0.2f;
    private float dashCooldown = 0.6f;
    private bool canDash = true;
    private float currentDashCooldown;

    private float jumpCooldown;

    private bool iswalljumping;
    private float walljumpingdirection;
    private float walljumpingtime = 0.12f;
    private float walljumpingcounter;
    private float walljumpingduration = 0.4f;
    [SerializeField] private Vector2 walljumpingpower;

    private int sticky;

    private float coyote;

    [SerializeField] private float friction;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private AudioClip jumpLow;
    [SerializeField] private AudioClip jumpHigh;
    [SerializeField] private TrailRenderer tr;

    public InputActionReference dash;
    public InputActionReference jump;
    public InputActionReference slowJump;
    public InputActionReference move;

    [SerializeField] private float stamina = 50f;
    private float maxStamina = 50f;
    public int life = 3;
    private float invincibility;

    private void OnEnable()
    {
        if (canDash == true && stamina >= 10f)
        {
            dash.action.performed += OnDash;
        }
        jump.action.started += Jump;
        slowJump.action.canceled += SlowJump;
        move.action.Enable();
    }
    private void OnDisable()
    {
        dash.action.performed -= OnDash;
        jump.action.started -= Jump;
        slowJump.action.canceled -= SlowJump;
        move.action.Disable();
    }

    private void OnDash(InputAction.CallbackContext obj)//dashing
    {
        if (canDash && !isdashing)
        {
            StartCoroutine(Dash());
        }
    }
    private void Jump(InputAction.CallbackContext obj) //jumping
    {
        if(sticky == 0 && jumpCooldown <= 0f)
        {
        jumpCooldown = 0.15f;
        if (isgrounded() || coyote > 0f)
        {
            Jump();
        }

        if (walljumpingcounter > 0f)
        {
            iswalljumping = true;
            rb.linearVelocity = new Vector2(walljumpingdirection * walljumpingpower.x, walljumpingpower.y);
            walljumpingcounter = 0f;
            doublejump = 1;
            SoundManager.instance.PlaySoundFXClip(jumpLow, transform, 1f);

            if (transform.localScale.x != walljumpingdirection)
            {
                isfacingright = !isfacingright;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
            Invoke(nameof(stopwalljumping), walljumpingduration);
        }
        else
        {
            if (doublejump > 0 && !isgrounded() && coyote <= 0f )
            {
                rb.AddForce(Vector2.up * jumpingpower, ForceMode2D.Impulse);
                doublejump--;
                SoundManager.instance.PlaySoundFXClip(jumpHigh, transform, 1f);
            }
        }
        }
    }

    private void SlowJump(InputAction.CallbackContext context)
    {
        if (rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }


    void Update()
    {
        invincibility -= Time.deltaTime;
        jumpCooldown -= Time.deltaTime;
        if(stamina > maxStamina)
        {
            if (stamina > maxStamina + 25) {
                stamina = maxStamina + 25;
            }
                
            stamina -= Time.deltaTime;
            if(stamina < maxStamina)
            {
                stamina = maxStamina;
            }
        }



        if (isdashing == true)
        {
            return;
        }
        horizontal = move.action.ReadValue<float>();



        wallslide();
        walljump();

        if (!iswalljumping)
        {
            Flip();
        }

        currentDashCooldown -= Time.deltaTime;

        if (isgrounded())
        {
            if (currentDashCooldown <= 0f)
            {
                canDash = true;
            }
            doublejump = 1;
            coyote = walljumpingtime;
        }
        else
        {
            coyote -= Time.deltaTime;
        }

        if (iswallsliding)
        {
            if (currentDashCooldown <= 0f)
            {
                canDash = true;
            }
            doublejump = 1;
        }


    }

    private void FixedUpdate()
    {
        if (isdashing == true)
        {
            return;
        }

        if (!iswalljumping) //actually important movement transformation
        {
            Vector2 horizontalVector = new Vector2(horizontal * speed, 0);
            rb.AddForce(horizontalVector, ForceMode2D.Impulse);
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x * friction, rb.linearVelocity.y);
    }

    private bool isgrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }



    private bool iswalled()
    {
        Collider2D hit = Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer); //slide but not on oneways
        if (hit == null)
        {
            return false;
        }
        if (hit.CompareTag("OneWayPlatform"))
        {
            return false;
        }
        return true;
    }
    private void wallslide()
    {
        if (iswalled() && !isgrounded() && horizontal != 0f)
        {
            iswallsliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallslidingspeed, float.MaxValue));
        }
        else
        {
            iswallsliding = false;
        }
    }

    private void walljump()
    {
        if (iswallsliding)
        {
            iswalljumping = false;
            walljumpingdirection = transform.localScale.x;
            walljumpingcounter = walljumpingtime;

            CancelInvoke(nameof(stopwalljumping));
        }
        else
        {
            walljumpingcounter -= Time.deltaTime;
        }


    }

    private void stopwalljumping() //relic of a bygone era
    {
        iswalljumping = false;
    }

    private void Flip() //turns around
    {
        if (isfacingright && horizontal < 0f || !isfacingright && horizontal > 0f)
        {
            isfacingright = !isfacingright;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void Jump()
    {
            rb.AddForce(Vector2.up * jumpingpower, ForceMode2D.Impulse);
            doublejump = 1;
            SoundManager.instance.PlaySoundFXClip(jumpLow, transform, 1f);
    }


    //Dash stuff
    private IEnumerator Dash()
    {
        stamina -= 10f;
        canDash = false;
        isdashing = true;
        float previousverticalmove = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(-transform.localScale.x * dashSpeed, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashduration);
        tr.emitting = false;
        rb.gravityScale = previousverticalmove;
        isdashing = false;
        currentDashCooldown = dashCooldown;
    }

    //sticky
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<tags>(out tags touchedComponent))
        {
            if (touchedComponent.sticky == true)
            {
                sticky += 1;
            }

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<tags>(out tags touchedComponent))
        {
            if (touchedComponent.sticky == true)
            {
                sticky -= 1;
            }

        }
    }

    public void Damaged()
    {
        if(invincibility <= 0f)
        {
        life -= 1;
        invincibility = 0.1f;
            rb.AddForce(Vector2.up * jumpingpower * 0.5f, ForceMode2D.Impulse);
        }
    }
}