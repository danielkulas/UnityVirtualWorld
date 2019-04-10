/*
 * Daniel Kulas, 2018
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public GameObject UIPauseMenu;
    public Transform Water;
    public Transform Player;
    private bool paused = false;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused == false)
                pause();
            else
                resume();
        }
    }

    public void pause()
    {
        GetComponent<Shop>().exitShop();
        UIPauseMenu.SetActive(true);
        Player.GetComponent<InsertionObjectOnClick>().enabled = false;
        Water.GetComponent<WaterGenerator>().enabled = false;
        Time.timeScale = 0;
        paused = true;
    }

    public void resume()
    {
        Time.timeScale = 1;
        Water.GetComponent<WaterGenerator>().enabled = true;
        Player.GetComponent<InsertionObjectOnClick>().enabled = true;
        UIPauseMenu.SetActive(false);
        paused = false;
    }


    //Buttons
    public void quit()
    {
        Application.Quit();
    }

    public void toMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }
}