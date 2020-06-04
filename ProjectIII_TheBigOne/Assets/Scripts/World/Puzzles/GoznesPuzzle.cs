using System;
using UnityEngine;

namespace World.Puzzles
{
    public class GoznesPuzzle : Puzzle
    {
        [Header("Components")] public GoznesPiece piece1;
        public GoznesPiece piece2;
        public GoznesPiece piece3;
        public GoznesPiece piece4;

        private bool _done = false;

        private void Start()
        {
            _done = false;
        }

        public void CheckAnswer()
        {
            if (piece1.isCorrect() && piece2.isCorrect() && piece3.isCorrect() && piece4.isCorrect() && !_done)
            {
                //UnlockDoor
                _done = true;
                OnPuzzleWin.Invoke();
                Debug.Log("FUNCIONA");
            }
        }
    }
}