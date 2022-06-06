using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevenantBehavior : Enemy
{
    private int revHealth = 4;
    private int currentRevHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentRevHealth = revHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // If hit enough times, die
        if (currentRevHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override void TakeDamage(int damage)
    {
        currentRevHealth -= damage;
        //Debug.Log("Revenant health = " + currentRevHealth);
    }
}
