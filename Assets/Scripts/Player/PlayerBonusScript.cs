using System;
using System.Collections;
using UnityEngine;

public class PlayerBonusScript : MonoBehaviour
{
    [SerializeField]
    PlayerBonusStat playerBonus = PlayerBonusStat.Instant;
    // Use this for initialization
    void Start()
    {
        Messenger<int>.AddListener(GameEvent.TAKE_BONUS_JUMP, new Action<int>(c => ActiveBuff(BonusType.Jump, c)));
        Messenger<int>.AddListener(GameEvent.TAKE_BONUS_SPEED, new Action<int>(c => ActiveBuff(BonusType.Speed, c)));
        Messenger<int>.AddListener(GameEvent.TAKE_BONUS_RESIST, new Action<int>(c => ActiveBuff(BonusType.Resist, c)));
        Messenger<int>.AddListener(GameEvent.TAKE_BONUS_DOT, new Action<int>(c => ActiveBuff(BonusType.DOT, c)));
        Messenger<int>.AddListener(GameEvent.TAKE_BONUS_MUGNET, new Action<int>(c => ActiveBuff(BonusType.Magnet, c)));


        Messenger<int>.AddListener(GameEvent.TAKE_DEBUFF_JUMP, new Action<int>(c => ActiveDebuff(BonusType.Jump, c)));
        Messenger<int>.AddListener(GameEvent.TAKE_DEBUFF_SPEED, new Action<int>(c => ActiveDebuff(BonusType.Speed, c)));
        Messenger<int>.AddListener(GameEvent.TAKE_DEBUFF_RESIST, new Action<int>(c => ActiveDebuff(BonusType.Resist, c)));
        Messenger<int>.AddListener(GameEvent.TAKE_DEBUFF_DOT, new Action<int>(c => ActiveDebuff(BonusType.DOT, c)));
        Messenger<int>.AddListener(GameEvent.TAKE_DEBUFF_MUGNET, new Action<int>(c => ActiveDebuff(BonusType.Magnet, c)));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActiveBuff(BonusType bonusType, int value)
    {
        if (playerBonus.bonusPack[bonusType].time <= 0)
        {
            StartCoroutine(DeactiveBuff(bonusType));
        }
        playerBonus.bonusPack[bonusType].value = value;
        playerBonus.bonusPack[bonusType].time += playerBonus.BuffTime;
    }

    public void ActiveDebuff(BonusType bonusType, int value)
    {
        if (playerBonus.bonusPack[bonusType].time <= 0)
        {
            StartCoroutine(DeactiveDebuff(bonusType));
        }
        playerBonus.debuffPack[bonusType].value = value;
        playerBonus.debuffPack[bonusType].time += playerBonus.BuffTime;
    }

    public IEnumerator DeactiveBuff(BonusType bonusType)
    {
        do
        {
            playerBonus.bonusPack[bonusType].time--;
            yield return new WaitForSeconds(1);
        } while (playerBonus.bonusPack[bonusType].time > 0);
        playerBonus.bonusPack[bonusType].value = 0;
    }
    public IEnumerator DeactiveDebuff(BonusType bonusType)
    {
        do
        {
            playerBonus.debuffPack[bonusType].time--;
            yield return new WaitForSeconds(1);
        } while (playerBonus.bonusPack[bonusType].time > 0);
        playerBonus.debuffPack[bonusType].value = 0;
    }
}
