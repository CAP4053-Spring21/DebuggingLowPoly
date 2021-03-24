using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hud : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject minimap;
    public bool ispaused;

    public void toggle()
    {
        minimap.SetActive(ispaused);

        ispaused = !ispaused;

        // Toggles the pause menu
        pauseMenu.SetActive(ispaused);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2) || Input.GetKeyDown(KeyCode.F3)) 
        {
            // Freezes the game.
            Time.timeScale = (ispaused) ? 1 : 0;
            
            toggle();
        }
    }
}
