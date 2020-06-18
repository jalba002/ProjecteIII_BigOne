using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class Dimitry_NoticingComplete : StateMachineBehaviour
{
    private EnemyBrain enemyBrain;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyBrain = FindObjectOfType<EnemyController>().currentBrain;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemyBrain.IsPlayerInSight)
        {
            animator.SetBool("IsNoticing", false);
            enemyBrain.IsChasingPlayer = true;
            enemyBrain.IsNoticingPlayer = false;
            enemyBrain.HasSucceededToNotice = false;
            enemyBrain.HasFailedToNotice = false;
        }
        if (enemyBrain.HasSucceededToNotice)
        {
            animator.SetTrigger("NoticeConfirmed");
        }
        if (!enemyBrain.IsPlayerInSight && stateInfo.normalizedTime >= 0.8f)
        {
            animator.SetBool("IsNoticing", false);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}