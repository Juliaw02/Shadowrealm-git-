using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BansheeBehavior : Enemy
{
    private int banHealth = 3;
    public int currentBanHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentBanHealth = banHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // If hit enough times, die
        if (currentBanHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override void TakeDamage(int damage)
    {
        currentBanHealth -= damage;
        Debug.Log("Banshee health = " + currentBanHealth);
    }
}
