using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockManager_Dice : MonoBehaviour
{
    public GameObject dice;
    public Text dice_Text;
    public int actualNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RotateLeft()
    {

        Vector3 rot = new Vector3(0, -90, 0);
        dice.transform.Rotate(rot);

        
            actualNumber -= 1;
            if (actualNumber < 0)
            {
                actualNumber = 9;
            }
            dice_Text.text = actualNumber.ToString();
       
    }
    public void RotateRight()
    {

        Vector3 rot = new Vector3(0, 90, 0);
        dice.transform.Rotate(rot);


        actualNumber += 1;
        if (actualNumber > 9)
        {
            actualNumber = 0;
        }
        dice_Text.text = actualNumber.ToString();
    }

}
