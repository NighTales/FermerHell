using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScareCrow : HellEnemy
{
    [SerializeField, Tooltip("Дальность прыжка"), Range(1, 100)] protected float jumpDistance = 15;



    protected override void Update()
    {
        
        if ((target.transform.position - transform.position).magnitude < jumpDistance+1 && (target.transform.position - transform.position).magnitude > jumpDistance-1)
        {
            anim.SetBool("Jump",true);
        }
        base.Update();
        
    }
    
    public override  IEnumerator SpecialMove()
    {
        transform.position = target.transform.position + Vector3.back*1;
        transform.rotation = target.transform.rotation;
        anim.SetBool("Jump",false);
        yield return new WaitForSeconds(0);

    }
}
