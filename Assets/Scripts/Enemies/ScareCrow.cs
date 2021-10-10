using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScareCrow : HellEnemy
{
    [SerializeField, Tooltip("Дальность прыжка"), Range(1, 100)] protected float jumpDistance = 15;
    [SerializeField, Tooltip("Задержка перед телепортацией"), Range(1, 100)] protected float jumpDelay = 5;


    public void SpecialMove()
    {
        transform.position = target.transform.position + Vector3.back;

    }
    protected override void Update()
    {
        
        if ((target.transform.position - transform.position).magnitude < jumpDistance+1 && (target.transform.position - transform.position).magnitude > jumpDistance+1)
        {
            //anim.SetBool("Jump",true);
        }
        base.Update();
        
    }
}
