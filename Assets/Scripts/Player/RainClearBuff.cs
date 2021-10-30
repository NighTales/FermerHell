using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainClearBuff : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Transform target = GameObject.FindGameObjectWithTag("Player").transform;
        if( target.TryGetComponent(out PlayerBonusScript playerBonus))
        {
            playerBonus.ActiveDebuff(BonusType.Jump, 0);
            playerBonus.ActiveDebuff(BonusType.DOT, 0);
            playerBonus.ActiveDebuff(BonusType.Speed, 0);
            playerBonus.ActiveDebuff(BonusType.Resist, 0);
            playerBonus.ActiveDebuff(BonusType.Magnet, 0);
        }
    }

   
}
