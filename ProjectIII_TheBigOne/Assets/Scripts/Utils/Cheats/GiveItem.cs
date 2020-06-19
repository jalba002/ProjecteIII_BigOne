#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

public class GiveItem : EditorWindow
{
    [MenuItem("Cheats/Editor/Give Item")]
    public static void ShowWindow()
    {
        GetWindow<GiveItem>("Give Item");
    }

    string checkPointNumberString = "Write the item ID here.";

    private void OnGUI()
    {
        GUILayout.Label("Write the Item ID: ", EditorStyles.boldLabel);

        checkPointNumberString = EditorGUILayout.TextField("Item ID (String)", checkPointNumberString);

        if (GUILayout.Button("Give item to Player"))
        {
            if (!Application.isPlaying)
            {
                Debug.LogError("Game not in Play Mode");
                return;
            }

            try
            {
                GameManager.Instance.PlayerController.playerInventory.AddItem(checkPointNumberString);
            }
            catch (IndexOutOfRangeException)
            {
                Debug.LogWarning("Error when giving item.");
            }
        }
    }
}
#endif