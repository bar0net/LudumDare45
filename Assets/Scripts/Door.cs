using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string nextScene = "ScenesMenu";

    int contact = 0;
    public bool active = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Permanent p = FindObjectOfType<Permanent>();
            if (p) p.deathPositions.Clear();
            SceneManager.LoadScene(0);
        }


        if ((active && contact > 0 && Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") > 0) || 
            Input.GetKeyDown(KeyCode.N))
        {
            Permanent p = FindObjectOfType<Permanent>();
            if (p) p.deathPositions.Clear();
            SceneManager.LoadScene(nextScene);
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") contact++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") contact--;
    }
}
