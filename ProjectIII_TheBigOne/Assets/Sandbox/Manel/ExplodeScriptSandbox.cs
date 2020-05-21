using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class ExplodeScriptSandbox : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Explosion Settings")]
    public float minForce;
    public float maxForce;
    public float radius;

    [Header("Settings")] public float removingTime = 3f;

    private Rigidbody[] brokenPieces;

    private void Start()
    {
        brokenPieces = GetComponentsInChildren<Rigidbody>();
    }

    public void Explode(Transform explosionOrigin)
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(true);
            Rigidbody rb = t.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.mass = 5f;
                rb.AddExplosionForce(Random.Range(minForce, maxForce), explosionOrigin.position, radius);
            }
        }

        StartCoroutine(RemovePieces());
    }

    public IEnumerator RemovePieces()
    {
        yield return new WaitForSeconds(removingTime);
        foreach (Rigidbody rb in brokenPieces)
        {
            Destroy(rb.gameObject.GetComponent<Collider>());
            rb.gameObject.isStatic = true;
            Destroy(rb);
        }
        //Destroy(this.gameObject);
    }
}