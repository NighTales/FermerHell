using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : HellEnemy
{
    private Debuff _debuff;
    public void Init(GameObject target, Debuff debuff)
    {
        this.target = target;
        agent.destination = target.transform.position;
        agent.isStopped = true;
       // suicideKey = true;//?
        _debuff = debuff;
    }
}
