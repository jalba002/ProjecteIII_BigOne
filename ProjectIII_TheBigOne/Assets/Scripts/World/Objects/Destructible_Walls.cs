using System.Security.Cryptography;
using UnityEngine;

public class Destructible_Walls : MonoBehaviour
{
    // TODO Load from resources at runtime or prefab.
    public GameObject destroyedVersion;
    public Transform explosionOrigin;
    public GameObject mesh;
    

    private ExplodeScriptSandbox instantiatedVersion;

    public void Activate()
    {
        var instantiatedWall = Instantiate(destroyedVersion, destroyedVersion.transform.position, destroyedVersion.transform.rotation);
        instantiatedVersion = instantiatedWall.GetComponent<ExplodeScriptSandbox>();
        instantiatedVersion.Explode(explosionOrigin);
        CallRemovePieces();
    }

    public void CallRemovePieces()
    {
        instantiatedVersion.RemovePieces();
        Destroy(this.gameObject);
    }

}
