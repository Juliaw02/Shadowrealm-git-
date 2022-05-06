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
    public float playerSpeed = 8f;
    public float jumpPower = 15f;
    public float jumpSpeed = 20f;
    public float playerScaleX = 1f;
    public float playerScaleY = 1f;

    private float horizontalInput;

    // Double jumps
    private int extraJumps = 1;
    private int jumpCounter = 0;

    // Coyote time (if you walk off a cliff you can still jump after)
    public float coyoteTimer = .25f;
    private float coyoteCounter;

    // Wall jumping
    public float wallJumpX = 200f;
    public float wallJumpY = 100f;
    //private float wallJumpCooldown;

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

        // Flip player in the direction they're moving in
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(-playerScaleX, playerScaleY, 1);
        } 
        else if (horizontalInput < -0.0f)
        {
            transform.localScale = Vector3.one;
        }

        // Setting animator parameters
        anim.SetBool("PlayerRun", horizontalInput != 0);
        anim.SetBool("Grounded", isGrounded());


        // Jumping
        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }

        // Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && rbody.velocity.y > 0)
        {
            rbody.velocity = new Vector2(rbody.velocity.x, rbody.velocity.y / 2);
        }

        if (onWall())
        {
            rbody.gravityScale = 0;
            rbody.velocity = Vector2.zero;
        }
        else
        {
            rbody.gravityScale = 2.5f;
            rbody.velocity = new Vector2(horizontalInput * jumpSpeed, rbody.velocity.y);
            
            // Reset counters when grounded
            if (isGrounded())
            {
                coyoteCounter = coyoteTimer;
                //jumpCounter = extraJumps;
            }
            else
            {
                coyoteCounter -= Time.deltaTime;
            }
        }
    }

    // Jump mechanics
    private void Jump()
    {
        // If you aren't on a wall or doing anything crazy, don't do anything
        if (coyoteCounter <= 0 && !onWall() && jumpCounter <= 0) return;

        // Add sound for jump?

        if (onWall())
        {
            WallJump();
        }
        else
        {
            // Normal jump
            if (isGrounded())
            {
                rbody.velocity = new Vector2(rbody.velocity.x, jumpPower);
            }
            // Coyote jump
            else if (coyoteCounter > coyoteTimer)
            {
                    rbody.velocity = new Vector2(rbody.velocity.x, jumpPower);
            }
            // Double jump
            else if (jumpCounter < extraJumps)
            {
                rbody.velocity = new Vector2(rbody.velocity.x, jumpPower);
                Debug.Log("jumpCounter++");
                jumpCounter++;
            }
            
            // reset the counter to avoid double jump
            coyoteCounter = 0;
        }
    }

    private void WallJump()
    {
        rbody.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
       // wallJumpCooldown = 0;
    }

    // Check if player is on the ground
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    // Check if player is on a wall
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}
