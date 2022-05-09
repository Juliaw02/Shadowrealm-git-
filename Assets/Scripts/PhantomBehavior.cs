using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomBehavior : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = .8f;
    private Rigidbody2D phantRbody;
    private Vector2 phantMovement;


    // Start is called before the first frame update
    void Start()
    {
        phantRbody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //phantRbody.rotation = angle;

        direction.Normalize();
        phantMovement = direction;
    }

    private void FixedUpdate()
    {
        movePhantom(phantMovement);
    }

    private void movePhantom(Vector2 direction)
    {
        phantRbody.MovePosition((Vector2)transform.position + direction * moveSpeed * Time.deltaTime);
    }
}
