using Enemy;
using UnityEngine;

// Also known as the cunt that always tries to scare you.
public class ParanormalManager : MonoBehaviour
{
    private EnemyController Dimitry;

    public void Start()
    {
        Dimitry = FindObjectOfType<EnemyController>();
    }

}
