using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    public Slider slider;
    public Unit unit;

    public void SetMaxXP(int xp)
    {
        slider.maxValue = xp;
    }

    public void SetXP(int xp)
    {
        slider.value = xp;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetMaxXP(100);
    }

    void Update()
    {
        SetXP(unit.currentXP);
    }
}
