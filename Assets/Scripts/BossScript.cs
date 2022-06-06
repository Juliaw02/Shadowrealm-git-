using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossScript : Enemy
{
    public float XottixSpeed = 2;
    public float range = 2;
    public float maxDistance = 5;
    private Vector2 wayPoint;

    private int xottixHealth = 22;
    private int currentXottixHealth;

    // Start is called before the first frame update
    void Start()
    {
        SetNewDestination();
        currentXottixHealth = xottixHealth;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, wayPoint, XottixSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, wayPoint) < range)
        {
            SetNewDestination();
        }

        // If hit enough times, die
        if (currentXottixHealth <= 0)
        {
            Destroy(gameObject);
            if (SceneManager.GetActiveScene().name == "Sister Boss")
            {
                SceneManager.LoadScene("Bad Ending");
            }
        }
    }

    private void SetNewDestination()
    {
        wayPoint = new Vector2(transform.position.x + Random.Range(-maxDistance, maxDistance), transform.position.y + Random.Range(-maxDistance, maxDistance));
    }

    public override void TakeDamage(int damage)
    {
        currentXottixHealth -= damage;
        //Debug.Log("Xottix health = " + currentXottixHealth);
    }
}
