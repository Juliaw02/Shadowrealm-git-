using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadeBehavior : Enemy
{
    public Transform player;
    public float moveSpeed = .8f;
    private Rigidbody2D shadeRbody;
    private Vector2 shadeMovement;

    private int shadeHealth = 2;
    private int currentShadeHealth;

    // Start is called before the first frame update
    void Start()
    {
        shadeRbody = this.GetComponent<Rigidbody2D>();
        currentShadeHealth = shadeHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //shadeRbody.rotation = angle;

        direction.Normalize();
        shadeMovement = direction;

        // If hit enough times, die
        if (currentShadeHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        moveShade(shadeMovement);
    }

    private void moveShade(Vector2 direction)
    {
        shadeRbody.MovePosition((Vector2)transform.position + direction * moveSpeed * Time.deltaTime);
    }

    public override void TakeDamage(int damage)
    {
        currentShadeHealth -= damage;
        //Debug.Log("Shade health = " + currentShadeHealth);
    }
}
