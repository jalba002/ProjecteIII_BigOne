using Tavaris.Entities;
using UnityEngine;
using UnityEngine.AI;

namespace Tavaris.Manager
{
    public class PathDrawer : MonoBehaviour
    {
        public LineRenderer lineRenderer;

        public Transform target;

        public NavMeshAgent trackedAgent;

        // Start is called before the first frame update
        void Start()
        {
            if (trackedAgent == null)
                trackedAgent = FindObjectOfType<EnemyController>().NavMeshAgent;
            target = trackedAgent.transform;
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            GetPath();
            DrawPath();
        }

        void GetPath()
        {
            lineRenderer.SetPosition(0, target.position);
        }

        void DrawPath()
        {
            while (trackedAgent.pathPending)
            {
                return;
            }

            if (trackedAgent.path.corners.Length < 2)
            {
                // return
            }
            else
            {
                lineRenderer.positionCount = trackedAgent.path.corners.Length;

                for (int i = 0; i < trackedAgent.path.corners.Length; i++)
                {
                    lineRenderer.SetPosition(i, trackedAgent.path.corners[i]);
                }
            }
        }
    }
}