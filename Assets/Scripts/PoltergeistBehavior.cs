using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoltergeistBehavior : MonoBehaviour
{
    private int poltHealth = 2;
    private int currentPoltHealth;

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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Melee")
        {
            currentPoltHealth--;
            Debug.Log("Poltergeist health = " + currentPoltHealth);
        }
    }
}
