using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusItem : GameItem //разные бонусы
{
    public BonusType type;
    public int value;
    PlayerBonusStat playerStat = PlayerBonusStat.Instant;
    public override void Action()
    {
        if(target.TryGetComponent(out PlayerBonusScript playerBonus))
        {
            playerBonus.ActiveBuff(type, value);
        }
        //Messenger<int>.Broadcast("TAKE_BONUS_" + type.ToString().ToUpper(), value);
    }
}

