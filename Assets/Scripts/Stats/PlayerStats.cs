using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Die()
    {
        base.Die();

        // Kill the player
        PlayerManager.instance.KillPlayer();
    }
}
