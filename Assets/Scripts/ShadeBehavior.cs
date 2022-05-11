using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadeBehavior : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = .8f;
    private Rigidbody2D shadeRbody;
    private Vector2 shadeMovement;


    // Start is called before the first frame update
    void Start()
    {
        shadeRbody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //shadeRbody.rotation = angle;

        direction.Normalize();
        shadeMovement = direction;
    }

    private void FixedUpdate()
    {
        moveShade(shadeMovement);
    }

    private void moveShade(Vector2 direction)
    {
        shadeRbody.MovePosition((Vector2)transform.position + direction * moveSpeed * Time.deltaTime);
    }
}
