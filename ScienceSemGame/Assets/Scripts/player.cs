using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;


public class player : MonoBehaviour
{
    private float speed = 8f;
    private float horizontal;
    private float jumpingpower = 16f;
    private bool isfacingright = true;
    private int doublejump = 0;

    private bool iswallsliding;
    private float wallslidingspeed = 2f;

    private bool isdashing;
    private float dashSpeed = 30f;
    private float dashduration = 0.2f;
    private float dashCooldown = 1.25f;
    private bool canDash = true;
    [SerializeField] private float currentDashCooldown;

    private bool iswalljumping;
    private float walljumpingdirection;
    private float walljumpingtime = 0.2f;
    private float walljumpingcounter;
    private float walljumpingduration = 0.4f;
    private Vector2 walljumpingpower = new Vector2(3.5f, 16f);

    private float coyote;

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

    private void OnEnable()
    {
        if(canDash == true)
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
        if(canDash && !isdashing)
        {
            StartCoroutine(Dash());
        }
    }
    private void Jump(InputAction.CallbackContext obj) //jumping
    {

        if (isgrounded() || coyote > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingpower);
            doublejump = 1;
            SoundManager.instance.PlaySoundFXClip(jumpLow, transform, 1f);
           
        }
        if (doublejump > 0 && !isgrounded() && coyote <= 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingpower);
            doublejump--;
            SoundManager.instance.PlaySoundFXClip(jumpHigh, transform, 1f);
        }
        if (walljumpingcounter > 0f)
        {
            iswalljumping = true;
            rb.linearVelocity = new Vector2(walljumpingdirection * walljumpingpower.x, walljumpingpower.y);
            walljumpingcounter = 0f;
            doublejump++;

            if (transform.localScale.x != walljumpingdirection)
            {
                isfacingright = !isfacingright;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(stopwalljumping), walljumpingduration);
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
        if(isdashing == true)
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
            if(currentDashCooldown <= 0f)
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
}
