using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TraversableWall : MonoBehaviour
{
    [Header("Settings")] public float removalTime;

    [Space(10)] [Header("Components")] public OffMeshLink attachedLink;

    void Start()
    {
        if (attachedLink == null)
            attachedLink = GetComponent<OffMeshLink>();
    }
}
