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

    // Health
    private bool vulnerable = true;
    private int playerHealth = 5;
    private float invulnerabilityTime = 1.2f;
    bool hurt = false;
    private int currentPlayerHealth;
    private int maxPlayerHealth;
    public GameObject[] health;

    // Gravestones
    public Transform grave1;
    bool grave2Active = false;
    bool grave3Active = false;
    bool grave4Active = false;
    bool grave5Active = false;
    bool grave6Active = false;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        gravity = rbody.gravityScale;
        originalVelocity = rbody.velocity;

        currentPlayerHealth = playerHealth;
        maxPlayerHealth = 5;
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
            dashCoroutine = Dash(.18f, .5f);
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
        if (!Input.GetKeyDown(KeyCode.C))
        {
            isUpDashing = false;
            rbody.gravityScale = gravity;
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


        // HEALTH HUD
        if (currentPlayerHealth == 0)
        {
            health[0].SetActive(false);
            // Player respawns at last gravestone/spawn point
        } 
        else if (currentPlayerHealth == 1)
        {
            health[1].SetActive(false);
        } 
        else if (currentPlayerHealth == 2)
        {
            health[2].SetActive(false);
        }
        else if (currentPlayerHealth == 3)
        {
            health[3].SetActive(false);
        }
        else if (currentPlayerHealth == 4)
        {
            health[4].SetActive(false);
        }


        // GRAVESTONE SPAWN POINTS

        // If player dies, teleport to last used gravestone
        if (currentPlayerHealth <= 0)
        {
            if (SceneManager.GetActiveScene().name == "Hall_1")
            {
                gameObject.transform.position = grave1.transform.position;
            }
            // If in Crypt 1, change back to Hall 1 and teleport player to Gravestone1
            else if (SceneManager.GetActiveScene().name == "Crypt 1")
            {
                SceneManager.LoadScene("Hall_1");
                gameObject.transform.position = grave1.transform.position;
            }
            // If in Hall 2, teleport player to last used gravestone in this scene
            else if (SceneManager.GetActiveScene().name == "Hall 2")
            {
                if (grave2Active == true)
                {
                    gameObject.transform.position = new Vector2(334.2563f, 37.832f);
                }
                else if (grave3Active == true)
                {
                    gameObject.transform.position = new Vector2(189.98f, 55.193f);
                }
                // If both graves have not been activated in Hall 2, teleport player back to the beginning of Hall 2
                else if (grave2Active == false && grave3Active == false)
                {
                    gameObject.transform.position = new Vector2(290.85f, -1.606f);
                }
            }
            // If in Crypt 2, change back to Hall 2 and teleport player to last used gravestone
            else if (SceneManager.GetActiveScene().name == "Crypt 2")
            {
                SceneManager.LoadScene("Hall 2");
                if (grave2Active == true)
                {
                    gameObject.transform.position = new Vector2(334.2563f, 37.832f);
                }
                else if (grave3Active == true)
                {
                    gameObject.transform.position = new Vector2(189.98f, 55.193f);
                }
                // Teleport to beginning of Hall 2
                else if (grave2Active == false && grave3Active == false)
                {
                    gameObject.transform.position = new Vector2(290.85f, -1.606f);
                }
            }
            // If in Hall 3, teleport player to last used gravestone
            else if (SceneManager.GetActiveScene().name == "Hall 3")
            {
                if (grave4Active == true)
                {
                    gameObject.transform.position = new Vector2(-17.26f, 76.77f);
                }
                // If Gravestone4 has not been used/activated, teleport player to the beginning of Hall 3
                else if (grave4Active == false)
                {
                    gameObject.transform.position = new Vector2(56.97f, 80.97f);
                }
            }
            // If in Crypt 3, change back to Hall 3 and teleport player to last used gravestone
            else if (SceneManager.GetActiveScene().name == "Crypt_3")
            {
                SceneManager.LoadScene("Hall 3");
                if (grave4Active == true)
                {
                    gameObject.transform.position = new Vector2(-17.26f, 76.77f);
                }
                // Teleport to beginning of Hall 3
                else if (grave4Active == false)
                {
                    gameObject.transform.position = new Vector2(56.97f, 80.97f);
                }
            }
            // If in Hall 4, teleport player to last used gravestone
            else if (SceneManager.GetActiveScene().name == "Hall 3")
            {
                if (grave4Active == true)
                {
                    gameObject.transform.position = new Vector2(-17.26f, 76.77f);
                }
                else if (grave2Active == true)
                {
                    SceneManager.LoadScene("Hall 2");
                    gameObject.transform.position = new Vector2(334.2563f, 37.832f);
                }
                // If Gravestone4 has not been used/activated, teleport player to the beginning of Hall 2/outside of Crypt 3
                else if (grave4Active == false && grave2Active == false)
                {
                    SceneManager.LoadScene("Hall 2");
                    gameObject.transform.position = new Vector2(303.8f, 96.15f);
                }
            }
            // If in Crypt 4, change back to Hall 4 and teleport player to last used gravestone
            else if (SceneManager.GetActiveScene().name == "Crypt 4")
            {
                if (grave4Active == true)
                {
                    SceneManager.LoadScene("Hall 3");
                    gameObject.transform.position = new Vector2(-17.26f, 76.77f);
                }
                else if (grave2Active == true)
                {
                    SceneManager.LoadScene("Hall 2");
                    gameObject.transform.position = new Vector2(334.2563f, 37.832f);
                }
                // Teleport player to the beginning of Hall 2/outside of Crypt 3
                else if (grave4Active == false && grave2Active == false)
                {
                    SceneManager.LoadScene("Hall 2");
                    gameObject.transform.position = new Vector2(303.8f, 96.15f);
                }
            }
            // If in Hall 5, teleport player to last used gravestone
            else if (SceneManager.GetActiveScene().name == "Hall 5")
            {
                if (grave5Active == true)
                {
                    gameObject.transform.position = new Vector2(-57.91f, 279.33f);
                }
                // If Gravestone5 has not been used/activated, teleport player to the beginning of Hall 5
                else if (grave5Active == false)
                {
                    gameObject.transform.position = new Vector2(79.62f, 213.82f);
                }
            }
            // If in Crypt 5, change back to Hall 5, and teleport player to last used gravestone
            else if (SceneManager.GetActiveScene().name == "Crypt 5")
            {
                SceneManager.LoadScene("Hall 5");
                if (grave5Active == true)
                {
                    gameObject.transform.position = new Vector2(-57.91f, 279.33f);
                }
                // Teleport player to the beginning of Hall 5
                else if (grave5Active == false)
                {
                    gameObject.transform.position = new Vector2(79.62f, 213.82f);
                }
            }
            // If in Hall 6, teleport player to last used gravestone
            else if (SceneManager.GetActiveScene().name == "Hall 6")
            {
                if (grave6Active == true)
                {
                    gameObject.transform.position = new Vector2(-57.91f, 279.33f);
                }
                // If Gravestone6 has not been used/activated, teleport player to the beginning of Hall 6
                else if (grave6Active == false)
                {
                    gameObject.transform.position = new Vector2(154.13f, 298.87f);
                }
            }
            // If in Hall 7, teleport player to last used gravestone
            else if (SceneManager.GetActiveScene().name == "Hall 7")
            {
                SceneManager.LoadScene("Hall 6");
                if (grave6Active == true)
                {
                    gameObject.transform.position = new Vector2(-57.91f, 279.33f);
                }
                // Teleport player to the beginning of Hall 6
                else if (grave6Active == false)
                {
                    gameObject.transform.position = new Vector2(154.13f, 298.87f);
                }
            }
            // If the Sister Boss is activated, trigger the bad ending
            else if (SceneManager.GetActiveScene().name == "Sister Boss")
            {
                SceneManager.LoadScene("Hall 6");
                if (grave6Active == true)
                {
                    gameObject.transform.position = new Vector2(-57.91f, 279.33f);
                }
                // Teleport player to the beginning of Hall 6
                else if (grave6Active == false)
                {
                    gameObject.transform.position = new Vector2(154.13f, 298.87f);
                }
            }
            // If player enters the Boss Room, teleport player to last used gravestone
            else if (SceneManager.GetActiveScene().name == "Boss Room")
            {
                SceneManager.LoadScene("Hall 6");
                if (grave6Active == true)
                {
                    gameObject.transform.position = new Vector2(-57.91f, 279.33f);
                }
                // Teleport player to the beginning of Hall 6
                else if (grave6Active == false)
                {
                    gameObject.transform.position = new Vector2(154.13f, 298.87f);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rbody.AddForce(new Vector2(dashDirection * 14, 0), ForceMode2D.Impulse);
        }

        if (isDodging)
        {
            rbody.AddForce(new Vector2(dodgeDirection * 16, 0), ForceMode2D.Impulse);
        }
    }

    // HEALTH (Cont.)
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
        }
    }

    // Gravestones + health
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If near/on the gravestone, heal
        if (other.gameObject.tag == "Gravestone" && currentPlayerHealth < maxPlayerHealth)
        {
            currentPlayerHealth = maxPlayerHealth;
            health[0].SetActive(true);
            health[1].SetActive(true);
            health[2].SetActive(true);
            health[3].SetActive(true);
            health[4].SetActive(true);
        }

        // Activating/deactivating each grave after being triggered
        if (other.gameObject.name == "Gravestone2")
        {
            grave2Active = true;
            grave3Active = false;
            grave4Active = false;
            grave5Active = false;
            grave6Active = false;
        }
        else if (other.gameObject.name == "Gravestone3")
        {
            grave2Active = false;
            grave3Active = true;
            grave4Active = false;
            grave5Active = false;
            grave6Active = false;
        }
        else if (other.gameObject.name == "Gravestone4")
        {
            grave2Active = false;
            grave3Active = false;
            grave4Active = true;
            grave5Active = false;
            grave6Active = false;
        }
        else if (other.gameObject.name == "Gravestone5")
        {
            grave2Active = false;
            grave3Active = false;
            grave4Active = false;
            grave5Active = true;
            grave6Active = false;
        }
        else if (other.gameObject.name == "Gravestone6")
        {
            grave2Active = false;
            grave3Active = false;
            grave4Active = false;
            grave5Active = false;
            grave6Active = true;
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
