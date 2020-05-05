using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LockManager : MonoBehaviour
{
    public int solutionNumber0 = 1;
    public int solutionNumber1 = 1;
    public int solutionNumber2 = 1;

    public LockManager_Dice dice0;
    public LockManager_Dice dice1;
    public LockManager_Dice dice2;

    private bool _isCompleted = false;

    public UnityEvent whatUnlocks;
 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (solutionNumber0 == dice0.actualNumber && solutionNumber1 == dice1.actualNumber && solutionNumber2 == dice2.actualNumber && !_isCompleted)
        {
            Debug.Log("Congrats!");
            _isCompleted = true;
            //TODO: unlock door
            whatUnlocks.Invoke();
        }
    }

    
}
