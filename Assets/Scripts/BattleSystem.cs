using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    #region Singleton

    public static BattleSystem instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    //public GameObject playerPrefab;
    GameObject enemyGO;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public Camera mainCam;
    public Camera battleCam;

    public Canvas mainCanvas;
    public Canvas battleCanvas;

    Unit playerUnit;
    Unit enemyUnit;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    // Start is called before the first frame update
    public void StartBattle(GameObject enemy)
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle(enemy));
    }

    IEnumerator SetupBattle(GameObject enemy)
    {
        mainCam.enabled = false;
        battleCam.enabled = true;

        mainCanvas.enabled = false;
        battleCanvas.enabled = true;

        PlayerManager.instance.player.GetComponent<NavMeshAgent>().Warp(playerBattleStation.position);
        PlayerManager.instance.player.transform.rotation = playerBattleStation.rotation;
        playerUnit = PlayerManager.instance.player.GetComponent<Unit>();
        PlayerManager.instance.player.GetComponent<PlayerController>().enabled = false;

        enemyGO = enemy;
        enemy.GetComponent<NavMeshAgent>().Warp(enemyBattleStation.position);
        enemy.GetComponent<NavMeshAgent>().SetDestination(enemyBattleStation.position);
        enemy.transform.rotation = enemyBattleStation.rotation;
        enemyUnit = enemy.GetComponent<Unit>();

        dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "The attack is successful!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            Destroy(enemyGO);
            dialogueText.text = "You won the battle!";

        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(5);

        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "You feel renewed strength!";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal());
    }

}
