using System.Collections.Generic;
using UnityEngine;
using World.Objects;

public class ObjectTracker : MonoBehaviour
{
    public static List<DynamicObject> doorList = new List<DynamicObject>();

    public static List<DynamicObject> drawerList = new List<DynamicObject>();

    public static List<TraversableBlockage> palletList;

    public static List<InteractableObject> interactablesList;

    // Start is called before the first frame update
    void Start()
    {
        PrepareLists(FindObjectsOfType<DynamicObject>());
        PrepareObjectList(out palletList, FindObjectsOfType<TraversableBlockage>());
        PrepareObjectList(out interactablesList);
    }

    private void PrepareObjectList(out List<TraversableBlockage> storedList, TraversableBlockage[] givenArray)
    {
        storedList = new List<TraversableBlockage>();
        foreach(TraversableBlockage currentObject in givenArray)
        {
            storedList.Add(currentObject);
        }
    }
    
    private void PrepareObjectList(out List<InteractableObject> storedList)
    {
        storedList = new List<InteractableObject>();
        var givenArray = FindObjectsOfType<InteractableObject>();

        foreach(InteractableObject currentObject in givenArray)
        {
            storedList.Add(currentObject);
        }
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
            else if (element.objectType == DynamicObject.ObjectType.Drawer)
            {
                drawerList.Add(element);
            }
            
        }
        
    }
}
