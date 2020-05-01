using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using World.Objects;

public class ObjectTracker : MonoBehaviour
{
    public static List<DynamicObject> doorList = new List<DynamicObject>();

    public static List<DynamicObject> drawerList = new List<DynamicObject>();

    // Start is called before the first frame update
    void Start()
    {
        PrepareLists(FindObjectsOfType<DynamicObject>());
    }

    private void PrepareLists(DynamicObject[] dynamicObjects)
    {
        foreach (DynamicObject element in dynamicObjects)
        {
            if (element == null) return;
            
            if (element.objectType == DynamicObject.ObjectType.Door)
            {
                doorList.Add(element);
            }
            else
            {
                drawerList.Add(element);
            }
        }
    }
}
