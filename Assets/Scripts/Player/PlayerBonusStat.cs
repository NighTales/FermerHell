using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBonusStat
{
    public readonly Dictionary<BonusType, Bonus> bonusPack = new Dictionary<BonusType, Bonus>();
    public readonly Dictionary<BonusType, Bonus> debuffPack = new Dictionary<BonusType, Bonus>();
    public int scoreMultiplicator = 1;
    public float BuffTime = 10f;
    public float DebuffTimt = 8f;
    public static PlayerBonusStat Instant
    {
        get
        {
            if (instant == null)
                instant = new PlayerBonusStat();
            return instant;
        }
    }
    private static PlayerBonusStat instant;
    private PlayerBonusStat() { Init(); }

    public class Bonus
    {
        public int value { get; set; }
        public float time { get; set; }

        public Bonus(int value, float time)
        {
            this.value = value;
            this.time = time;
        }
    }
    public void Init()
    {
        bonusPack.Add(BonusType.Jump, new Bonus(0, 0));
        bonusPack.Add(BonusType.Speed, new Bonus(0, 0));
        bonusPack.Add(BonusType.DOT, new Bonus(0, 0));
        bonusPack.Add(BonusType.Resist, new Bonus(0, 0));
        bonusPack.Add(BonusType.Magnet, new Bonus(0, 0));

        debuffPack.Add(BonusType.Jump, new Bonus(0, 0));
        debuffPack.Add(BonusType.Speed, new Bonus(0, 0));
        debuffPack.Add(BonusType.DOT, new Bonus(0, 0));
        debuffPack.Add(BonusType.Resist, new Bonus(0, 0));
        debuffPack.Add(BonusType.Magnet, new Bonus(0, 0));
    }
}
public enum BonusType
{
    Speed, //увеличенная скорость
    Jump, //усиленный прыжок
    DOT, // Damage Over Time
    Resist, // Сопротивление
    Magnet, // магнит плюшек

    Damage, // урон
    FireRate, //скорострельность
    Area, //площадь
}
