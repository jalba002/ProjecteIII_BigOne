using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DrawerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public DrawerPiece dp1;
    public DrawerPiece dp2;
    public DrawerPiece dp3;
    public DrawerPiece dp4;


    private bool _done;
    public UnityEvent puzzleWon;
    

    void Start()
    {
        _done = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckAnswer()
    {
        if (dp1.isCorrect() && dp2.isCorrect() && dp3.isCorrect() && dp4.isCorrect() && !_done)
        {
            //UnlockDoor
            _done = true;
            puzzleWon.Invoke();
            Debug.Log("FUNCIONA");
        }
    }

}
