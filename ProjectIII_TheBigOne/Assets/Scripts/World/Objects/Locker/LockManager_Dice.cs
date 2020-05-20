using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockManager_Dice : MonoBehaviour
{
    public TMPro.TMP_Text dice_Text;
    public int actualNumber = 0;

    public void RotateLeft()
    {
        actualNumber -= 1;
        if (actualNumber < 0)
        {
            actualNumber = 9;
        }

        dice_Text.text = actualNumber.ToString();
    }

    public void RotateRight()
    {
        actualNumber += 1;
        if (actualNumber > 9)
        {
            actualNumber = 0;
        }

        dice_Text.text = actualNumber.ToString();
    }
}