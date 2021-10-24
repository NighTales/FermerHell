using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public float time = 5f;
    public BonusType typeBuff;

    public int buffvalue = 2;

    public int buffed = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.buffeeds[typeBuff]++;
            if (enemy.buffeeds[typeBuff] == 1)
            {
                enemy.StartBuff(typeBuff, buffvalue);
            }
        } 
    }
        


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Slow"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.buffeeds[typeBuff]--;
            if (enemy.buffeeds[typeBuff] == 0)
            {
                enemy.StartBuff(typeBuff, buffvalue);
            }
        }
    }
    // public Buff() //?&??
    // {
    // }
    //
    // void Start()
    // {
    //    // stateBuff = true;
    //    //StartCoroutine(Timer());
    // }
    //
    //
    // public IEnumerator Timer() //
    // {
    //     while (stateBuff)
    //     {
    //         ActiveBuff(true);
    //         yield return new WaitForSeconds(time);
    //     }
    //
    //     ActiveBuff(false);
    // }
    //
    // protected virtual void ActiveBuff(bool active) //активатор/деактиватор баффа
    // {
    //     
    // }
}