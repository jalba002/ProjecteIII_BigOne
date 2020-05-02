using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible_Walls : MonoBehaviour
{
    public GameObject destroyedVersion;

    public float explosionForce;
    public Vector3 positionToSpawnExplosion;
    public float radius;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            
            Instantiate(destroyedVersion, transform.position, transform.rotation);
            Destroy(gameObject);           
           
        }
    }

}
