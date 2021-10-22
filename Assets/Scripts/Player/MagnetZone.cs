using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class MagnetZone : MonoBehaviour
{
    
    [SerializeField, Range(1, 100), Tooltip("радиус триггера")] private float magnetMAxDistance = 1;
    private CapsuleCollider collider;

    private void Start()
    {
        collider = GetComponent<CapsuleCollider>();
        collider.radius = magnetMAxDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Item"))
        {
            other.GetComponent<GameItem>().SetTarget(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag.Equals("Item"))
        {
            other.GetComponent<GameItem>().UnSetTarget();
        }
    }
}
