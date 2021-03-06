using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MainPausedMenu : MonoBehaviour
{
    public void SaveAndQuit()
    {
        int scene_index = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(scene_index - 1);
    }
}
