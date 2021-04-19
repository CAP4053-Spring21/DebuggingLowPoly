using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{

    public Text nameText;
    public Text levelText;
    public Slider hpSlider;
    public Slider xpSlider;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
        if (xpSlider != null)
        {
            xpSlider.maxValue = unit.maxXP;
            xpSlider.value = unit.currentXP;
        }
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

    public void SetXP(int xp)
    {
        xpSlider.value = xp;
    }

}
