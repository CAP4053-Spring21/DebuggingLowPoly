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
    GameObject playerGO;

    public Transform playerAttackSpot;
    public Transform enemeyAttackSpot;

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


    Vector3 playerPreBattlePosition;
    Quaternion playerPreBattleRotation;
    int enemyStunCount;
    int enemyPoisonCount;
    bool stunnedBefore;

    // Start is called before the first frame update
    public void StartBattle(GameObject enemy)
    {
        state = BattleState.START;
        playerGO = PlayerManager.instance.player;
        playerPreBattlePosition = playerGO.transform.position;
        playerPreBattleRotation = playerGO.transform.rotation;

        StartCoroutine(SetupBattle(enemy));
    }


    IEnumerator SetupBattle(GameObject enemy)
    {
        enemyStunCount = 0;
        enemyPoisonCount = 0;
        stunnedBefore = false;

        mainCam.enabled = false;
        battleCam.enabled = true;

        mainCanvas.enabled = false;
        battleCanvas.enabled = true;

        playerGO.GetComponent<NavMeshAgent>().Warp(playerBattleStation.position);
        playerGO.transform.rotation = playerBattleStation.rotation;
        playerUnit = playerGO.GetComponent<Unit>();
        playerGO.GetComponent<PlayerController>().enabled = false;

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
    void FaceTarget()
    {
        Vector3 direction = (playerGO.transform.position - enemyBattleStation.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        playerGO.transform.rotation = Quaternion.Slerp(playerGO.transform.rotation, lookRotation, Time.deltaTime * 5f);

    }

    IEnumerator PlayerAttack(int damage, string animationName)
    {
        playerGO.GetComponent<NavMeshAgent>().SetDestination(playerAttackSpot.position);
        yield return new WaitForSeconds(3f);

        Animator playerAnimator = playerGO.GetComponent<Animator>();
        playerAnimator.SetTrigger(animationName);
        yield return new WaitForSeconds(0.3f);

        Animator enemyAnimator = enemyGO.GetComponent<Animator>();
        enemyAnimator.SetTrigger("Jump");

        bool isDead = enemyUnit.TakeDamage(damage);
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "The attack is successful!";
        yield return new WaitForSeconds(1.5f);


        playerGO.GetComponent<NavMeshAgent>().SetDestination(playerBattleStation.position);
        yield return new WaitForSeconds(3f);

        playerGO.transform.rotation = playerBattleStation.rotation;


        if (isDead)
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        float enemyMoveProb = Random.Range(0f, 1f);
        bool isDead;

        Debug.Log(enemyMoveProb);
        if (enemyMoveProb < 0.15f && !stunnedBefore)
        {
            stunnedBefore = true;
            Animator enemyAnimator = enemyGO.GetComponent<Animator>();
            enemyAnimator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.7f);

            Animator playerAnimator = playerGO.GetComponent<Animator>();
            playerAnimator.SetTrigger("Take Damage");
            dialogueText.text = enemyUnit.unitName + " shoots web.";
            yield return new WaitForSeconds(1.5f);

            isDead = playerUnit.TakeDamage(enemyUnit.damage2);
            playerHUD.SetHP(playerUnit.currentHP);
            enemyStunCount = 1; // can also make this a random number
            yield return new WaitForSeconds(1f);
        }
        else if (enemyMoveProb < 0.45f)
        {
            stunnedBefore = false;
            if (enemyPoisonCount == 0)
            {
                enemyPoisonCount = Random.Range(0, 5);
            }

            Animator enemyAnimator = enemyGO.GetComponent<Animator>();
            enemyAnimator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.7f);

            Animator playerAnimator = playerGO.GetComponent<Animator>();
            playerAnimator.SetTrigger("Take Damage");
            dialogueText.text = enemyUnit.unitName + " poisons you!";
            yield return new WaitForSeconds(1.5f);

            isDead = playerUnit.TakeDamage(enemyUnit.damage2);
            playerHUD.SetHP(playerUnit.currentHP);
            yield return new WaitForSeconds(1f);
        }
        else
        {
            stunnedBefore = false;
            enemyGO.GetComponent<NavMeshAgent>().SetDestination(enemeyAttackSpot.position);
            yield return new WaitForSeconds(2.8f);

            Animator enemyAnimator = enemyGO.GetComponent<Animator>();
            enemyAnimator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.7f);

            Animator playerAnimator = playerGO.GetComponent<Animator>();
            playerAnimator.SetTrigger("Take Damage");
            dialogueText.text = enemyUnit.unitName + " attacks!";
            yield return new WaitForSeconds(1.5f);

            isDead = playerUnit.TakeDamage(enemyUnit.damage);
            playerHUD.SetHP(playerUnit.currentHP);
            yield return new WaitForSeconds(1f);

            enemyGO.GetComponent<NavMeshAgent>().SetDestination(enemyBattleStation.position);
            yield return new WaitForSeconds(3f);

            enemyGO.transform.rotation = enemyBattleStation.rotation;
        }

        if (enemyPoisonCount > 0)
        {
            Animator playerAnimator = playerGO.GetComponent<Animator>();
            playerAnimator.SetTrigger("Take Damage");
            dialogueText.text = playerUnit.unitName + " hurt by poison!";
            yield return new WaitForSeconds(1.5f);
            isDead = playerUnit.TakeDamage(enemyUnit.damage2);
            playerHUD.SetHP(playerUnit.currentHP);

            enemyPoisonCount--;
            if (enemyPoisonCount == 0)
            {
                dialogueText.text = "Poison has faded away.";
                yield return new WaitForSeconds(1.5f);
            }
        }

        if (isDead)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else if (enemyStunCount > 0)
        {
            enemyStunCount -= 1;
            dialogueText.text = "Stuck in web. Turn skipped.";
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(EnemyTurn());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

    }

    IEnumerator EndBattle()
    {
        if (state == BattleState.WON)
        {
            Animator enemyAnimator = enemyGO.GetComponent<Animator>();
            enemyAnimator.SetTrigger("Die");

            dialogueText.text = "You won the battle!";

            yield return new WaitForSeconds(1.5f);

            Destroy(enemyGO);

            mainCam.enabled = true;
            battleCam.enabled = false;

            mainCanvas.enabled = true;
            battleCanvas.enabled = false;

            playerGO.GetComponent<PlayerController>().enabled = true;
            playerGO.GetComponent<NavMeshAgent>().Warp(playerPreBattlePosition);
            playerGO.transform.rotation = playerPreBattleRotation;

        }
        else if (state == BattleState.LOST)
        {
            Animator playerAnimator = playerGO.GetComponent<Animator>();
            playerAnimator.SetTrigger("Die");
            dialogueText.text = "You were defeated.";
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(15);

        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "You feel renewed strength!";

        Animator playerAnimator = playerGO.GetComponent<Animator>();
        playerAnimator.SetTrigger("Cast Spell");
        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack(playerUnit.damage, "Stab Attack"));
    }

    public void OnSmashAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack(playerUnit.damage2, "Smash Attack"));
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal());
    }

}
