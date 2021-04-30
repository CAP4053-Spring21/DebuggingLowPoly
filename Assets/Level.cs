using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public Unit unit;
    public Text levelText;

    // Start is called before the first frame update
    void Start()
    {
        string temp = "Lvl " + unit.unitLevel.ToString();
        levelText.text = temp;
    }

    // Update is called once per frame
    void Update()
    {
        string temp = "Lvl " + unit.unitLevel.ToString();
        levelText.text = temp;
    }
}
