using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public float time = 5f;
    public bool stateBuff;

    public Buff() //?&??
    {
    }

    void Start()
    {
        stateBuff = true;
        StartCoroutine(Timer());
    }


    public IEnumerator Timer() //
    {
        while (stateBuff)
        {
            ActiveBuff(true);
            yield return new WaitForSeconds(time);
        }

        ActiveBuff(false);
    }

    protected virtual void ActiveBuff(bool active) //активатор/деактиватор баффа
    {
        
    }
}