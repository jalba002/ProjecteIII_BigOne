using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using CharacterController = Characters.Generic.CharacterController;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public GameObject NotificationPrefab;
    public GameObject NotificationPanel;
    public Slider lightingSlider;
    public Slider runningSlider;
    private FlashlightController flashligh;
    private PlayerController playerController;
    private State_Player_Walking playerWalking;


    private void Start()
    {
        flashligh = FindObjectOfType<FlashlightController>();
        playerController = FindObjectOfType<PlayerController>();
        playerWalking = FindObjectOfType<State_Player_Walking>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            AddPickupMessage("key");
        }

        playerLightingUpdate();
        playerRunningUpdate();

        if (playerWalking.currentStamina >= playerController.characterProperties.maximumStamina)
            runningSlider.gameObject.SetActive(false);
        else
            runningSlider.gameObject.SetActive(true);
    }

    public void AddPickupMessage(string itemName)
    {
        GameObject Message = Instantiate(NotificationPrefab, NotificationPanel.transform);
        //Notifications.Add(Message);
        //Message.GetComponent<Notification>().SetMessage(string.Format("Picked up {0}", itemName));
        Message.GetComponent<Notification>().SetMessage(itemName + " COLLECTED");
    }

    public void playerLightingUpdate()
    {
        lightingSlider.value = flashligh.currentCharge / flashligh.maxCharge;
    }

    public void playerRunningUpdate()
    {
        runningSlider.value = playerWalking.currentStamina / playerController.characterProperties.maximumStamina;
    }
}
