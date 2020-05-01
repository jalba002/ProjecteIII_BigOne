using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleActivator : MonoBehaviour
{
    [Header("Configuration")] [Range(1f, 500f)]
    public float raycastDistance = 500f;

    public float forceScale = 5f;

    [Header("Private Variables")] [Tooltip("Door variables")]
    private RaycastHit _selectedDoor;


    public bool Activate(Camera camera)
    {
        if (_selectedDoor.rigidbody == null)
        {
            _selectedDoor = SimpleRaycast(camera);
            return false;
        }    
        ApplyForceOnPoint(_selectedDoor.rigidbody, _selectedDoor.point,
            CalculateForce(_selectedDoor.normal * -1, forceScale));
        return true;
    }

    public bool Deactivate()
    {
        if (_selectedDoor.rigidbody == null) return false;

        _selectedDoor = new RaycastHit()
        {
            rigidbody =
            {
            }
        };

        Debug.Log("Deactivated door.");
        return true;
    }


    private Vector3 CalculateForce(Vector3 direction, float force = 1f)
    {
        Vector3 calculatedForce = Vector3.zero;
        float mouseY = Input.GetAxis("Mouse Y");
        //Debug.Log("The mouse movement is " + mouseY);
        calculatedForce = direction * (force * mouseY);

        return calculatedForce;
    }

    public bool MoveTheRigidbodies(Camera camera)
    {
        bool worked = false;
        var rayResults = SimpleRaycast(camera);
        worked = ApplyForceOnPoint(rayResults.rigidbody, rayResults.point,
            CalculateForce(rayResults.normal * -1, forceScale));
        return worked;
    }

    RaycastHit SimpleRaycast(Camera camera)
    {
        RaycastHit hit;
        Ray cameraRay = camera.ViewportPointToRay(new Vector3(.5f, .5f, .5f));
        if (Physics.Raycast(cameraRay, out hit, raycastDistance))
        {
            Debug.Log("Hit this: " + hit.transform.gameObject.name);
        }
        //Debug.DrawRay(cameraRay.origin, cameraRay.direction * hit.distance, Color.red, 2f);

        return hit;
    }

    bool ApplyForceOnPoint(Rigidbody rigidbody, Vector3 hitPoint, Vector3 force)
    {
        if (rigidbody == null)
        {
            Debug.LogError("Rigidbody is null");
            return false;
        }

        rigidbody.AddForceAtPosition(force, hitPoint, ForceMode.Force);
        return true;
    }
}