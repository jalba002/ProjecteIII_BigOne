using UnityEngine;

[CreateAssetMenu(fileName = "Game Settings", menuName = "Data/Game/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("General Settings")]
    public bool isPlayerInvincible = false;
    public bool showHints = true;
    
    [Header("Player")] 
    public int PlayerViewAngle = 94;
    
    [Header("Enemy")]
    public LayerMask DetectionLayers;
    
    [Header("Puzzles")] public LayerMask PuzzleElementsLayers;
}
