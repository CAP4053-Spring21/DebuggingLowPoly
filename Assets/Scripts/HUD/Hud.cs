using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Hud : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject minimap;
    public bool ispaused;

    public void togglePause()
    {
        minimap.SetActive(ispaused);

        // Toggles the pause menu
        pauseMenu.SetActive(!ispaused);

        // Freezes the game.
        Time.timeScale = (ispaused) ? 1 : 0;
        ispaused = !ispaused;
    }

    public void saveQuit()
    {
        int scene_index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene_index - 1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2) || Input.GetKeyDown(KeyCode.F3)) 
        {
            togglePause();
        }
    }
}
