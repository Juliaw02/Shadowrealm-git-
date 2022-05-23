using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomBehavior : MonoBehaviour
{
    public float phantSpeed;
    public float range;
    public float maxDistance;
    private Vector2 wayPoint;

    private int phantHealth = 1;
    private int currentPhantHealth;

    // Start is called before the first frame update
    void Start()
    {
        SetNewDestination();
        currentPhantHealth = phantHealth;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, wayPoint, phantSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, wayPoint) < range)
        {
            SetNewDestination();
        }
    }

    private void SetNewDestination()
    {
        wayPoint = new Vector2(transform.position.x + Random.Range(-maxDistance, maxDistance), transform.position.y + Random.Range(-maxDistance, maxDistance));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("ladies and gents the phantom hit something");
    }
}
