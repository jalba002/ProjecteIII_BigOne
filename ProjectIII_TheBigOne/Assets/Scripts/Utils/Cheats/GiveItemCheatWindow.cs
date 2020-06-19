#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

// Creates an instance of a primitive depending on the option selected by the user.
public class GiveItemCheatWindow : EditorWindow
{
    public string[] options = new string[] {"Cube", "Sphere", "Plane"};
    public int index = 0;

    [MenuItem("Cheats/Game/Give Item Cheat Window")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(GiveItemCheatWindow));
        window.Show();
    }

    void OnGUI()
    {
        ReadyList();
        
        index = EditorGUILayout.Popup(index, options);
        
        if (GUILayout.Button("Give Item"))
        {
            GiveTheItem(options[index]);
        }
    }

    void ReadyList()
    {
        List<InventoryItem> inventoryItems = GameManager.Instance.PlayerController.playerInventory.database.itemList;
        options = new string[inventoryItems.Count];
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            options[i] = inventoryItems[i].itemID;
        }
    }

    void GiveTheItem(string itemId)
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Game not in Play Mode");
            return;
        }

        try
        {
            GameManager.Instance.PlayerController.playerInventory.AddItem(itemId);
        }
        catch (IndexOutOfRangeException)
        {
            Debug.LogWarning("Error when giving item.");
        }
    }
}
#endif
