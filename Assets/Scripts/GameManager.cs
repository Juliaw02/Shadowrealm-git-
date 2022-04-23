using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /* if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene("Crypt 1");
        } */
    }

    // Scene changes
    private void OnTriggerEnter2D(Collider2D other)
    {
        // from Hall 1 to Crypt 1
        if (this.gameObject.tag == "Crypt1_1" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Crypt 1");
        // from Crypt 1 back to Hall 1
        } else if (this.gameObject.tag == "Hall1" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Hall 1");
            //other.transform.position = new Vector2(200, -2.632f);


        // from Crypt 1 to Hall 2 (bottom left)
        } else if (this.gameObject.tag == "Hall2_bL" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Hall 2");
        // from Hall 2 back to Crypt 1
        } else if (this.gameObject.tag == "Crypt1_2" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Crypt 1");


        // from Hall 2 to Crypt 2
        } else if (this.gameObject.tag == "Crypt2_1" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Crypt 2");
        // from Crypt 2 back to Hall 2 (top left)
        } else if (this.gameObject.tag == "Hall2_tL" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Hall 2");


        // from Crypt 2 to Hall 3
        } else if (this.gameObject.tag == "Hall3_1" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Hall 3");
        // from Hall 3 back to Crypt 2
        } else if (this.gameObject.tag == "Crypt2_2" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Crypt 2");


        // from Hall 3 to Crypt 3
        } else if (this.gameObject.tag == "Crypt3_1" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Crypt 3");
        // from Crypt 3 back to Hall 3
        } else if (this.gameObject.tag == "Hall3_2" && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Hall 3");
        }
    }
}
