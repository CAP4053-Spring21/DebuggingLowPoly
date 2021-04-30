using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkScript : MonoBehaviour
{
    // Start is called before the first frame update
    int counter;
    int x, y, z;
    void Start()
    {
         counter = 0;
        x = 0;
        y = 0;
        z = 1;
            
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(x, y, z);
        counter = counter + 1;
        if (counter == 200)
        {
            transform.Rotate(0, 180, 0);
            counter = 0;

        }



    }
}
