using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public GameObject NotificationPrefab;
    public GameObject NotificationPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            AddPickupMessage("key");
        }
    }

    public void AddPickupMessage(string itemName)
    {
        GameObject Message = Instantiate(NotificationPrefab, NotificationPanel.transform);
        //Notifications.Add(Message);
        //Message.GetComponent<Notification>().SetMessage(string.Format("Picked up {0}", itemName));
        Message.GetComponent<Notification>().SetMessage(itemName + " COLLECTED");
    }
}
