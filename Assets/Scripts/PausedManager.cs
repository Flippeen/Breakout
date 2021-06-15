using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausedManager : MonoBehaviour
{
    [SerializeField] GameObject pausedCanvas;
    Gamemanager gamemanager;
    bool paused;
    private void Awake()
    {
        gamemanager = GetComponent<Gamemanager>();
        pausedCanvas.SetActive(false);
    }

    //Pauses the game and stops all movement.
    public void PauseGame()
    {
        paused = !paused;
        pausedCanvas.SetActive(paused);
        if (paused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
    //Makes the player go back to the main menu.
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MM");
        Time.timeScale = 1;
    }
    public void RestartGame()
    {
        PauseGame();
        gamemanager.ResetLevel();
    }
}
