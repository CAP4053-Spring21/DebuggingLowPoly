using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CollectableScript : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource collectSound;
    public Unit unit;
    public GameObject prompt;

    void OnTriggerEnter(Collider other)
    {
        collectSound.Play();

        Destroy(gameObject); 
    }

}
