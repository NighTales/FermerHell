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
            if (enemy.slowDebuffed == 0)
            {
                enemy.StartBuff(typeBuff, buffvalue);
            }
            enemy.buffeeds[typeBuff]++;
        } 
    }
        


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Slow"))
        {
            other.GetComponent<Enemy>().buffeeds[typeBuff]++;
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