using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectRock : MonoBehaviour
{
    public AudioSource collectSound;
    public Unit unit;
    public GameObject prompt;
    public GameObject rockMessage;
    public Rocks rocks;

    void OnTriggerEnter(Collider other)
    {
        collectSound.Play();
        Destroy(gameObject);
        
        if (!rocks.seenMessages)
        {
            rocks.seenMessages = true;
            prompt.SetActive(true);
            rockMessage.SetActive(true);
        }

    }
}
