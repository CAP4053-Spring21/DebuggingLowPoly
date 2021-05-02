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
    public GameObject healthMessage;
    public GameObject rockMessage;
    public GameObject Welcome;
    public GameObject collectPoison;
    public GameObject fireMessage;

    void OnTriggerEnter(Collider other)
    {
        collectSound.Play();


        if (unit.currentHP <  unit.maxHP)
        {
            int hpDiff = unit.maxHP - unit.currentHP;
            unit.currentHP += (hpDiff >= 15) ? 15 : hpDiff;
            Destroy(gameObject);
        }
        else
        {
            prompt.SetActive(true);
            healthMessage.SetActive(true);

            rockMessage.SetActive(false);
            Welcome.SetActive(false);
            collectPoison.SetActive(false);
            fireMessage.SetActive(false);

        }
        
    }

}
