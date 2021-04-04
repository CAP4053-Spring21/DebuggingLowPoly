using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        int scene_index = SceneManager.GetActiveScene().buildIndex;

        Time.timeScale = 1;
        SceneManager.LoadScene(scene_index + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT GAME");
        Application.Quit();
    }
}

