using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class EnemyController : MonoBehaviour
{

    public float lookRadius = 100f;
    public AudioClip overworld;
    public AudioClip battleScene;
    Transform target;
    NavMeshAgent agent;
    CharacterCombat combat;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        combat = GetComponent<CharacterCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);


        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                BattleSystem.instance.StartBattle(gameObject);

                lookRadius = 0;
                //CharacterStats targetStats = target.GetComponent<CharacterStats>();

                //if (targetStats != null)
                //{
                //    combat.Attack(targetStats);

                //}
                //FaceTarget();

            }

        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
