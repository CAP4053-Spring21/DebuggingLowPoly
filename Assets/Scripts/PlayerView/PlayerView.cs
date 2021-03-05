using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // F2 and F3 keys open pause menu
        if (Input.GetKeyUp(KeyCode.F2) || Input.GetKeyUp(KeyCode.F3))
        {
            Debug.Log("Paused");
        }
    }
}
