using System;
using Player;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ParanormalTrigger : MonoBehaviour
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
        OnStartup.Invoke();
    }
    

    private void OnTriggerEnter(Collider other)
    {
        try
        {
            if (other.GetComponent<PlayerController>() != null)
            {
                Activate();
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
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

        OnActivation.Invoke();

        hasBeenTriggered = true;

        Debug.Log("Trigger activated!");
    }

    public void Restore()
    {
        if (hasBeenTriggered)
        {
            Debug.Log("Restoring trigger", this.gameObject);
            hasBeenTriggered = false;
        }
    }
}