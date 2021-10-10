using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ExplosionZone : MonoBehaviour
{
    [Range(1,100)] public int damage = 10;
    [Range(1, 100)] public float force = 10;
   

    public void ChangeRange(float range, float damage)
    {
        GetComponent<SphereCollider>().radius = range;
        this.damage = (int)Mathf.Round( damage);
    }
}
