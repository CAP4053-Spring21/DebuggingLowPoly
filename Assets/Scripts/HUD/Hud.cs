using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hud : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject minimap;
    public GameObject healthBar;
    public GameObject welcome;
    public bool gamePaused;

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
        minimap.SetActive(!gamePaused);
        healthBar.SetActive(!gamePaused);
        welcome.SetActive(!gamePaused);
        pauseMenu.SetActive(gamePaused);
        Time.timeScale = (gamePaused) ? 0 : 1;
    }

    // Update is called once per frame
    void Update()
    {
        int scene_index = SceneManager.GetActiveScene().buildIndex;
        if (scene_index == 1)
        {
            if (Input.GetKeyDown(KeyCode.F2) || Input.GetKeyDown(KeyCode.F3))
            {
                gamePaused = !gamePaused;

                togglePause(gamePaused);
            }
        }
    }
}
