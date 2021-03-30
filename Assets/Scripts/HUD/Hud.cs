using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject minimap;
    public Slider slider;
    public bool ispaused;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

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
