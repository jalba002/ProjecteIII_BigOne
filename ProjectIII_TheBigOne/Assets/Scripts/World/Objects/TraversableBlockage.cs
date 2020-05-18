using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using World.Objects;

public class TraversableBlockage : MonoBehaviour
{
    [Header("Settings")] public float removalTime;
    public bool doCheck = false;

    [Space(10)] [Header("Components")] public OffMeshLink attachedLink;
    public DynamicObject attachedDynamicObject;

    void Start()
    {
        if (attachedLink == null)
            attachedLink = GetComponent<OffMeshLink>();
        if (attachedDynamicObject == null)
            attachedDynamicObject = GetComponent<DynamicObject>();
    }

    void Update()
    {
        if (doCheck)
            attachedLink.activated = attachedDynamicObject.HingeJoint.angle > 15f;
    }
}