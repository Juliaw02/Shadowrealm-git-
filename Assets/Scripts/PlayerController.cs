using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rbody;
    public float playerSpeed;
    public float jumpSpeed;

    private bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // A + D or left and right arrow keys to move left and right
        float horizontalInput = Input.GetAxis("Horizontal");
        rbody.velocity = new Vector2(horizontalInput * playerSpeed, rbody.velocity.y);

        // Flip player in the direction they're moving in
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(0.5f, 1, 1);
        } else if (horizontalInput < -0.0f)
        {
            transform.localScale = new Vector3(-0.5f, 1, 1);
        }

        // Space to jump
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rbody.velocity = new Vector2(rbody.velocity.x, jumpSpeed);
        grounded = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // on ground (if not jumping)
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }
}
