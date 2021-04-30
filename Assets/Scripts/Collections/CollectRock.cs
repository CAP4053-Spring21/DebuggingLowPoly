using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectRock : MonoBehaviour
{
    public AudioSource collectSound;
    public Unit unit;
    public GameObject prompt;
    public GameObject rockMessage;
    public Collectables collectables;

    public Collections collections;

    void OnTriggerEnter(Collider other)
    {
        collectSound.Play();
        Destroy(gameObject);
        
        collections.incrementRockCount();

        if (!collectables.seenRockMessages)
        {
            collectables.seenRockMessages = true;
            prompt.SetActive(true);
            rockMessage.SetActive(true);
        }

    }
}
