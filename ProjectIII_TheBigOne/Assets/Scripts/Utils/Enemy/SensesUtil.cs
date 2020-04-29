using Player;
using UnityEngine;
using CharacterController = Characters.Generic.CharacterController;

public static class SensesUtil
{
    public static bool IsInSight(GameObject instigator, GameObject target, float maxRange, LayerMask layerMask,
        bool debug = false)
    {
        bool HitTarget = false;
        Ray detectionRay = new Ray(instigator.transform.position, (target.transform.position - instigator.transform.position).normalized);
        RaycastHit raycastHitInfo;
        if (Physics.Raycast(detectionRay, out raycastHitInfo, maxRange, layerMask))
        {
            Debug.Log("Casted a laser!");
            if (raycastHitInfo.collider.gameObject.GetHashCode().Equals(target.gameObject.GetHashCode()))
            {
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

        return HitTarget;
    }

    public static bool HasFlashlightEnabled(PlayerController playerController)
    {
        return playerController.attachedFlashlight.IsFlashlightEnabled;
    }
}