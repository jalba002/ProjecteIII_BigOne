using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;

public class SimpleActivator : MonoBehaviour
{
    [Header("Configuration")] [Range(1f, 500f)]
    public float raycastDistance = 500f;

    public LayerMask interact;

    public float forceScale = 5f;

    public IInteractable detectedInteractable;

    public UnityEvent OnObjectActivate;
    public UnityEvent OnObjectDeactivate;

    public bool Activate(Camera camera)
    {
        if (detectedInteractable == null)
        {
            var returnedObject = SimpleRaycast(camera);
            if (returnedObject != null)
            {
                detectedInteractable = returnedObject;
                detectedInteractable.OnStartInteract();
            }
            return false;
        }
        else
        {
            detectedInteractable.Interact();
            OnObjectActivate.Invoke();
            return true;
        }
    }

    public bool Deactivate()
    {
        if (detectedInteractable == null) return false;

        detectedInteractable = null;
        OnObjectDeactivate.Invoke();
        return true;
    }


    /* public bool MoveTheRigidbodies(Camera camera)
     {
         bool worked = false;
         var rayResults = SimpleRaycast(camera);
         worked = ApplyForceOnPoint(rayResults.rigidbody, rayResults.point,
             CalculateForce(rayResults.normal * -1, forceScale));
         return worked;
     }*/

    IInteractable SimpleRaycast(Camera camera)
    {
        RaycastHit hit;
        Ray cameraRay = camera.ViewportPointToRay(new Vector3(.5f, .5f, .5f));
        if (Physics.Raycast(cameraRay, out hit, raycastDistance, interact))
        {
            Debug.Log("Hit this: " + hit.transform.gameObject.name);
            try
            {
                IInteractable objectCatched = hit.collider.gameObject.GetComponent<IInteractable>();
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