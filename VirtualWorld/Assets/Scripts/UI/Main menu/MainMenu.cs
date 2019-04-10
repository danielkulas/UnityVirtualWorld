/*
 * Daniel Kulas, 2018
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject options;


    public void playGame()
    {
        PlayerPrefs.SetInt("GameToLoad", 0); //1 if user pressed Load Game, 0 if user choosed New Game
        SceneManager.LoadScene("GameScene");
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void loadGame()
    {
        PlayerPrefs.SetInt("GameToLoad", 1); //1 if user pressed Load Game, 0 if user choosed New Game
        SceneManager.LoadScene("GameScene");
    }

    public void toOptions()
    {
        options.SetActive(true);
        menu.SetActive(false);
    }

    public void backToMenu()
    {
        menu.SetActive(true);
        options.SetActive(false);
    }
}