using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerPiece : Puzzle
{
    // Start is called before the first frame update
    public Transform attachedDrawerLocker;
    public int correctRot;
    public int currentRot;

    private DrawerManager dm;

    void Start()
    {
        dm = GameObject.FindObjectOfType<DrawerManager>();
        
    }     
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            attachedDrawerLocker.eulerAngles += new Vector3(5, 0, 0) * Time.deltaTime;
        }
    }

    public void RotateEulerY(int grades = 5)
    {        
        currentRot += grades;
        if (currentRot >= 360)
            currentRot = currentRot - 360;

        Debug.Log(currentRot);
        transform.eulerAngles = transform.eulerAngles + new Vector3(0,grades,0);
        //attachedDrawerLocker.eulerAngles = attachedDrawerLocker.eulerAngles + new Vector3(grades, 0, 0);
        attachedDrawerLocker.Rotate(0, grades, 0);
        dm.CheckAnswer();
    }

    public bool isCorrect()
    {
        if(currentRot == correctRot)
        {
            Debug.Log("Correct");
        }
        return (currentRot == correctRot);
    }
}
