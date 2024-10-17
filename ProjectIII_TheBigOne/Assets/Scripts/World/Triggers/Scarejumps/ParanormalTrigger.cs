using System;
using UnityEngine;
using UnityEngine.Events;

namespace Tavaris.Triggers
{
    [RequireComponent(typeof(Collider))]
    public class ParanormalTrigger : MonoBehaviour, IRestartable
    {
        [SerializeField]
        private bool ActivateOnce = false;

        public Animation[] Animations;
        public UnityEvent OnStartup;
        public UnityEvent OnActivation;

        private bool hasBeenTriggered = false;
        private Collider TriggerArea;

        public void Awake()
        {
            TriggerArea = GetComponent<Collider>();
        }

        public void Start()
        {
            hasBeenTriggered = false;
            OnStartup?.Invoke();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == GameManager.Player.gameObject)
            {
                Activate();
            }
        }

        public void Activate()
        {
            if (ActivateOnce)
            {
                if (hasBeenTriggered)
                {
                    Debug.Log("Scare already triggered!");
                    return;
                }
            }

            foreach (Animation animation in Animations)
            {
                animation.Play();
            }

            OnActivation?.Invoke();

            hasBeenTriggered = true;

            Debug.Log("Trigger activated!");
        }

        public void Restart()
        {
            if (hasBeenTriggered)
            {
                Debug.Log("Restoring trigger", this.gameObject);
                hasBeenTriggered = false;
            }
        }
    }
}