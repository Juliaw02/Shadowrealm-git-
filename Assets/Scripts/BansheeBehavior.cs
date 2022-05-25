using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BansheeBehavior : MonoBehaviour
{
    private int banHealth = 3;
    private int currentBanHealth;

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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Melee")
        {
            currentBanHealth--;
            Debug.Log("Banshee health = " + currentBanHealth);
        }
    }
}
