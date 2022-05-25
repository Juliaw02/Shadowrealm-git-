using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HauntBehavior : MonoBehaviour
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Melee")
        {
            currentHauntHealth--;
            Debug.Log("Haunt health = " + currentHauntHealth);
        }
    }
}
