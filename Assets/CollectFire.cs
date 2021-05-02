using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectFire : MonoBehaviour
{
    public AudioSource collectSound;
    public Unit unit;
    public GameObject prompt;
    public GameObject fireMessage;
    public Collectibles collectibles;
    public GameObject rockMessage;
    public GameObject Welcome;
    public GameObject fullHealth;
    public GameObject collectPoison;

    public Collections collections;

    void OnTriggerEnter(Collider other)
    {
        collectSound.Play();
        Destroy(gameObject);
        
        collections.incrementFireCount();

        if (!collectibles.seenFireMessages)
        {
            collectibles.seenFireMessages = true;
            prompt.SetActive(true);
            fireMessage.SetActive(true);

            rockMessage.SetActive(false);
            fullHealth.SetActive(false);
            collectPoison.SetActive(false);
            Welcome.SetActive(false);
        }
    }
}
