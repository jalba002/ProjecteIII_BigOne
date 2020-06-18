using Enemy;
using UnityEngine;

public class Dmitry_EndConfirmation : StateMachineBehaviour
{
    private EnemyBrain enemyBrain;
    private Vector3 finalPosition;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyBrain = FindObjectOfType<EnemyController>().currentBrain;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.length >= 0.99f)
        {
            finalPosition = GameObject.Find("Base HumanLPlatform").transform.position;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyBrain.HasSucceededToNotice = false;
        enemyBrain.IsNoticingPlayer = false;
        enemyBrain.IsPlayerInSight = true;

        EnemyController attachedController = enemyBrain.selfCharacter;

        attachedController.NavMeshAgent.updateRotation = false;
        attachedController.NavMeshAgent.updatePosition = false;
        
        attachedController.transform.forward = (attachedController.currentBrain.archnemesis.transform.position -
                                                 attachedController.gameObject.transform.position).normalized;
        
        attachedController.NavMeshAgent.Warp(
            //attachedController.GetComponentInChildren<Animator>().transform.TransformPoint
                (finalPosition + new Vector3(0f, 1f, 0f)));
        
        attachedController.NavMeshAgent.isStopped = false;
        attachedController.NavMeshAgent.updatePosition = true;
        attachedController.NavMeshAgent.updateRotation = true;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}