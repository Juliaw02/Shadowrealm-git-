using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevenantBehavior : MonoBehaviour
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Melee")
        {
            currentRevHealth--;
            Debug.Log("Revenant health = " + currentRevHealth);
        }
    }
}
