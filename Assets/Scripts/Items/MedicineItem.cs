using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicineItem : GameItem //аптечка
{
    [SerializeField, Range(1,100)] private int hp = 25;

    public override void SetTarget(Transform target)
    {
        if(target.TryGetComponent(out AliveController aliveController))
        {
            if (aliveController.Health < aliveController.maxHealth)
                base.SetTarget(target);
        }
    }

    public override void Action()
    {
        if(target.TryGetComponent(out PlayerCharacter  playerCharacter))
        {
            playerCharacter.RestoreHealth(hp);
        }
    }
}
