using UnityEngine;

[CreateAssetMenu(fileName = "Game Settings", menuName = "Data/Game/Game Settings")]
public class GameSettings : ScriptableObject
{
    public int PlayerViewAngle = 94;
    public float PlayerViewRange = 25f;
    public float EnemyViewRange = 100f;
    public LayerMask DetectionLayers;

    [Header("Puzzles")] public LayerMask PuzzleElementsLayers;

    [Header("Player")] public bool isPlayerInvincible = false;
}
