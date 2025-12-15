using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using System;
using TMPro;


public class player : MonoBehaviour
{
    private float speed = 12f;
    private float horizontal;
    private float jumpingpower = 18f;
    private bool isfacingright = true;
    private int doublejump = 0;

    private bool iswallsliding;
    private float wallslidingspeed = 2f;

    private bool isdashing;
    private float dashSpeed = 35f;
    private float dashduration = 0.2f;
    private float dashCooldown = 1f;
    private bool canDash = true;
    private float currentDashCooldown;

    private bool iswalljumping;
    private float walljumpingdirection;
    private float walljumpingtime = 0.12f;
    private float walljumpingcounter;
    private float walljumpingduration = 0.4f;
    private Vector2 walljumpingpower = new Vector2(3.5f, 18f);

    [SerializeField] private int sticky;

    private float coyote;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private AudioClip jumpLow;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip jumpHigh;
    [SerializeField] private TrailRenderer tr;

    private Boolean playingWalkSound = false;

    public InputActionReference dash;
    public InputActionReference jump;
    public InputActionReference slowJump;
    public InputActionReference move;

    private void OnEnable()
    {
        if (canDash == true)
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
        if (sticky == 0)
        {
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
                if (doublejump > 0 && !isgrounded() && coyote <= 0f && rb.linearVelocity.y < jumpingpower)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingpower);
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
        if (isgrounded() && rb.linearVelocity.x != 0 && playingWalkSound == false)
        {
            StartCoroutine(playWalkingSound());
        }


    }

    IEnumerator playWalkingSound()
    {
        playingWalkSound = true;
        SoundManager.instance.PlaySoundFXClip(walkSound, transform, 1f);
        yield return new WaitForSeconds(0.25f);
        playingWalkSound = false;
    }

    private void FixedUpdate()
    {
        if (isdashing == true)
        {
            return;
        }

        if (!iswalljumping) //actually important movement transformation
        {
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        }
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
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingpower);
        doublejump = 1;
        SoundManager.instance.PlaySoundFXClip(jumpLow, transform, 1f);
    }


    //Dash stuff
    private IEnumerator Dash()
    {
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
}