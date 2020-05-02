using UnityEngine;

public class EnemyTargetDummy : MonoBehaviour
{
    public Transform GetParent()
    {
        return this.gameObject.transform.parent;
    }
}
