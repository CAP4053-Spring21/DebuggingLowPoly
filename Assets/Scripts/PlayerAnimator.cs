using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimator : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    PlayerMotor motor;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isMoving = agent.velocity.magnitude > 0;
        if (isMoving)
        {
            if (motor.isRunning)
            {
                animator.SetBool("Run Forward", true);
                animator.SetBool("Walk Forward", false);
            }
            else
            {
                animator.SetBool("Walk Forward", true);
                animator.SetBool("Run Forward", false);
            }
        }
        else
        {
            animator.SetBool("Run Forward", false);
            animator.SetBool("Walk Forward", false);
        }
    }
}
