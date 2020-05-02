using UnityEngine;

public class Destructible_Walls : MonoBehaviour
{
    // TODO Load from resources at runtime or prefab.
    public GameObject destroyedVersion;
    public bool cheatMode = false;

    private void Update()
    {
        #if UNITY_EDITOR
        
            if (Input.GetKeyDown(KeyCode.Z))
            {
                ChangeTheWall();
            }
        
        #endif
    }


    public void ChangeTheWall()
    {
        Instantiate(destroyedVersion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}
