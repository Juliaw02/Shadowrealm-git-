using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBehavior : MonoBehaviour
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Melee")
        {
            currentDemonHealth--;
            Debug.Log("Demon health = " + currentDemonHealth);
        }
    }
}
