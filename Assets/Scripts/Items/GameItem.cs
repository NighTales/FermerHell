﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class GameItem : MonoBehaviour
{
    [SerializeField, Tooltip("Этот объект будет вращаться")] private Transform itemObject;
    [SerializeField, Range(0,50)] private float rotationSpeed = 1;
    [SerializeField, Range(1, 100), Tooltip("Скорость приближения к игроку, когда собрали")] private float moveSpeed = 1;
    [SerializeField, Tooltip("Падать на землю")] protected bool physics;
    [SerializeField, Tooltip("Удалятся со временем")] private bool deleted = true;
    [SerializeField, Range(1, 100), Tooltip("расстояние от которого начинается зона магнетизма")] private float magnetDistance = 1;

    [HideInInspector] public Transform target;

    private const float deletedTime = 120; //все собираемые объекты будут удаляться через равное время

    private void Start()
    {
        if(deleted)
        {
            Destroy(gameObject, deletedTime);
        }
    }

    void Update()
    {
        if(target == null)
            ItemRotate();
        else
            ItemMove();
    }
    
    private void ItemRotate()
    {
        itemObject.transform.Rotate(transform.up, rotationSpeed * Time.deltaTime);
    }
    private void ItemMove()
    {
        Vector3 dir = (target.position + Vector3.up) - itemObject.position;
        if (dir.magnitude > 2 * moveSpeed * Time.deltaTime && dir.magnitude<magnetDistance )
        {
            itemObject.position += dir.normalized * moveSpeed * Time.deltaTime;
        }
        else if (dir.magnitude > magnetDistance)
        {
            int num = PlayerBonusStat.bonusPack[BonusType.Magnet] - PlayerBonusStat.debuffPack[BonusType.Magnet];

            itemObject.position += dir.normalized * moveSpeed * Time.deltaTime*num;
        }
        else
        {
            Action();
            Destroy(gameObject);
        }
    }

    
    public virtual void SetTarget(Transform target)
    {
        if(physics)
        {
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<BoxCollider>());
        }
        this.target = target;
    } 
    public virtual void UnSetTarget()
    {
        if(physics)
        {
            this.gameObject.AddComponent<Rigidbody>();
            this.gameObject.AddComponent<BoxCollider>();
        }
        this.target = null;
    }
    /// <summary>
    /// Эффект от собираемого предмета
    /// </summary>
    public abstract void Action();
}
