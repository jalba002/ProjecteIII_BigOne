using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndgamePuzzle : Puzzle
{
    public GameObject attachedQuestItem;

    public string requiredItemID = "";

    public bool alreadyCompleted { get; private set; }

    private void Start()
    {
        alreadyCompleted = false;
    }

    public override void StartGame()
    {
        if (alreadyCompleted) return;

        var playerInventory = GameManager.Instance.PlayerController.playerInventory;

        base.StartGame();
        var item = playerInventory.characterItems.Find(x =>
            x.itemID == requiredItemID);

        if (item != null) // Finds item in inventory
        {
            playerInventory.inventoryDisplay.RemoveItem(item);
            playerInventory.characterItems.Remove(item);
            SolvePuzzle();
        }
    }

    public override void SolvePuzzle()
    {
        Debug.Log("Solving Puzzle");
        alreadyCompleted = true;
        try
        {
            attachedQuestItem.SetActive(true);
        }
        catch (NullReferenceException)
        {
        }

        this.gameObject.layer = 0;
        
        OnPuzzleWin.Invoke();
    }
}