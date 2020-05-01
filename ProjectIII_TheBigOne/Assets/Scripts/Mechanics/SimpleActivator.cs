using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class SimpleActivator : MonoBehaviour
{
    [Header("Configuration")] [Range(1f, 500f)]
    public float raycastDistance = 500f;

    public LayerMask interact;

    public float forceScale = 5f;

    public IMovable DetectedMovable;

    public bool Activate(Camera camera)
    {
        if (DetectedMovable == null)
        {
            var returnedObject = SimpleRaycast(camera);
            if (returnedObject != null)
                DetectedMovable = returnedObject;
            return false;
        }
        else
        {
            DetectedMovable.Use(CalculateForce(forceScale));
        }
        return true;
    }

    public bool Deactivate()
    {
        if (DetectedMovable == null) return false;

        DetectedMovable = null;

        Debug.Log("Deactivated Movable.");
        return true;
    }

    private float CalculateForce(float force = 1f)
    {
        var calculatedForce = 0f;
        float mouseY = Input.GetAxis("Mouse Y");
        calculatedForce = (force * mouseY);

        return calculatedForce;
    }

    /* public bool MoveTheRigidbodies(Camera camera)
     {
         bool worked = false;
         var rayResults = SimpleRaycast(camera);
         worked = ApplyForceOnPoint(rayResults.rigidbody, rayResults.point,
             CalculateForce(rayResults.normal * -1, forceScale));
         return worked;
     }*/

    IMovable SimpleRaycast(Camera camera)
    {
        RaycastHit hit;
        Ray cameraRay = camera.ViewportPointToRay(new Vector3(.5f, .5f, .5f));
        if (Physics.Raycast(cameraRay, out hit, raycastDistance, interact))
        {
            Debug.Log("Hit this: " + hit.transform.gameObject.name);
            try
            {
                IMovable objectCatched = hit.collider.gameObject.GetComponent<IMovable>();
                if (objectCatched != null) return objectCatched;
            }
            catch (NullReferenceException)
            {
            }
        }
        //Debug.DrawRay(cameraRay.origin, cameraRay.direction * hit.distance, Color.red, 2f);

        return null;
    }

    /*bool ApplyForceOnPoint(Rigidbody rigidbody, Vector3 hitPoint, Vector3 force)
    {
        if (rigidbody == null)
        {
            Debug.LogError("Rigidbody is null");
            return false;
        }

        rigidbody.AddForceAtPosition(force, hitPoint, ForceMode.Force);
        return true;
    }*/
}