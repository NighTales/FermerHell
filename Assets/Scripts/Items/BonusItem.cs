using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusItem : GameItem //разные бонусы
{
    public BonusType type;
    public int value;
    public override void Action()
    {
        Messenger<int>.Broadcast("TAKE_BONUS_" + type.ToString().ToUpper(), value);
    }
}

