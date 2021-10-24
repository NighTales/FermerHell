using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItem : GameItem
{
    public Color color = Color.red;
    public override void Action()
    {
        if (target.TryGetComponent(out PlayerMagic  playerMagic))
        {
            playerMagic.AddColor(color);
            Debug.Log("SkillItem Action TryGetComponent");
        }
        Debug.Log("SkillItem Action Else");
        //Messenger<int>.Broadcast("TAKE_BONUS_" + type.ToString().ToUpper(), value);
    }
}