using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leader : MonoBehaviour
{
    public GameObject Button;   //a picture write R
    public GameObject talkUI;   //the talk Canva
    public GameObject Panel;    //panel on Canva

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Button.SetActive(true);//means to guide player to press the keyboard "R"

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //means good-bye
        Button.SetActive(false);
        talkUI.SetActive(false);
        Panel.SetActive(false);
    }

    private void Update()
    {
        //always check if the player is pressing the keyboard "R"
        if (Button.activeSelf && Input.GetKeyDown(KeyCode.R))
        {
            talkUI.SetActive(true);
            Panel.SetActive(true);
        }
    }
}
