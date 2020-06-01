using System;
using Enemy;
using UnityEngine;

public class Dimitry_AnimatorController : MonoBehaviour
{
    public EnemyController EnemyController;

    public Animator enemyAnimator;

    private void Start()
    {
        if (EnemyController == null)
        {
            EnemyController = FindObjectOfType<EnemyController>();
        }

        if (enemyAnimator == null)
        {
            enemyAnimator = EnemyController.GetComponentInChildren<Animator>();
        }
    }

    public void Update()
    {
        if (Vector3.Magnitude(EnemyController.NavMeshAgent.velocity) > 0f)
        {
            enemyAnimator.SetBool("IsWalking", true);
        }
        else if (enemyAnimator.GetBool("IsWalking"))
        {
            enemyAnimator.SetBool("IsWalking", false);
        }
    }
}