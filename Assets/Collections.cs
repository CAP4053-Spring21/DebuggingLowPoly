using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collections : MonoBehaviour
{
    public Text rockCountText;
    public Text fireCountText;
    public Text poisonCountText;
    public int numRocks = 0;
    public int numFire = 0;
    public int numPoison = 0;

    public void incrementRockCount()
    {
        numRocks++;
        string newcount = "x" + numRocks.ToString();
        rockCountText.text = newcount; 
    }

    public void incrementFireCount()
    {
        numFire++;
        string newcount = "x" + numFire.ToString();
        fireCountText.text = newcount; 
    }

    public void incrementPoisonCount()
    {
        numPoison++;
        string newcount = "x" + numPoison.ToString();
        poisonCountText.text = newcount; 
    }

    void Start()
    {
        rockCountText.text += numRocks.ToString();
        fireCountText.text += numFire.ToString();
        poisonCountText.text += numPoison.ToString();
    }
}
