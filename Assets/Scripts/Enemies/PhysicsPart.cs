using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PhysicsPart : MonoBehaviour
{
    private const int DeleteTime = 5;

    private Rigidbody rb;
    private Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
        col.enabled = false;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    public void ImpulseToPart(Vector3 direction)
    {
        col.enabled = true;
        transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(direction * 10, ForceMode.Impulse);
        Destroy(gameObject, DeleteTime);
    }
}
