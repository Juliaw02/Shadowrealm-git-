using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBehavior : Enemy
{
    private int demonHealth = 2;
    private int currentDemonHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentDemonHealth = demonHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // If hit enough times, die
        if (currentDemonHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override void TakeDamage(int damage)
    {
        currentDemonHealth -= damage;
        //Debug.Log("Demon health = " + currentDemonHealth);
    }
}
