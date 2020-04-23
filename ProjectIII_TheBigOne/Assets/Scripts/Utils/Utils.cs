using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    /// <summary>
    /// Returns true of false depending of the raycast.
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="raycastDistance"></param>
    /// <param name="raycastHit"></param>
    /// <param name="debug"></param>
    /// <returns></returns>
    public static bool SimpleRaycast(Camera camera, float raycastDistance, out RaycastHit raycastHit, bool debug = false)
    {
        bool success = false;
        
        Ray cameraRay = camera.ViewportPointToRay(new Vector3(.5f, .5f, .5f));
        if (Physics.Raycast(cameraRay, out raycastHit, raycastDistance))
        {
            Debug.Log("Hit this: " + raycastHit.transform.gameObject.name);
            success = true;
        }

        if (debug)
            Debug.DrawRay(cameraRay.origin, cameraRay.direction * raycastHit.distance, Color.blue, 2f);
        
        return success;
    }
}