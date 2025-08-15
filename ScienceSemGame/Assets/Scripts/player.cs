using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    private float speed = 8f;
    private float horizontal;
    private float jumpingpower = 16f;
    private bool isfacingright = true;
    private int doublejump = 0;

    private bool iswallsliding;
    private float wallslidingspeed = 2f;

    private bool iswalljumping;
    private float walljumpingdirection;
    private float walljumpingtime = 0.2f;
    private float walljumpingcounter;
    private float walljumpingduration = 0.4f;
    private Vector2 walljumpingpower = new Vector2(3.5f, 16f);

    [SerializeField] private float coyote;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private AudioClip jumpLow;
    [SerializeField] private AudioClip jumpHigh;

    public InputActionReference dash;
    public InputActionReference jump;

    private void OnEnable()
    {
        jump.action.started += Jump;
    }
    private void OnDisable()
    {
        jump.action.started -= Jump;
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        Debug.Log("jumped");
        if (isgrounded() || coyote > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingpower);
            doublejump = 1;
            SoundManager.instance.PlaySoundFXClip(jumpLow, transform, 1f);
            if (doublejump > 0 && !isgrounded() && coyote <= 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingpower);
                doublejump--;
                SoundManager.instance.PlaySoundFXClip(jumpHigh, transform, 1f);
            }
        }

    }
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && isgrounded() || Input.GetButtonDown("Jump") && coyote > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingpower);
            doublejump = 1;
            SoundManager.instance.PlaySoundFXClip(jumpLow, transform, 1f);
        }


        if (Input.GetButtonDown("Jump") && doublejump > 0 && !isgrounded() && coyote <= 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingpower);
            doublejump--;
            SoundManager.instance.PlaySoundFXClip(jumpHigh, transform, 1f);
        }

        wallslide();
        walljump();

        if (!iswalljumping)
        {
            Flip();
        }


        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (isgrounded())
        {
            doublejump = 1;
            coyote = walljumpingtime;
        }
        else
        {
            coyote -= Time.deltaTime;
        }

        if (iswallsliding)
        {
            doublejump = 1;
        }


    }

    private void FixedUpdate()
    {
        if (!iswalljumping)
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
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);
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

        if (Input.GetButtonDown("Jump") && walljumpingcounter > 0f)
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

    private void stopwalljumping()
    {
        iswalljumping = false;
    }

    private void Flip()
    {
        if (isfacingright && horizontal < 0f || !isfacingright && horizontal > 0f)
        {
            isfacingright = !isfacingright;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
