using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost:HellEnemy
{
    private Debuff _debuff;
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
       mother.GhostDeadReactor();
       Death();
    }
    
}
