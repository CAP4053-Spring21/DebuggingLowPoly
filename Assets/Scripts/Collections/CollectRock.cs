using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectRock : MonoBehaviour
{
    public AudioSource collectSound;
    public Unit unit;
    public GameObject prompt;
    public GameObject rockMessage;
    public GameObject Welcome;
    public GameObject fullHealth;
    public GameObject collectFire;
    public GameObject collectPoison;
    public Collectibles collectibles;

    public Collections collections;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Rock");
        collectSound.Play();
        Destroy(gameObject);
        
        collections.incrementRockCount();

        if (!collectibles.seenRockMessages)
        {
            collectibles.seenRockMessages = true;
            prompt.SetActive(true);
            rockMessage.SetActive(true);
            fullHealth.SetActive(false);
            collectFire.SetActive(false);
            collectPoison.SetActive(false);
            Welcome.SetActive(false);
        }

    }
}
