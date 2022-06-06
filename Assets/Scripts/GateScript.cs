using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateScript : MonoBehaviour
{
    private Animator anim;

    // Crypt Enemies
    public GameObject[] crypt1Enemies;
    public GameObject[] crypt2Enemies;
    public GameObject[] crypt3Enemies;
    public GameObject[] crypt4Enemies;
    public GameObject[] crypt5Enemies;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the crypt enemies are killed
        if (gameObject.tag == "GateUp")
        {
            if (SceneManager.GetActiveScene().name == "Crypt 1")
            {
                if (!crypt1Enemies[0] && !crypt1Enemies[1] && !crypt1Enemies[2] && !crypt1Enemies[3] && !crypt1Enemies[4] && !crypt1Enemies[5] && !crypt1Enemies[6])
                {
                    anim.SetTrigger("CryptDone");
                }
            }
            if (SceneManager.GetActiveScene().name == "Crypt 2")
            {
                if (!crypt2Enemies[0] && !crypt2Enemies[1] && !crypt2Enemies[2] && !crypt2Enemies[3] && !crypt2Enemies[4] && !crypt2Enemies[5] && !crypt2Enemies[6] && !crypt2Enemies[7] && !crypt2Enemies[8])
                {
                    anim.SetTrigger("CryptDone");
                }
            }
            if (SceneManager.GetActiveScene().name == "Crypt_3")
            {
                if (!crypt3Enemies[0] && !crypt3Enemies[1] && !crypt3Enemies[2] && !crypt3Enemies[3] && !crypt3Enemies[4] && !crypt3Enemies[5])
                {
                    anim.SetTrigger("CryptDone");
                }
            }
            if (SceneManager.GetActiveScene().name == "Crypt 4")
            {
                if (!crypt4Enemies[0] && !crypt4Enemies[1] && !crypt4Enemies[2] && !crypt4Enemies[3] && !crypt4Enemies[4] && !crypt4Enemies[5] && !crypt4Enemies[6] && !crypt4Enemies[7])
                {
                    anim.SetTrigger("CryptDone");
                }
            }
            if (SceneManager.GetActiveScene().name == "Crypt 5")
            {
                if (!crypt5Enemies[0] && !crypt5Enemies[1] && !crypt5Enemies[2] && !crypt5Enemies[3] && !crypt5Enemies[4] && !crypt5Enemies[5] && !crypt5Enemies[6] && !crypt5Enemies[7] && !crypt5Enemies[8] && !crypt5Enemies[9] && !crypt5Enemies[10] && !crypt5Enemies[11] && !crypt5Enemies[12] && !crypt5Enemies[13] && !crypt5Enemies[14])
                {
                    anim.SetTrigger("CryptDone");
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            anim.SetTrigger("InCrypt");
        }
    }
}
