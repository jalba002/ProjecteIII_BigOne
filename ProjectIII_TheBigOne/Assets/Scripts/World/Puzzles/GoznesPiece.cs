using UnityEngine;

namespace World.Puzzles
{
    public class GoznesPiece : PuzzlePiece
    {
        // Start is called before the first frame update
        [Header("Settings")] [Range(0f, 360f)] public int startingRotation;
        [Range(0f, 360f)] public int correctRot;

        private int currentRot;

        //private GoznesPuzzle goznesPuzzle;

        public void Start()
        {
            transform.eulerAngles =
                new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, startingRotation);
            currentRot = startingRotation;
        }

        public void RotateEulerY(int grades = 5)
        {
            currentRot += grades;
            if (currentRot >= 360)
                currentRot = currentRot - 360;
            //transform.eulerAngles = transform.eulerAngles + new Vector3(0,grades,0);
            //attachedDrawerLocker.eulerAngles = attachedDrawerLocker.eulerAngles + new Vector3(grades, 0, 0);
            transform.Rotate(0, 0, grades);
            //goznesPuzzle.CheckAnswer();
        }

        public bool isCorrect()
        {
            /*if(currentRot == correctRot)
        {
            Debug.Log("Correct");
        }*/
            return (currentRot == correctRot);
        }
    }
}