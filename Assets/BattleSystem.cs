using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}
public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject EnemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    //Unit playerUnit;
    //Unit enemyUnit;
    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        SetupBattle();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetupBattle()
    {
        GameObject playerGo = Instantiate(playerPrefab, playerBattleStation);
        //playerGo.GetComponent<Unit>();
        Instantiate(EnemyPrefab, enemyBattleStation);
    }
}
