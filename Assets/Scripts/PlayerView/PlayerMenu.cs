using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenu : MonoBehaviour
{
    public GameObject playerMenu;
    public GameObject player;
    public GameObject minimap;
    public bool ispaused;

    public void DeactivateMenu()
    {
        ispaused = !ispaused;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2) || Input.GetKeyDown(KeyCode.F3))
        {
            if (ispaused)
            {
                playerMenu.SetActive(false);
                player.SetActive(true);
                minimap.SetActive(true);
                ispaused = !ispaused;
            }
            else
            {
                player.SetActive(false);
                minimap.SetActive(false);
                playerMenu.SetActive(true);
                ispaused = !ispaused;
            }
        }
    }
}
