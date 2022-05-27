using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoltergeistBehavior : Enemy
{
    private int poltHealth = 2;
    public int currentPoltHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentPoltHealth = poltHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // If hit enough times, die
        if (currentPoltHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override void TakeDamage(int damage)
    {
        currentPoltHealth -= damage;
        Debug.Log("Poltergeist health = " + currentPoltHealth);
    }
}
