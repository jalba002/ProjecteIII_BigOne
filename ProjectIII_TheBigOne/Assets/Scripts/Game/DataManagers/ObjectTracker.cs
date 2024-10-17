using System.Collections.Generic;
using System.Linq;
using Tavaris.AI;
using Tavaris.Dynamic;
using Tavaris.Interactable;
using UnityEngine;

namespace Tavaris.Manager
{
    public class ObjectTracker : MonoBehaviour
    {
        public static List<Door> doorList = new List<Door>();

        public static List<Drawer> drawerList = new List<Drawer>();

        public static List<TraversableBlockage> palletList = new List<TraversableBlockage>();

        public static List<InteractableObject> interactablesList = new List<InteractableObject>();

        public static List<PatrolPoint> PatrolPointsFirstPhase = new List<PatrolPoint>();

        public static List<PatrolPoint> PatrolPointsSecondPhase = new List<PatrolPoint>();

        // Start is called before the first frame update
        void Start()
        {
            doorList.AddRange(FindObjectsOfType<Door>());
            drawerList.AddRange(FindObjectsOfType<Drawer>());
            palletList.AddRange(FindObjectsOfType<TraversableBlockage>());
            interactablesList.AddRange(FindObjectsOfType<InteractableObject>());
            SplitPatrolPoints();
        }

        private void SplitPatrolPoints()
        {
            var patrolPoints = FindObjectsOfType<PatrolPoint>().ToList();
            PatrolPointsFirstPhase.AddRange(patrolPoints.Where(x => x.IsMainPhase));
            PatrolPointsSecondPhase.AddRange(patrolPoints.Where(x => !x.IsMainPhase));
        }
    }
}