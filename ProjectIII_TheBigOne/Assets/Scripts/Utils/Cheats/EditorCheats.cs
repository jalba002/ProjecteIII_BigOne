using System;
using Characters.Player;
using Player;
using UnityEditor;
using UnityEngine;

namespace Cheats
{
#if UNITY_EDITOR
    public static class EditorCheats
    {
        /*[MenuItem("Cheats/Game/Complete Nearest Puzzle %#C")]
        public static void CompleteNearestPuzzle()
        {
            if (Application.isPlaying)
            {
                try
                {
                    var PuzzleFound =
                        GameManager.Instance.PlayerController.interactablesManager.registeredInteractables.Find(x =>
                            x.attachedGameobject.GetComponent<Puzzle>());
                    PuzzleFound.attachedGameobject.GetComponent<Puzzle>()?.OnPuzzleWin.Invoke();
                }
                catch (NullReferenceException)
                {
                    Debug.LogWarning("No puzzle detected.");
                }
            }
            else
            {
                Debug.LogWarning("Only in Play Mode!");
            }
        }*/

        [MenuItem("Cheats/Level/Destroy Wall %E")]
        public static void DestroyWall()
        {
            if (Application.isPlaying)
            {
                try
                {
                    Selection.activeGameObject.GetComponent<Destructible_Walls>().Activate();
                }
                catch (NullReferenceException)
                {
                    Debug.LogWarning("Selection is not a destructible wall!");
                }
            }
            else
            {
                Debug.LogWarning("Only in Play Mode!");
            }
        }

        [MenuItem("Cheats/Player/Clear Interactable %#R")]
        public static void ClearInteractable()
        {
            if (Application.isPlaying)
            {
                GameObject.FindObjectOfType<InteractablesManager>().ClearInteractable();
            }
            else
            {
                Debug.LogWarning("Only in Play Mode!");
            }
        }

        [MenuItem("Cheats/Player/Recharge Battery #R")]
        public static void RechargeBattery()
        {
            if (Application.isPlaying)
            {
                GameObject.FindObjectOfType<FlashlightController>().Recharge(999f);
            }
            else
            {
                Debug.LogWarning("Only in Play Mode!");
            }
        }

        [MenuItem("Cheats/Game/Stun Dimitry %K")]
        public static void StunDimitry()
        {
            try
            {
                if (Application.isPlaying)
                {
                    GameObject.FindObjectOfType<EnemyBrain>().IsStunned = true;
                    return;
                }
                else
                {
                    Debug.LogWarning("Only in Play Mode!");
                }
            }
            catch (NullReferenceException)
            {
                Debug.LogWarning("No Dimitry with brain in-game!");
            }
        }

        [MenuItem("Cheats/Enemy/Load First Phase %B")]
        public static void LoadFirstPhase()
        {
            try
            {
                if (Application.isPlaying)
                {
                    GameObject.FindObjectOfType<ParanormalManager>()?.StartFirstPhase();

                    return;
                }
                else
                {
                    Debug.LogWarning("Only in Play Mode!");
                }
            }
            catch (NullReferenceException)
            {
                Debug.LogWarning("No paranormal manager in-game!");
            }
        }

        [MenuItem("Cheats/Enemy/Load Second Phase %N")]
        public static void LoadSecondPhase()
        {
            try
            {
                if (Application.isPlaying)
                {
                    GameObject.FindObjectOfType<ParanormalManager>()?.StartSecondPhase();
                    return;
                }
                else
                {
                    Debug.LogWarning("Only in Play Mode!");
                }
            }
            catch (NullReferenceException)
            {
                Debug.LogWarning("No paranormal manager in-game!");
            }
        }
    }
#endif
}