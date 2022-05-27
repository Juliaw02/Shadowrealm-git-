using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeScript : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Click to attack
        if (Input.GetMouseButtonDown(0) && Input.GetAxis("Vertical") == 0)
        {
            anim.SetTrigger("Attacking(trig)");
        }

        // Up-swipe
        if (Input.GetMouseButtonDown(0) && Input.GetAxis("Vertical") > 0)
        {
            anim.SetTrigger("UpAttacking");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(1);
        }
    }
}
