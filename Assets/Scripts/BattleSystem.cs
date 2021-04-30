using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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

    public ParticleSystem enemyEmitter;

    Unit playerUnit;
    Unit enemyUnit;

    // temporary feature
    // TODO: This is just a rip off of our expected game
    public int killCount = 0;

    public Text dialogueText;
    public Text specialStatus;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    public AudioClip attackAudio;
    public AudioSource audioSource;

    public GameObject smashAttackObj;
    public GameObject healObj;

    // Collection Buttons
    public Collections collections;
    public Text stoneText;
    public Text fireText;
    public Text poisonText;

    public AudioSource winSound;
    public AudioSource loseSound;

    Vector3 playerPreBattlePosition;
    Quaternion playerPreBattleRotation;
    int enemyStunCount;
    int enemyPoisonCount;
    bool stunnedBefore;
    bool isDefending;

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
        isDefending = false;

        specialStatus.text = "";

        mainCam.enabled = false;
        battleCam.enabled = true;


        playerGO.GetComponent<NavMeshAgent>().Warp(playerBattleStation.position);
        playerGO.transform.rotation = playerBattleStation.rotation;
        playerUnit = playerGO.GetComponent<Unit>();
        playerGO.GetComponent<PlayerController>().enabled = false;

        if (playerUnit.unitLevel < 3)
        {
            smashAttackObj.SetActive(false);
        }
        else
        {
            smashAttackObj.SetActive(true);
        }

        if (playerUnit.unitLevel < 2)
        {
            healObj.SetActive(false);
        }
        else
        {
            healObj.SetActive(true);
        }

        // Setting up Inventory
        string newcount = "x" + collections.numRocks.ToString();
        stoneText.text = newcount;

        newcount = "x" + collections.numFire.ToString();
        fireText.text = newcount;

        newcount = "x" + collections.numPoison.ToString();
        poisonText.text = newcount;

        mainCanvas.enabled = false;
        battleCanvas.enabled = true;


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
        state = BattleState.ENEMYTURN;
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
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        float enemyMoveProb = Random.Range(0f, 1f);
        bool isDead;

        Debug.Log(enemyMoveProb);
        if (enemyMoveProb < 0.15f && !stunnedBefore && enemyUnit.unitLevel > 2)
        {
            stunnedBefore = true;
            Animator enemyAnimator = enemyGO.GetComponent<Animator>();
            enemyAnimator.SetTrigger("Attack");

            enemyEmitter.startColor = new Color(1f, 1f, 1f);
            enemyEmitter.Play();

            dialogueText.text = enemyUnit.unitName + " shoots web.";
            yield return new WaitForSeconds(0.7f);

            if (isDefending)
            {
                int damage = (int)(enemyUnit.damage2 * 0.5);
                isDead = playerUnit.TakeDamage(damage);
            }
            else
            {
                Animator playerAnimator = playerGO.GetComponent<Animator>();
                playerAnimator.SetTrigger("Take Damage");
                isDead = playerUnit.TakeDamage(enemyUnit.damage2);
            }

            playerHUD.SetHP(playerUnit.currentHP);
            enemyStunCount = 1; // can also make this a random number
            yield return new WaitForSeconds(1f);
        }
        else if (enemyMoveProb < 0.45f && enemyUnit.unitLevel > 1)
        {
            stunnedBefore = false;
            if (enemyPoisonCount == 0)
            {
                enemyPoisonCount = Random.Range(0, 5);
            }

            Animator enemyAnimator = enemyGO.GetComponent<Animator>();
            enemyAnimator.SetTrigger("Attack");
            dialogueText.text = enemyUnit.unitName + " poisons you!";

            enemyEmitter.startColor = new Color(0.62f, .27f, 1f);
            enemyEmitter.Play();

            yield return new WaitForSeconds(0.7f);


            if (isDefending)
            {
                int damage = (int)(enemyUnit.damage2 * 0.5);
                isDead = playerUnit.TakeDamage(damage);
            }
            else
            {
                Animator playerAnimator = playerGO.GetComponent<Animator>();
                playerAnimator.SetTrigger("Take Damage");
                isDead = playerUnit.TakeDamage(enemyUnit.damage2);
            }

            playerHUD.SetHP(playerUnit.currentHP);
            yield return new WaitForSeconds(1f);
        }
        else
        {
            stunnedBefore = false;
            dialogueText.text = enemyUnit.unitName + " attacks!";
            enemyGO.GetComponent<NavMeshAgent>().SetDestination(enemeyAttackSpot.position);
            yield return new WaitForSeconds(2.8f);

            Animator enemyAnimator = enemyGO.GetComponent<Animator>();
            enemyAnimator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.7f);


            if (isDefending)
            {
                int damage = (int)(enemyUnit.damage * 0.5);
                isDead = playerUnit.TakeDamage(damage);
            }
            else
            {
                Animator playerAnimator = playerGO.GetComponent<Animator>();
                playerAnimator.SetTrigger("Take Damage");
                isDead = playerUnit.TakeDamage(enemyUnit.damage);
            }

            playerHUD.SetHP(playerUnit.currentHP);
            yield return new WaitForSeconds(1f);

            enemyGO.GetComponent<NavMeshAgent>().SetDestination(enemyBattleStation.position);
            yield return new WaitForSeconds(3f);

            enemyGO.transform.rotation = enemyBattleStation.rotation;
        }

        if (isDefending)
        {
            isDefending = false;
            Animator playerAnimator = playerGO.GetComponent<Animator>();
            playerAnimator.SetBool("Defend", isDefending);
        }

        if (enemyPoisonCount > 0)
        {
            Animator playerAnimator = playerGO.GetComponent<Animator>();
            playerAnimator.SetTrigger("Take Damage");
            specialStatus.color = new Color(0.62f, .27f, 1f);
            specialStatus.text = "PSN";
            dialogueText.text = playerUnit.unitName + " hurt by poison!";
            yield return new WaitForSeconds(1.5f);
            isDead = playerUnit.TakeDamage(enemyUnit.damage2);
            playerHUD.SetHP(playerUnit.currentHP);

            enemyPoisonCount--;
            if (enemyPoisonCount == 0)
            {
                dialogueText.text = "Poison has faded away.";
                specialStatus.text = "";
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
            yield return new WaitForSeconds(2f);
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

            winSound.Play();
            Animator enemyAnimator = enemyGO.GetComponent<Animator>();
            enemyAnimator.SetTrigger("Die");

            dialogueText.text = "You won the battle!";

            yield return new WaitForSeconds(1.5f);

            int earnedXP = (enemyUnit.unitLevel * 25) + Random.Range(1, 50);
            playerUnit.currentXP += earnedXP;
            dialogueText.text = "You earned " + earnedXP + " XP";
            yield return new WaitForSeconds(1.5f);

            if (playerUnit.currentXP < playerUnit.maxXP)
            {
                playerHUD.SetXP(playerUnit.currentXP);
            }
            else
            {
                playerHUD.SetXP(playerUnit.maxXP);
                dialogueText.text = "You leveled up!";

                // Commented Temporarily
                playerUnit.currentHP = (playerUnit.currentHP * 2 < playerUnit.maxHP) ? playerUnit.currentHP * 2 : playerUnit.maxHP;
                playerHUD.SetHP(playerUnit.currentHP);

                yield return new WaitForSeconds(2f);

                playerUnit.currentXP = playerUnit.currentXP % playerUnit.maxXP;
                playerUnit.unitLevel++;
                if (playerUnit.unitLevel == 2)
                {
                    dialogueText.text = "You learned to heal!";
                    yield return new WaitForSeconds(2f);
                }

                if (playerUnit.unitLevel == 3)
                {
                    dialogueText.text = "You learned smash attack!";
                    yield return new WaitForSeconds(2f);
                }
                playerUnit.maxXP = playerUnit.unitLevel * 50;
                playerHUD.SetHUD(playerUnit);

            }

            yield return new WaitForSeconds(2f);

            Destroy(enemyGO);

            mainCam.enabled = true;
            battleCam.enabled = false;

            mainCanvas.enabled = true;
            battleCanvas.enabled = false;

            playerGO.GetComponent<PlayerController>().enabled = true;
            playerGO.GetComponent<NavMeshAgent>().Warp(playerPreBattlePosition);
            playerGO.transform.rotation = playerPreBattleRotation;
            killCount++;

        }
        else if (state == BattleState.LOST)
        {
            loseSound.Play();
            Animator playerAnimator = playerGO.GetComponent<Animator>();
            playerAnimator.SetTrigger("Die");
            dialogueText.text = "You were defeated.";

            yield return new WaitForSeconds(2.0f);
            SceneManager.LoadScene(0);
        }

        // temporary if statement for the video assignment
        //if (killCount > 1)
        //{
        //    SceneManager.LoadScene(0);
        //}
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
    }

    IEnumerator PlayerHeal()
    {
        state = BattleState.ENEMYTURN;
        playerUnit.Heal(15);

        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "You feel renewed strength!";

        Animator playerAnimator = playerGO.GetComponent<Animator>();
        playerAnimator.SetTrigger("Cast Spell");
        yield return new WaitForSeconds(2f);

        if (enemyPoisonCount > 0)
        {
            enemyPoisonCount = 0;
            dialogueText.text = "Poison was removed!";
            specialStatus.text = "";
            yield return new WaitForSeconds(1.5f);
        }

        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerDefend()
    {
        state = BattleState.ENEMYTURN;
        dialogueText.text = "You've gone defensive!";
        isDefending = true;
        Animator playerAnimator = playerGO.GetComponent<Animator>();
        playerAnimator.SetBool("Defend", isDefending);
        yield return new WaitForSeconds(2f);

        StartCoroutine(EnemyTurn());
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;


        StartCoroutine(PlayerAttack(playerUnit.damage, "Stab Attack"));
    }

    public void OnDefendButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerDefend());
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

    public void useRockButton()
    {
        // Check if it is player's turn
        if (state != BattleState.PLAYERTURN)
            return;
        

        // Make move 
        // Change count
        collections.numRocks--;
        string newcount = "x" + collections.numRocks.ToString();
        stoneText.text = newcount;
    }

    public void useFireButton()
    {
        // Check if it is player's turn
        if (state != BattleState.PLAYERTURN)
            return;
        

        // Make move 
        // Change count
        collections.numFire--;
        string newcount = "x" + collections.numFire.ToString();
        fireText.text = newcount;
    }

    public void usePoisonButton()
    {
        // Check if it is player's turn
        if (state != BattleState.PLAYERTURN)
            return;
        

        // Make move 
        // Change count
        collections.numPoison--;
        string newcount = "x" + collections.numPoison.ToString();
        poisonText.text = newcount;
    }
}
