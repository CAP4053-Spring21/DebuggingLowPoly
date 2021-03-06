using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hud : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject healthBar;
    public GameObject welcome;
    public GameObject front;
    public GameObject optionsPage;
    public GameObject Inventory;
    public GameObject Prompt;
    public Text levelText;
    public Unit unit;

    public bool gamePaused;
    public bool optionsOn;

    public void restartGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void resumeGame() 
    {
        gamePaused = !gamePaused;
        togglePause(gamePaused);
    }

    public void saveQuit()
    {
        int scene_index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene_index - 1);
    }

    public void togglePause(bool gamePaused)
    {
        healthBar.SetActive(!gamePaused);
        Inventory.SetActive(!gamePaused);
        
        pauseMenu.SetActive(gamePaused);
        front.SetActive(gamePaused);

        if (optionsOn)
        {
            optionsPage.SetActive(gamePaused);
            optionsOn = !optionsOn;
        }

        Time.timeScale = (gamePaused) ? 0 : 1;
    }

    public void inOptions() 
    {
        optionsOn = !optionsOn;
    }

    // Update is called once per frame
    void Update()
    {
        levelText.text = "Lvl " + unit.unitLevel;

        int scene_index = SceneManager.GetActiveScene().buildIndex;
        if (scene_index == 1)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                    gamePaused = !gamePaused;
                    togglePause(gamePaused);
            }
        }
    }
}
