using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Unit unit;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetMaxHealth(100);
    }

    void Update()
    {
        SetHealth(unit.currentHP);
    }
}
