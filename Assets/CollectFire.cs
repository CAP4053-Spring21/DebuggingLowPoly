using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectFire : MonoBehaviour
{
    public AudioSource collectSound;
    public Unit unit;
    public GameObject prompt;
    public GameObject fireMessage;
    public Collectables collectables;

    public Collections collections;

    void OnTriggerEnter(Collider other)
    {
        collectSound.Play();
        Destroy(gameObject);
        
        collections.incrementFireCount();

        if (!collectables.seenFireMessages)
        {
            collectables.seenFireMessages = true;
            prompt.SetActive(true);
            fireMessage.SetActive(true);
        }
    }
}
