using System;
using Interfaces;
using Player;
using UnityEngine;

public class ObjectInspector : MonoBehaviour
{
    public IInspectable currentInspectedObject;

    [Header("Render output")] public InspectedElement objectRenderer;

    [Header("Render Settings")] public Vector2 distances = new Vector2(1.5f, 2f);

    // And more parameters to come.
    public Vector2 rotationSpeed = new Vector2(0.5f, 0.5f);

    // Private parameters.
    private PlayerController playerController;

    public void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public void Update()
    {
        UpdateRenderer();
    }

    private void UpdateRenderer()
    {
        if (currentInspectedObject != null && objectRenderer.enabled)
        {
            Transform rendererTransform = objectRenderer.transform;
            rendererTransform.localPosition += (playerController.cameraController.attachedCamera.transform.forward *
                                                (playerController.currentBrain.MouseWheel * Time.deltaTime));
            rendererTransform.localPosition = new Vector3(0f, 0f,
                Mathf.Clamp(objectRenderer.transform.localPosition.z, distances.x, distances.y));

            UpdateRotation(rendererTransform);
        }
    }

    private void UpdateRotation(Transform rendererTransform)
    {
        float xRot = playerController.currentBrain.MouseInput.x * rotationSpeed.x * Mathf.Deg2Rad;
        float yRot = playerController.currentBrain.MouseInput.y * rotationSpeed.y * Mathf.Deg2Rad;

        rendererTransform.RotateAround(playerController.cameraController.attachedCamera.transform.up, -xRot);
        rendererTransform.RotateAround(playerController.cameraController.attachedCamera.transform.right, yRot);
    }

    public bool Activate(IInteractable interactable)
    {
        if (interactable == null) return false;
        if (GetEnabled())
        {
            StopInspect();
            currentInspectedObject = null;
            return true;
        }

        try
        {
            var inspectable = interactable.attachedGameobject.GetComponent<IInspectable>();

            StartInspect(inspectable);
        }
        catch (NullReferenceException)
        {
        }

        return false;
    }

    public bool StartInspect(IInspectable inspectable)
    {
        try
        {
            var info = inspectable.Inspect();
            objectRenderer.SetComponents(info.objectMesh, info.objectTexture, info.objectTransform);
            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }

    public void StopInspect()
    {
        currentInspectedObject.StopInspect();
        currentInspectedObject = null;
        objectRenderer.ResetComponents();
        // Debug.Log("Stopped inspecting.");
    }

    public bool GetEnabled()
    {
        return currentInspectedObject != null;
    }
}