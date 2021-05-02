using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPoison : MonoBehaviour
{
    public AudioSource collectSound;
    public Unit unit;
    public GameObject prompt;
    public GameObject poisonMessage;
    public Collectibles collectibles;
    public GameObject rockMessage;
    public GameObject Welcome;
    public GameObject fullHealth;
    public GameObject collectFire;

    public Collections collections;

    void OnTriggerEnter(Collider other)
    {
        collectSound.Play();
        Destroy(gameObject);
        
        collections.incrementPoisonCount();

        if (!collectibles.seenPoisonMessages)
        {
            collectibles.seenPoisonMessages = true;
            prompt.SetActive(true);
            poisonMessage.SetActive(true);

            rockMessage.SetActive(false);
            fullHealth.SetActive(false);
            collectFire.SetActive(false);
            Welcome.SetActive(false);
        }

    }
}
