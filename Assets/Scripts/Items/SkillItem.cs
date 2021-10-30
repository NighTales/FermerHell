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
        }
    }

    public override void SetTarget(Transform target)
    {
        if (target.TryGetComponent(out PlayerMagic playerMagic))
        {
            if (playerMagic.rGBCharge.ColorCount < 3)
            {
                this.target = target;
            }
        }
    }
}