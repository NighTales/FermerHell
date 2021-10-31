using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBonusScript : MonoBehaviour
{
    [SerializeField]
    private PlayerBonusStat playerBonus = PlayerBonusStat.Instant;

    [Range(0f, 60f)] public float BuffTime = 10f;
    [Range(0f, 60f)] public float DebuffTime = 8f;
    //private List<Action<int>> actions;
    // Use this for initialization
    //void Awake()
    //{
    //    actions = new List<Action<int>>();

    //    actions.Add(new Action<int>(c => ActiveBuff(BonusType.Jump, c)));
    //    actions.Add(new Action<int>(c => ActiveBuff(BonusType.Speed, c)));
    //    actions.Add(new Action<int>(c => ActiveBuff(BonusType.Resist, c)));
    //    actions.Add(new Action<int>(c => ActiveBuff(BonusType.DOT, c)));
    //    actions.Add(new Action<int>(c => ActiveBuff(BonusType.Magnet, c)));

    //    actions.Add(new Action<int>(c => ActiveDebuff(BonusType.Jump, c)));
    //    actions.Add(new Action<int>(c => ActiveDebuff(BonusType.Speed, c)));
    //    actions.Add(new Action<int>(c => ActiveDebuff(BonusType.Resist, c)));
    //    actions.Add(new Action<int>(c => ActiveDebuff(BonusType.DOT, c)));
    //    actions.Add(new Action<int>(c => ActiveDebuff(BonusType.Magnet, c)));

    //    Messenger<int>.AddListener(GameEvent.Set_BONUS_JUMP, actions[0]);
    //    Messenger<int>.AddListener(GameEvent.Set_BONUS_SPEED, actions[1]);
    //    Messenger<int>.AddListener(GameEvent.Set_BONUS_RESIST, actions[2]);
    //    Messenger<int>.AddListener(GameEvent.Set_BONUS_DOT, actions[3]);
    //    Messenger<int>.AddListener(GameEvent.Set_BONUS_MUGNET, actions[4]);


    //    Messenger<int>.AddListener(GameEvent.Set_DEBUFF_JUMP, actions[5]);
    //    Messenger<int>.AddListener(GameEvent.Set_DEBUFF_SPEED, actions[6]);
    //    Messenger<int>.AddListener(GameEvent.Set_DEBUFF_RESIST, actions[7]);
    //    Messenger<int>.AddListener(GameEvent.Set_DEBUFF_DOT, actions[8]);
    //    Messenger<int>.AddListener(GameEvent.Set_DEBUFF_MUGNET, actions[9]);
    //}

    private void OnDestroy()
    {
        StopAllCoroutines();

        playerBonus.ReCreate();
        //Messenger<int>.RemoveListener(GameEvent.Set_BONUS_JUMP, actions[0]);
        //Messenger<int>.RemoveListener(GameEvent.Set_BONUS_SPEED, actions[1]);
        //Messenger<int>.RemoveListener(GameEvent.Set_BONUS_RESIST, actions[2]);
        //Messenger<int>.RemoveListener(GameEvent.Set_BONUS_DOT, actions[3]);
        //Messenger<int>.RemoveListener(GameEvent.Set_BONUS_MUGNET, actions[4]);


        //Messenger<int>.RemoveListener(GameEvent.Set_DEBUFF_JUMP, actions[5]);
        //Messenger<int>.RemoveListener(GameEvent.Set_DEBUFF_SPEED, actions[6]);
        //Messenger<int>.RemoveListener(GameEvent.Set_DEBUFF_RESIST, actions[7]);
        //Messenger<int>.RemoveListener(GameEvent.Set_DEBUFF_DOT, actions[8]);
        //Messenger<int>.RemoveListener(GameEvent.Set_DEBUFF_MUGNET, actions[9]);
    }
    // Update is called once per frame
    void Update()
    {
    }
    //[ContextMenu(nameof(ApplyEffectTime))]
    //public void ApplyEffectTime()
    //{
    //    playerBonus.BuffTime = BuffTime;
    //    playerBonus.DebuffTimt = DebuffTimt;

    //}

    public void ActiveBuff(BonusType bonusType, int value)
    {
        playerBonus.bonusPack[bonusType].time += BuffTime;
        if (playerBonus.bonusPack[bonusType].value <= 0)
        {
            StartCoroutine(DeactiveBuff(bonusType));
        }
        playerBonus.bonusPack[bonusType].value = value;
        Messenger<int>.Broadcast("TAKE_BONUS_" + bonusType.ToString().ToUpper(), value);
        Debug.Log("ActiveBuff " + bonusType + "  Broadcast");
    }

    public void ActiveDebuff(BonusType bonusType, int value)
    {
        playerBonus.debuffPack[bonusType].time += DebuffTime;
        if (playerBonus.debuffPack[bonusType].value <= 0)    
        {
            StartCoroutine(DeactiveDebuff(bonusType));
        }
        playerBonus.debuffPack[bonusType].value = value;
        Messenger<int>.Broadcast("TAKE_DEBUFF_" + bonusType.ToString().ToUpper(), value);
        Debug.Log("ActiveDebuff " + bonusType + "  Broadcast");
    }

    public IEnumerator DeactiveBuff(BonusType bonusType)
    {
        do
        {
            yield return new WaitForSeconds(1);
            playerBonus.bonusPack[bonusType].time--;

        } while (playerBonus.bonusPack[bonusType].time > 0);
        playerBonus.bonusPack[bonusType].value = 0;
        Messenger<int>.Broadcast("TAKE_DEBUFF_" + bonusType.ToString().ToUpper(), 0);
        Debug.Log("DeactiveBuff " + bonusType + "  Broadcast");
    }
    public IEnumerator DeactiveDebuff(BonusType bonusType)
    {
        do
        {
            yield return new WaitForSeconds(1);
            playerBonus.debuffPack[bonusType].time--;

        } while (playerBonus.debuffPack[bonusType].time > 0);
        playerBonus.debuffPack[bonusType].value = 0;
        Messenger<int>.Broadcast("TAKE_DEBUFF_" + bonusType.ToString().ToUpper(), 0);
        Debug.Log("DeactiveDebuff " + bonusType + " Broadcast");
    }
}
