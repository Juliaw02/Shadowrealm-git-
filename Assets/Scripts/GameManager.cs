using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public GameObject player;
    //public Transform gravestone;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // SCENE CHANGES
    private void OnTriggerEnter2D(Collider2D other)
    {
        // From Hall 1 to Crypt 1
        if (this.gameObject.tag == "Crypt1_1" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Crypt 1");
        }

        // From Crypt 1 to Hall 2 (bottom left)
        if (this.gameObject.tag == "Hall2_bL" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Hall 2");
        }
        // From Hall 2 back to Crypt 1
        if (this.gameObject.tag == "Crypt1_2" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Crypt 1");
        }

        // From Hall 2 to Crypt 2
        if (this.gameObject.tag == "Crypt2_1" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Crypt 2");
        }
        // From Crypt 2 back to Hall 2 (top left)
        if (this.gameObject.tag == "Hall2_tL" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Hall 2");
        }
        
        // From Crypt 2 to Hall 3
        if (this.gameObject.tag == "Hall3_1" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Hall 3");
        }
        // From Hall 3 back to Crypt 2
        if (this.gameObject.tag == "Crypt2_2" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Crypt 2");
        }

        // From Hall 3 to Crypt 3
        if (this.gameObject.tag == "Crypt3_1" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Crypt_3");
        }
        // From Crypt 3 back to Hall 3
        if (this.gameObject.tag == "Hall3_2" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Hall 3");
        }

        // From Crypt 3 to Hall 2 (bottom right)
        if (this.gameObject.tag == "Hall2_bR" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Crypt 3");
        }
        // From Hall 2 back to Crypt 3
        if (this.gameObject.tag == "Crypt3_2" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Hall 3");
        }

        // From Hall 2 to Hall 4 
        if (this.gameObject.tag == "Hall4_1" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Crypt 3");
        }
        // From Hall 4 back to Hall 2 (top right)
        if (this.gameObject.tag == "Hall2_tR" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Hall 3");
        }

        // From Hall 4 to Crypt 4 
        if (this.gameObject.tag == "Crypt4_1" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Crypt 3");
        }
        // From Crypt 4 back to Hall 4
        if (this.gameObject.tag == "Hall4_2" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Hall 3");
        }

        // From Crypt 4 to Hall 5 (bottom right)
        if (this.gameObject.tag == "Hall5_bR" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Crypt 3");
        }
        // From Hall 5 back to Crypt 4
        if (this.gameObject.tag == "Crypt4_2" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Hall 3");
        }

        // From Hall 5 to Crypt 5
        if (this.gameObject.tag == "Crypt5_1" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Crypt 3");
        }
        // From Crypt 5 back to Hall 5 (top right)
        if (this.gameObject.tag == "Hall5_tR" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Hall 3");
        }

        // From Crypt 5 to Hall 6 (bottom left)
        if (this.gameObject.tag == "Hall6_bL" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Crypt 3");
        }
        // From Hall 6 back to Crypt 5
        if (this.gameObject.tag == "Crypt5_2" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Hall 3");
        }

        // From Hall 6 to Hall 7
        if (this.gameObject.tag == "Hall7" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Crypt 3");
        }
        // From Hall 7 back to Hall 6 (top left)
        if (this.gameObject.tag == "Hall6_tL" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Hall 3");
        }

        // From Hall 6 to Sister Boss Room (bottom entrance)
        if (this.gameObject.tag == "sBoss_bot" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Crypt 3");
        }
        // From Sister Boss Room back to Hall 6 from bottom entrance (bottom right)
        if (this.gameObject.tag == "Hall6_bR" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Hall 3");
        }

        // From Hall 6 to Sister Boss Room (top entrance)
        if (this.gameObject.tag == "sBoss_top" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Crypt 3");
        }
        // There is no way to exit back to Hall 6 from the top entrance--you're screwed

        // From Hall 6 to Boss Room
        if (this.gameObject.tag == "bossRoom" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Crypt 3");
        }
        // From Boss Room back to Hall 6 (top right)
        if (this.gameObject.tag == "Hall6_tR" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Hall 3");
        }
        // From Boss Room to Eternal Peace
        if (this.gameObject.tag == "eternalPeace" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Hall 3");
        }

    }

    // Buttons

    public void Play()
    {
        SceneManager.LoadScene("Hall_1");
    }

    public void Controls()
    {
        SceneManager.LoadScene("Controls");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
