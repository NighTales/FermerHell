using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost:HellEnemy
{
    [SerializeField, Tooltip("Тип дебаффа")] private BonusType type;
    [SerializeField, Tooltip("модификатор дебаффа"),Range(1,1000)]private int value = 2;
    private Mourner mother;
    public void Init(GameObject target, Mourner mother)
    {
        this.mother = mother;
        this.target = target;
        if (agent==null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        agent.destination = target.transform.position;
        agent.isStopped = true;
       // suicideKey = true;//?
       // _debuff = debuff;
    }
    
    
    
    public override IEnumerator SpecialMove()
    {
        yield return new WaitForSeconds(0);
       // target.GetComponent<Buff>().Timer();
       //mother.GhostDeadReactor();
       Action();
       Death();
    }

    public override void Death()
    {
        anim.SetTrigger("Attack");

        mother.GhostDeadReactor();
        base.Death();
    }
    public void Action()
    {
        if(target.TryGetComponent(out PlayerBonusScript playerBonus))
        {
            playerBonus.ActiveDebuff(type, value);
        }
    }
    
}
