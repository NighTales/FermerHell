using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerBonusStat
{
    public static Dictionary<BonusType, int> bonusPack;
    public static Dictionary<BonusType, int> debuffPack;
    public static int scoreMultiplicator = 1;

    public static void Init()
    {
        scoreMultiplicator = 1;
        bonusPack = new Dictionary<BonusType, int>();
        bonusPack.Add(BonusType.Damage, 1);
        bonusPack.Add(BonusType.Invulnerable, 1);
        bonusPack.Add(BonusType.Jump, 1);
        bonusPack.Add(BonusType.Speed, 1);
        bonusPack.Add(BonusType.DOT, 1);
        bonusPack.Add(BonusType.Resist, 1);
         debuffPack = new Dictionary<BonusType, int>();
        // debuffPack.Add(BonusType.Damage, 1);
        // debuffPack.Add(BonusType.Invulnerable, 1);
        debuffPack.Add(BonusType.Jump, 1);
        debuffPack.Add(BonusType.Speed, 1);
        debuffPack.Add(BonusType.Resist, 1);
        debuffPack.Add(BonusType.DOT, 1);
    }
}
