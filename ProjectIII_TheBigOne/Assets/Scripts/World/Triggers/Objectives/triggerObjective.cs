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
            OnTrigger();
    }

    public void OnTrigger()
    {
        if (triggerType == TriggerType.Completed && !GameManager.Instance.CanvasController.CheckEmtpyObjectiveList())
        {
            if (GameManager.Instance.CanvasController.CheckObjectiveIDList(questID)) //if quest has been received
            {
                GameManager.Instance.CanvasController.ObjectiveCompleted(questID);
                isTriggered = true;
            }
        }

        if (triggerType == TriggerType.NewObjective)
        {
            FindObjectOfType<CanvasController>().PushObjectiveText(questObjective, 3, false, questID);
            isTriggered = true;
        }
    }
}