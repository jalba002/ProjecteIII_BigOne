using System;
using Enemy;
using UnityEngine;

public class Dimitry_AnimatorController : MonoBehaviour
{
    [Header("Components")] public EnemyController EnemyController;

    public Animator enemyAnimator;
    public GameObject enemyHammer;

    private void Start()
    {
        if (EnemyController == null)
        {
            EnemyController = FindObjectOfType<EnemyController>();
        }

        if (enemyHammer == null)
        {
            Debug.LogError("No Hammer attached in Dimitry.", this);
        }

        if (enemyAnimator == null)
        {
            enemyAnimator = EnemyController.GetComponentInChildren<Animator>();
        }

        if (EnemyController != null)
        {
            EnemyController.OnPhaseChange.AddListener(LoadNewPhase);
        }
    }

    public void LoadNewPhase()
    {
        if (EnemyController.currentBehaviourTree is BehaviourTree_Enemy_FirstPhase)
        {
            enemyAnimator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animations/Dimitry_Animator_FirstPhase");
            enemyHammer.SetActive(false);
        }
        else if (EnemyController.currentBehaviourTree is BehaviourTree_Enemy_SecondPhase)
        {
            enemyAnimator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animations/Dimitry_Animator_SecondPhase");
            enemyHammer.SetActive(true);
        }
    }

    public void Update()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        switch (EnemyController.currentBehaviourTree)
        {
            case BehaviourTree_Enemy_FirstPhase behaviourTreeEnemyFirstPhase:
            case BehaviourTree_Enemy_Halted behaviourTreeEnemyHalted:
            case BehaviourTree_Enemy_OnlyChase behaviourTreeEnemyOnlyChase:
                UpdateFirstPhase();
                break;
            case BehaviourTree_Enemy_SecondPhase behaviourTreeEnemySecondPhase:
                UpdateSecondPhase();
                break;
        }
    }

    private void UpdateFirstPhase()
    {
        float enemySpeed = EnemyController.NavMeshAgent.velocity.magnitude;
        
        if (EnemyController.stateMachine.GetCurrentState is State_Enemy_DestructionTraversing
            || EnemyController.stateMachine.GetCurrentState is State_Enemy_Traversing)
        {
            //if(enemyAnimator.)
            enemyAnimator.SetBool("Kicking", true);
        }
        else
        {
            enemyAnimator.SetBool("Kicking", false);
        }

        enemyAnimator.SetFloat("Speed", enemySpeed);

        enemyAnimator.SetBool("IsLighted", EnemyController.stateMachine.GetCurrentState is State_Enemy_Idle);

        if (enemySpeed > 0f)
        {
            enemyAnimator.SetBool("IsMoving", true);
        }
        else if (enemyAnimator.GetBool("IsMoving"))
        {
            enemyAnimator.SetBool("IsMoving", false);
        }

        if (EnemyController.stateMachine.GetCurrentState is State_Enemy_Noticed)
        {
            enemyAnimator.SetBool("IsNoticing", true);
        }
        else if (EnemyController.stateMachine.GetCurrentState is State_Enemy_NoticedFailed)
        {
            enemyAnimator.SetTrigger("NoticedFailed");
        }
    }

    private void UpdateSecondPhase()
    {
        
    }

    public void EndGame()
    {
        if (GameManager.Instance.GameSettings.isPlayerInvincible) return;
        
        GameManager.Instance.EndGame();
    }
}