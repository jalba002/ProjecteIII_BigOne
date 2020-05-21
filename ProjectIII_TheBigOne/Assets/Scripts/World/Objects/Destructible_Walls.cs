using UnityEngine;

public class Destructible_Walls : MonoBehaviour
{
    // TODO Load from resources at runtime or prefab.
    public GameObject destroyedVersion;
    public Transform explosionOrigin;

    private ExplodeScriptSandbox instantiatedVersion;

    public void Activate()
    {
        var instantiatedWall = Instantiate(destroyedVersion, transform.position, transform.rotation);
        instantiatedVersion = instantiatedWall.GetComponent<ExplodeScriptSandbox>();
        instantiatedVersion.Explode(explosionOrigin);
        Destroy(gameObject);
        CallRemovePieces();
    }

    public void CallRemovePieces()
    {
        instantiatedVersion.RemovePieces();
    }

}
