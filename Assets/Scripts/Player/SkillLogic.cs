using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SkillLogic : MonoBehaviour
{
    [SerializeField] private SphereCollider triggerCollider;
    [SerializeField, Range(0, 5f)] private float delayTime = 1;
    [SerializeField, Range(0, 30f)] private float durationTime = 10;
    [SerializeField] private bool isBuff = false;
    [SerializeField] private bool isNeedToRestart = false;
    [SerializeField] private BonusType Buff;
    [SerializeField, Range(0, 50)] private int buffValue = 40;
    


    public void Init(float Range, Transform target)
    {
        //если триггер будет дочерним объектом

        if (triggerCollider!= null)
        {
            triggerCollider.radius = Range;
            triggerCollider.gameObject.SetActive(false); 
            StartCoroutine(SpecialSkillMove());  
        }
        StartCoroutine(DeadSkill());
        if (isNeedToRestart)
        {
            StartCoroutine(ReSkillEffect());
        }

        if (isBuff)
        {
            if (target.TryGetComponent(out PlayerBonusScript playerBonus))
            {
                playerBonus.ActiveBuff(Buff, buffValue);
            }
        }
    }

    private IEnumerator ReSkillEffect()
    {
        VisualEffect eff = GetComponent<VisualEffect>();
        //здесь должны запускаться все эффекты
        while (true)
        {
            yield return new WaitForSeconds(2.5f);
            eff.Reinit();
        }
    }


    private IEnumerator SpecialSkillMove()
    {
        //здесь должны запускаться все эффекты
        yield return new WaitForSeconds(delayTime);
        triggerCollider.gameObject.SetActive(true);
    }

    private IEnumerator DeadSkill()
    {
        yield return new WaitForSeconds(durationTime);
        if (triggerCollider != null)
        {
            triggerCollider.radius = 0;
        }
        Destroy(this.gameObject, 0.1f);
    }
}