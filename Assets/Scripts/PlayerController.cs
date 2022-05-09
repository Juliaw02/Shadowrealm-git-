using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rbody;
    private BoxCollider2D boxCol;
    private Animator anim;

    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public Transform RightBoob;
    public Transform Hall1Spawn;
    public float playerSpeed;
    public float jumpPower;
    public float jumpSpeed;
    public float playerScaleX;
    public float playerScaleY;

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

    // Attacking
    //private bool attacking = false;
    //private float attackTimer = 0;
    //private float attackCool = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // A + D or left and right arrow keys to move left and right
        horizontalInput = Input.GetAxis("Horizontal");
        rbody.velocity = new Vector2(horizontalInput * playerSpeed, rbody.velocity.y);
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
        } 
        else if (horizontalInput < -0.0f)
        {
            transform.localScale = new Vector3(-playerScaleX, playerScaleY, 1);
        }

        // Setting animator parameters
        anim.SetBool("PlayerRun", horizontalInput != 0);
        anim.SetBool("Grounded", isGrounded());
        anim.SetBool("WallJumping", onWall());

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
            rbody.velocity = new Vector2(horizontalInput * playerSpeed, rbody.velocity.y);
            
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

        // Click to attack
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attacking(trig)");
        }
    }

    // Jump mechanics
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
        /**if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(-playerScaleX, playerScaleY, 1);
        }  */               
        rbody.velocity = new Vector2(0, 0);
        rbody.velocity = new Vector2(-horizontalInput * 100, jumpPower);

        //rbody.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) + wallJumpX, jumpPower);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Spawn point for getting back to Hall 1
        if (collision.tag == "Hall1")
        {
            transform.position = Hall1Spawn.position;
        }
    }

    // Check if player is on the ground
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    // Check if player is on a wall.'
    private bool onWall()
    {
        return Physics2D.OverlapCircle(RightBoob.position, RightBoob.GetComponent<CircleCollider2D>().radius, wallLayer);

    
        //RaycastHit2D raycastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        //RaycastHit2D raycastHit2 = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0, new Vector2(-transform.localScale.x, 0), 0.1f, wallLayer);
        //return (raycastHit.collider != null && raycastHit2.collider != null);
    }
}
