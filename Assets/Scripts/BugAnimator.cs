using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BugAnimator : MonoBehaviour
{
    const float animationSmoothTime = 0.1f;
    NavMeshAgent agent;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float speedPercent = agent.velocity.magnitude / agent.speed;
        animator.SetBool("Walk Forward", speedPercent > 0);
        //animator.SetFloat("speedPercent", speedPercent, animationSmoothTime, Time.deltaTime); ;
    }
}
