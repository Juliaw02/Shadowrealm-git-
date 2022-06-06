using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WraithBehavior : Enemy
{
    private int wraithHealth = 4;
    private int currentWraithHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentWraithHealth = wraithHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // If hit enough times, die
        if (currentWraithHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override void TakeDamage(int damage)
    {
        currentWraithHealth -= damage;
        //Debug.Log("Wraith health = " + currentWraithHealth);
    }
}
