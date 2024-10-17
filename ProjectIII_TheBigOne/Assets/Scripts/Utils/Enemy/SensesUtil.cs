using Tavaris.Entities;
using UnityEngine;

namespace Tavaris.Utils
{
    public static class SensesUtil
    {
        public static bool IsInSight(GameObject instigator, GameObject hitGameobject, Transform targetPosition,
            float maxRange, LayerMask layerMask,
            bool debug = false, float angle = 140f)
        {
            bool HitTarget = false;
            Ray detectionRay = new Ray(instigator.transform.position,
                (targetPosition.position - instigator.transform.position).normalized);
            RaycastHit raycastHitInfo;
            if (Physics.Raycast(detectionRay, out raycastHitInfo, maxRange, layerMask))
            {
                if (debug)
                    Debug.Log("Casted a laser!");
                if (raycastHitInfo.collider.gameObject == hitGameobject)
                {
                    if (debug)
                        Debug.Log($"Hit my target ··> {raycastHitInfo.collider.gameObject.name}");
                    HitTarget = true;
                }
            }

            if (debug)
            {
                if (HitTarget)
                {
                    Debug.DrawRay(detectionRay.origin, detectionRay.direction * raycastHitInfo.distance, Color.green, 5f);
                }
                else
                {
                    Debug.DrawRay(detectionRay.origin, detectionRay.direction * maxRange, Color.red, 1f);
                }
            }

            float l_DotAngle = Vector3.Dot(detectionRay.direction, instigator.transform.forward);

            return HitTarget && l_DotAngle > Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad);
        }

        public static bool IsHearingPlayer(EnemyController enemyController, PlayerController playerController,
            float maxRange, LayerMask cantHearThroughLayers, bool debug = false)
        {
            bool hearingTarget = false;
            Vector3 enemyPos = enemyController.gameObject.transform.position;
            Ray hearingRay = new Ray(enemyPos,
                (playerController.gameObject.transform.position - enemyPos));
            if (Physics.Raycast(hearingRay, out var hitInfo, maxRange, cantHearThroughLayers))
            {
                if (hitInfo.collider.gameObject == playerController.gameObject)
                {
                    if (debug)
                        Debug.Log("Hearing player!");
                    hearingTarget = true;
                }
            }

            if (debug)
            {
                if (hearingTarget)
                {
                    Debug.DrawRay(hearingRay.origin, hearingRay.direction * hitInfo.distance, Color.blue, 1f);
                }
                else
                {
                    Debug.DrawRay(hearingRay.origin, hearingRay.direction * maxRange, Color.yellow, 0.5f);
                }
            }

            return hearingTarget;
        }

        public static bool HasFlashlightEnabled(PlayerController playerController)
        {
            if (playerController == null) return false;
            return playerController.attachedFlashlight.IsFlashlightEnabled;
        }

        public static bool IsPlayerSeeingEnemy(PlayerController player, EnemyController enemy,
            LayerMask layerMask, float coneAngle = 94f)
        {
            if (player == null || enemy == null) return false;

            Vector3 l_Direction = enemy.transform.position - player.transform.position;
            Ray l_Ray = new Ray(player.cameraController.transform.position, l_Direction);

            float l_Distance = l_Direction.magnitude;
            l_Direction /= l_Distance;

            RaycastHit hitInfo;

            bool l_Collides = Physics.Raycast(l_Ray, out hitInfo, l_Distance, layerMask);
            if (l_Collides && hitInfo.collider.gameObject == enemy.gameObject)
            {
                l_Collides = false;
            }

            float l_DotAngle = Vector3.Dot(l_Direction, player.cameraController.transform.forward);

            Debug.DrawRay(l_Ray.origin, l_Ray.direction * l_Distance, l_Collides ? Color.red : Color.green);

            return !l_Collides && l_DotAngle > Mathf.Cos(coneAngle * 0.5f * Mathf.Deg2Rad);
        }
    }
}