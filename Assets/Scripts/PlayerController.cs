using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rbody;
    private BoxCollider2D boxCol;
    private Animator anim;

    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public Transform RightBoob;
    public Transform Hall1Spawn;
    private Vector2 originalVelocity;
    public float playerSpeed;
    public float jumpPower;
    public float jumpSpeed;
    public float playerScaleX;
    public float playerScaleY;
    public float accel = 0.2f;

    private float horizontalInput;

    // Double jumps
    public int extraJumps;
    private int jumpCounter;
    public float jumpBuffer;
    public int canDoubleJump;

    // Coyote time (if you walk off a cliff you can still jump after)
    public float coyoteTimer;
    private float coyoteCounter;

    // Wall jumping
    public float wallJumpX;
    public float wallJumpY;
    public float wallSlidingSpeed;
    bool lastWallState = false;
    bool canWallJump = false;

    // Dash
    private float dashDirection = 1;
    private float gravity;
    bool canDash = true;
    bool isDashing;
    IEnumerator dashCoroutine;

    // Dodge
    private float dodgeDirection = -1;
    bool canDodge = true;
    bool isDodging;
    IEnumerator dodgeCoroutine;

    // Up dash
    bool canUpDash = true;
    bool isUpDashing;

    // Health and stuff
    private bool vulnerable = true;
    private int playerHealth = 5;
    private float invulnerabilityTime = 1.2f;
    private bool hurt = false;
    private int currentPlayerHealth;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        gravity = rbody.gravityScale;
        originalVelocity = rbody.velocity;

        currentPlayerHealth = playerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // BASIC MOVEMENT

        // A + D or left and right arrow keys to move left and right
        horizontalInput = Input.GetAxis("Horizontal");
        if (!isDashing && !isDodging && !isUpDashing)
        {
            float playerSpeedX = rbody.velocity.x;
            if (horizontalInput != 0.0f && playerSpeedX < playerSpeed && playerSpeedX > -playerSpeed)
            {
                playerSpeedX = playerSpeedX + (playerSpeed * accel) * horizontalInput;
            } else
            {
                float incr = playerSpeed * accel;
                if (playerSpeedX > -incr && playerSpeedX < incr)
                {
                    playerSpeedX = 0;
                } else if (playerSpeedX < 0)
                {
                    playerSpeedX += incr;
                } else
                {
                    playerSpeedX -= incr;
                }
            }
            rbody.velocity = new Vector2(playerSpeedX, rbody.velocity.y);
        }
        if (isGrounded() || onWall()) {
            jumpBuffer = 0;
            jumpCounter = 1;
            if (isGrounded()) {
                canDoubleJump = 0;
            } else {
                canDoubleJump = 1;
            }
        } else {
            jumpBuffer += Time.deltaTime;
        }

        // Flip player in the direction they're moving in
        if (horizontalInput > 0.0f)
        {
            transform.localScale = new Vector3(playerScaleX, playerScaleY, 1);
            dashDirection = 1;
            dodgeDirection = -1;
        } 
        else if (horizontalInput < -0.0f)
        {
            transform.localScale = new Vector3(-playerScaleX, playerScaleY, 1);
            dashDirection = -1;
            dodgeDirection = 1;
        }

        // Setting animator parameters
        anim.SetBool("PlayerRun", horizontalInput != 0);
        anim.SetBool("Grounded", isGrounded());
        anim.SetBool("WallJumping", onWall());


        // GAINED ABILITIES

        // Dashing (forward)
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash == true)
        {
            anim.SetTrigger("Dashing");
            if (dashCoroutine != null)
            {
                StopCoroutine(dashCoroutine);
            }
            // (duration, cooldown)
            dashCoroutine = Dash(.25f, .5f);
            StartCoroutine(dashCoroutine);
        }
        // Dodging (backward)
        if (Input.GetKeyDown(KeyCode.LeftControl) && canDodge == true)
        {
            anim.SetTrigger("Dodging");
            if (dodgeCoroutine != null)
            {
                StopCoroutine(dodgeCoroutine);
            }
            // (duration, cooldown)
            dodgeCoroutine = Dodge(.1f, .5f);
            StartCoroutine(dodgeCoroutine);
        }
        // Up-dash
        if (Input.GetKeyDown(KeyCode.C) && canUpDash == true)
        {
            //UpDash();
            anim.SetTrigger("UpDashing");
            isUpDashing = true;
            rbody.velocity = Vector2.zero;
            rbody.gravityScale = 0;
            rbody.AddForce(new Vector2(0, 50), ForceMode2D.Impulse);
        }

        
        // JUMPING STUFF

        // Jumping
        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }

        // Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && rbody.velocity.y > 0)
        {
            rbody.velocity = new Vector2(rbody.velocity.x, rbody.velocity.y / 2);
            canDoubleJump = 1;
        }

        if (onWall())
        {
            rbody.velocity = new Vector2(rbody.velocity.x, Mathf.Clamp(rbody.velocity.y, -wallSlidingSpeed, float.MaxValue));
            //canWallJump = 1;
            //rbody.gravityScale = 0;
            //rbody.velocity = Vector2.zero;
        }
        else
        {
            rbody.gravityScale = 2.5f;
            //rbody.velocity = new Vector2(horizontalInput * playerSpeed, rbody.velocity.y);
            
            // Reset counters when grounded
            if (isGrounded())
            {
                coyoteCounter = coyoteTimer;
                jumpCounter = extraJumps;
            }
            else
            {
                // Decrease the coyote counter when the player is not grounded
                coyoteCounter -= Time.deltaTime;
            }
        }


        // MELEE

        // Click to attack
        if (Input.GetMouseButtonDown(0) && Input.GetAxis("Vertical") == 0)
        {
            anim.SetTrigger("Attacking(trig)");
        }

        // Up-swipe
        if (Input.GetMouseButtonDown(0) && Input.GetAxis("Vertical") > 0)
        {
            anim.SetTrigger("UpAttacking");
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rbody.AddForce(new Vector2(dashDirection * 20, 0), ForceMode2D.Impulse);
        }

        if (isDodging)
        {
            rbody.AddForce(new Vector2(dodgeDirection * 22, 0), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (vulnerable == true && currentPlayerHealth > 0)
            {
                hurt = true;
                currentPlayerHealth--;
                Debug.Log("Player health = " + currentPlayerHealth);
                Debug.Log("Player will be invulnerable!");
                StartCoroutine(Invulnerability());
            }
            //else if (currentPlayerHealth <= 0)
            //{

            //}
        }
    }

    // Jump mechanics (all the deets)
    private void Jump()
    {
        // If you aren't on a wall or doing anything crazy, don't do anything
        if (coyoteCounter <= 0 && !onWall() && jumpCounter <= 0) return;

        anim.SetTrigger("Jump");

        // Add sound for jump?
        if (onWall())
        {
            WallJump();
        }
        else
        {
            if (isGrounded()) {

                rbody.velocity = new Vector2(rbody.velocity.x, jumpPower);
            } else {
                // If you are within the .25 sec time frame, you can do the coyote jump
                if (coyoteCounter < 0.4 && coyoteCounter > 0)
                {
                    rbody.velocity = new Vector2(rbody.velocity.x, jumpPower);
                    coyoteCounter = -1;
                }
                else 
                {
                    // If you have extra jumps left, you can double jump
                    if (jumpCounter > 0 && jumpBuffer > 0.25 && canDoubleJump == 1)
                    {
                        rbody.velocity = new Vector2(rbody.velocity.x, 0);
                        rbody.velocity = new Vector2(rbody.velocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }

            // reset the counter to avoid double jump
            coyoteCounter = 0;
        }
    }

    private void WallJump()
    {
        /*if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(-playerScaleX, playerScaleY, 1);
        }  */

        canWallJump = false;
        //rbody.velocity = new Vector2(0, 0);
        Debug.Log("wallJump " + horizontalInput);
        //rbody.AddForce(new Vector2(dodgeDirection * 22, 5), ForceMode2D.Impulse);
        float playerSpeedX = playerSpeed * dodgeDirection * 5;
        rbody.velocity = new Vector2(playerSpeedX, jumpPower);

        //rbody.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) + wallJumpX, jumpPower);
    }

    // Check if player is on the ground
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    // Check if player is on a wall.
    private bool onWall()
    {
        bool currentWallState = Physics2D.OverlapCircle(RightBoob.position, RightBoob.GetComponent<CircleCollider2D>().radius, wallLayer);
        
        if (currentWallState != lastWallState)
        {
            lastWallState = currentWallState;
            
            if (lastWallState == true)
            {
                // transition from off the wall to on the wall
                canWallJump = true;
            }
            else
            {
                // transtion from on the wall to off the wall
                canWallJump = false;
            }
        }

        return canWallJump;

        //RaycastHit2D raycastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        //RaycastHit2D raycastHit2 = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0, new Vector2(-transform.localScale.x, 0), 0.1f, wallLayer);
        //return (raycastHit.collider != null && raycastHit2.collider != null);
    }

    IEnumerator Dash(float dashDuration, float dashCooldown)
    {
        isDashing = true;
        canDash = false;
        rbody.gravityScale = 0;
        rbody.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        rbody.gravityScale = gravity;
        rbody.velocity = originalVelocity;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    IEnumerator Dodge(float dodgeDuration, float dodgeCooldown)
    {
        isDodging = true;
        canDodge = false;
        rbody.gravityScale = 0;
        rbody.velocity = Vector2.zero;
        yield return new WaitForSeconds(dodgeDuration);
        isDodging = false;
        rbody.gravityScale = gravity;
        rbody.velocity = originalVelocity;
        yield return new WaitForSeconds(dodgeCooldown);
        canDodge = true;
    }

    IEnumerator Invulnerability()
    {
        vulnerable = false;
        // Wait 1.2 seconds before being vulnerable to taking damage again
        yield return new WaitForSeconds(invulnerabilityTime);
        Debug.Log("Warning: player is vulnerable");
        vulnerable = true;
        hurt = false;
    }
}
