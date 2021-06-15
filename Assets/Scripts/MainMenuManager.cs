using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public enum menuState { mainMenu, levelSelect, howToPlay};
    menuState currentState;
    [SerializeField] GameObject mainMenu, howToPlayMenu, levelSelectMenu;

    void Update()
    {
        //Manages which Menu UI to show and not to show in the main menu.
        switch (currentState)
        {
            case menuState.mainMenu:
                mainMenu.SetActive(true);
                levelSelectMenu.SetActive(false);
                howToPlayMenu.SetActive(false);
                break;
            case menuState.levelSelect:
                mainMenu.SetActive(false);
                levelSelectMenu.SetActive(true);
                howToPlayMenu.SetActive(false);
                break;
            case menuState.howToPlay:
                mainMenu.SetActive(false);
                levelSelectMenu.SetActive(false);
                howToPlayMenu.SetActive(true);
                break;
            default:
                break;
        }
    }
    //Used to switch state in the main menu.
    public void NewMenuState(string newState)
    {
        switch (newState)
        {
            case "MainMenu":
                currentState = menuState.mainMenu;
                break;
            case "LevelSelect":
                currentState = menuState.levelSelect;
                break;
            case "HowToPlay":
                currentState = menuState.howToPlay;
                break;
            default:
                break;
        }
    }
    //Used to select a level based on its number Levl_>levelnumber<
    public void SelectLevel(int levelNumber)
    {
        SceneManager.LoadScene($"Level_{levelNumber}");
    }
    //Quits the game.
    public void QuitGame()
    {
        Application.Quit();
    }
}
