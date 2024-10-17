using System.Collections;
using Tavaris.Dynamic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(OffMeshLink))]
[RequireComponent(typeof(Door))]
public class TraversableBlockage : MonoBehaviour
{
    [Header("Settings")] 
    public float removalTime;
 
    [Space(10)][Header("Components")] public OffMeshLink attachedLink;
    public Door attachedDynamicObject;

    private Coroutine coroutineHolder;

    void Start()
    {
        if (attachedLink == null)
            attachedLink = GetComponent<OffMeshLink>();
        if (attachedDynamicObject == null)
            attachedDynamicObject = GetComponent<Door>();

        attachedDynamicObject.OnStartInteraction += UpdateLink;
    }

    private void UpdateLink()
    {
        attachedLink.activated = attachedDynamicObject.HingeJoint.angle > 15f;
    }

    public void DisableLink(float disableTime = 0.5f)
    {
        if (coroutineHolder != null)
            StopCoroutine(coroutineHolder);
        coroutineHolder = StartCoroutine(TempDisable(disableTime));
    }

    IEnumerator TempDisable(float disableTime)
    {
        attachedLink.activated = false;
        yield return new WaitForSeconds(disableTime);
        attachedLink.activated = true;
    }
}