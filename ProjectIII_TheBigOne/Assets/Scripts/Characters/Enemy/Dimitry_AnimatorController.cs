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
        /*if (EnemyController.stateMachine.GetCurrentState is State_Enemy_Killing || EnemyController.stateMachine.GetCurrentState is State_Enemy_DestructionTraversing
            || EnemyController.stateMachine.GetCurrentState is State_Enemy_Traversing)
        {
            //if(enemyAnimator.)
            enemyAnimator.SetBool("Attack", true);
        }
        else
        {
            enemyAnimator.SetBool("Attack", false);
        }*/

        enemyAnimator.SetFloat("Speed", EnemyController.NavMeshAgent.speed);

        enemyAnimator.SetBool("IsLighted", EnemyController.stateMachine.GetCurrentState is State_Enemy_Idle);

        if (Vector3.Magnitude(EnemyController.NavMeshAgent.velocity) > 0.5f)
        {
            enemyAnimator.SetBool("IsWalking", true);
        }
        else if (enemyAnimator.GetBool("IsWalking"))
        {
            enemyAnimator.SetBool("IsWalking", false);
        }
    }

    public void EndGame()
    {
        if (GameManager.Instance.GameSettings.isPlayerInvincible) return;
        
        GameManager.Instance.EndGame();
    }
}