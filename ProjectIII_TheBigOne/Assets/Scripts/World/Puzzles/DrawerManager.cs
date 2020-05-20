using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public DrawerPiece dp1;
    public DrawerPiece dp2;
    public DrawerPiece dp3;
    public DrawerPiece dp4;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckAnswer()
    {
        if (dp1.isCorrect() && dp2.isCorrect() && dp3.isCorrect() && dp4.isCorrect())
        {
            //UnlockDoor
            Debug.Log("FUNCIONA");
        }
    }

}
