using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTransparency : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Color set to less transparent");
        Material mat = other.gameObject.GetComponent<Renderer>().material;
        Color oldColor = mat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0.3f);
        mat.SetColor("_Color", newColor);
    }

    void OnTriggerExit(Collider other)
    {
        Material mat = other.gameObject.GetComponent<Renderer>().material;
        Color oldColor = mat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 1f);
        mat.SetColor("_Color", newColor);
    }
}
