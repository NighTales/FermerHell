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
    //public float BuffTime = 10f;
    //public float DebuffTimt = 8f;
    public Sprite ActiveSlillSprite;
    public static PlayerBonusStat Instant
    {
        get
        {
            if (instant == null)
            {
                instant = new PlayerBonusStat();
                instant.Init();
            }
            return instant;
        }
    }

    private static PlayerBonusStat instant;
    private PlayerBonusStat() {  }

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
    public void ReCreate()
    {
        bonusPack.Clear();
        bonusPack.Add(BonusType.Jump, new Bonus(0, 0));
        bonusPack.Add(BonusType.Speed, new Bonus(0, 0));
        bonusPack.Add(BonusType.DOT, new Bonus(0, 0));
        bonusPack.Add(BonusType.Resist, new Bonus(0, 0));
        bonusPack.Add(BonusType.Magnet, new Bonus(0, 0));

        debuffPack.Clear();
        debuffPack.Add(BonusType.Jump, new Bonus(0, 0));
        debuffPack.Add(BonusType.Speed, new Bonus(0, 0));
        debuffPack.Add(BonusType.DOT, new Bonus(0, 0));
        debuffPack.Add(BonusType.Resist, new Bonus(0, 0));
        debuffPack.Add(BonusType.Magnet, new Bonus(0, 0));


        bonusPack.Add(BonusType.Damage, new Bonus(0, 0));
        bonusPack.Add(BonusType.Area, new Bonus(0, 0));
        bonusPack.Add(BonusType.FireRate, new Bonus(0, 0));
    }
    private void Init()
    {
        ReCreate();

        Messenger<int>.AddListener(GameEvent.SET_R_BONUS, SetRBonus);
        Messenger<int>.AddListener(GameEvent.SET_G_BONUS, SetGBonus);
        Messenger<int>.AddListener(GameEvent.SET_B_BONUS, SetBBonus);
    }
    public void SetRBonus(int value) => SetWeaponBuff(BonusType.Damage, value);
    public void SetGBonus(int value) => SetWeaponBuff(BonusType.FireRate, value);
    public void SetBBonus(int value) => SetWeaponBuff(BonusType.Area, value);

    public void SetWeaponBuff(BonusType type, int value)
    {
        bonusPack[type].value = value;
        Debug.Log("" + type + "  " + value);
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
