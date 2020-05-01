using UnityEditor;
using UnityEngine;

namespace Cheats
{
    public static class EditorCheats
    {
        [MenuItem("Cheats/Player/Recharge Battery #R")]
        public static void RechargeBattery()
        {
            if (Application.isPlaying)
            {
                GameObject.FindObjectOfType<FlashlightController>().Recharge(999f);
            }
            else
            {
                Debug.LogError("Application is not in play mode.");
            }
        }
    }
}
