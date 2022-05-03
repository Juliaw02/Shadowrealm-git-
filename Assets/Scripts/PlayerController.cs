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
    public float playerSpeed;
    public float jumpPower;
    public float jumpSpeed;
    public float playerScaleX;
    public float playerScaleY;

    private float wallJumpCooldown;
    private float horizontalInput;

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

        // Space to jump
        if (Input.GetKey(KeyCode.Space) && isGrounded())
        {
            Jump();
        }

        // Wall jumping
        if (wallJumpCooldown > 0.2f)
        {
            rbody.velocity = new Vector2(horizontalInput * playerSpeed, rbody.velocity.y);

            if (onWall() && !isGrounded())
            {
                rbody.gravityScale = 0;
                rbody.velocity = Vector2.zero;
            }
            else
            {
                rbody.gravityScale = 2;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
        } 
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }

        // Setting animator parameters
        anim.SetBool("PlayerRun", horizontalInput != 0);
    }

    // Jump mechanics
    private void Jump()
    {
        if (isGrounded())
        {
            rbody.velocity = new Vector2(rbody.velocity.x, jumpPower);
        } 
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                rbody.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            } 
            else
            {
                rbody.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            }
            wallJumpCooldown = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
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
