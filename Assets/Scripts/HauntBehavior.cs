using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HauntBehavior : Enemy
{
    private int hauntHealth = 3;
    private int currentHauntHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHauntHealth = hauntHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // If hit enough times, die
        if (currentHauntHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override void TakeDamage(int damage)
    {
        currentHauntHealth -= damage;
        //Debug.Log("Haunt health = " + currentHauntHealth);
    }
}
