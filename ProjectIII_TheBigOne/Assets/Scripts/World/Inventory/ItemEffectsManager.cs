using UnityEngine;

namespace Tavaris.Manager
{
    public static class ItemEffectsManager
    {
        private static FlashlightController playerFlashlight;

        public static void ReloadLantern(float amount = 9999f)
        {
            Debug.Log("Reloading lantern.");
            if (playerFlashlight == null)
            {
                playerFlashlight = GameObject.FindObjectOfType<FlashlightController>();
            }

            playerFlashlight.Recharge(amount);
        }

        public static void PrintDebug()
        {
            Debug.Log("Effect Print Debug!");
        }

        public static void PickWithAxe() //prova
        {
            Debug.Log("You have found gold!");
        }
    }
}