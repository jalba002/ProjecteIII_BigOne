using System;
using Enemy;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Dimitry_AnimatorController : MonoBehaviour
{
    [Header("Components")] public EnemyController EnemyController;
    public Animator enemyAnimator;
    public GameObject enemyHammer;

    [Header("Footstep Path")] public string footstepsPath;

    private Dimitry_Damage_Dealer _colliderController;

    private void Start()
    {
        if (EnemyController == null)
        {
            EnemyController = FindObjectOfType<EnemyController>();
        }

        if (_colliderController == null)
        { 
            _colliderController = gameObject.GetComponent<Dimitry_Damage_Dealer>();
            _colliderController.Init();
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

        EnemyController.stateMachine.OnStateChange.AddListener(UpdateAnimation);
    }

    public void LoadNewPhase()
    {
        if (EnemyController.currentBehaviourTree is BehaviourTree_Enemy_FirstPhase)
        {
            enemyAnimator.runtimeAnimatorController =
                (RuntimeAnimatorController) Resources.Load("Animations/Dimitry_Animator_FirstPhase");
            enemyHammer.SetActive(false);
            _colliderController.SetFistPosition();
        }
        else if (EnemyController.currentBehaviourTree is BehaviourTree_Enemy_SecondPhase)
        {
            enemyAnimator.runtimeAnimatorController =
                (RuntimeAnimatorController) Resources.Load("Animations/Dimitry_Animator_SecondPhase");
            enemyHammer.SetActive(true);
            _colliderController.SetHammerPosition();
        }
    }

    public void Update()
    {
        AnimationVariablesUpdate();
    }

    private void AnimationVariablesUpdate()
    {
        float enemySpeed = EnemyController.NavMeshAgent.velocity.magnitude;

        enemyAnimator.SetFloat("Speed", enemySpeed);

        if (enemySpeed > 0.5f)
        {
            enemyAnimator.SetBool("IsMoving", true);
        }
        else if (enemyAnimator.GetBool("IsMoving"))
        {
            enemyAnimator.SetBool("IsMoving", false);
        }
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
        if (EnemyController.stateMachine.GetCurrentState is State_Enemy_DestructionTraversing
            || EnemyController.stateMachine.GetCurrentState is State_Enemy_Traversing)
        {
            enemyAnimator.SetBool("Kicking", true);
        }
        else
        {
            enemyAnimator.SetBool("Kicking", false);
        }

        enemyAnimator.SetBool("IsLighted", EnemyController.stateMachine.GetCurrentState is State_Enemy_Lighted);
    }

    private void UpdateSecondPhase()
    {
        if (EnemyController.stateMachine.GetCurrentState is State_Enemy_DestructionTraversing
            || EnemyController.stateMachine.GetCurrentState is State_Enemy_Traversing)
        {
            enemyAnimator.SetBool("Kicking", true);
        }
        else
        {
            enemyAnimator.SetBool("Kicking", false);
        }
    }

    public void EndGame()
    {
        if (GameManager.Instance.GameSettings.isPlayerInvincible) return;

        GameManager.Instance.EndGame();
    }

    public void EnableDamage()
    {
        _colliderController.ToggleCollider(true);
    }

    public void DisableDamage()
    {
        _colliderController.ToggleCollider(false);
    }

    public void KickDoorOpen()
    {
        if (EnemyController.currentBrain.CurrentBlockage != null)
        {
            EnemyController.ResolveBlockage(EnemyController.currentBrain.CurrentBlockage);
            EnemyController.currentBrain.CurrentBlockage = null;
        }
    }

    public void BreakDoorOpen()
    {
        if (EnemyController.currentBrain.CurrentBlockage != null)
        {
            EnemyController.BreakBlockage(EnemyController.currentBrain.CurrentBlockage);
            EnemyController.currentBrain.CurrentBlockage = null;
        }
    }

    public void Footstep()
    {
        // Play sound of footsteps?
        try
        {
            SoundManager.Instance.PlayEvent(footstepsPath, EnemyController.transform, 1f, 10f);
        }
        catch (Exception e)
        {
            Debug.LogError("SOUND OF FOOTSTEPS NOT PLAYING");
        }
    }
}