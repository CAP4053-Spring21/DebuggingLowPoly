using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Welcome : MonoBehaviour
{
    public List<GameObject> messages = new List<GameObject>();
    public GameObject welcome;
    public GameObject nextButton;
    public GameObject doneButton;
    public int current = 0;
    public int size = 4;
    public bool newGame = true;

    public void next()
    {
        GameObject message = messages[current];
        message.SetActive(false);

        current++;

        message = messages[current];
        message.SetActive(true);
    }

    

    void Update()
    {
        if (current == size - 1)
        {
            nextButton.SetActive(false);
            doneButton.SetActive(true);
        }
    }

}
