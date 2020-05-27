using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerObjective : MonoBehaviour
{

    public enum TriggerType { NewObjective, Completed }
    public TriggerType triggerType = TriggerType.NewObjective;

    public int questID;
    public string questObjective;
    bool isTriggered;

    Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            OnTrigger();
        }
    }

    public void OnTrigger()
    {

        if (triggerType == TriggerType.Completed)
        {
            FindObjectOfType<CanvasController>().ObjectiveCompleted(questID);
        }

        if (triggerType == TriggerType.NewObjective)
        {
            triggerType = TriggerType.Completed;
            FindObjectOfType<CanvasController>().PushObjectiveText(questObjective, 3, false, questID);
        }

    }
}