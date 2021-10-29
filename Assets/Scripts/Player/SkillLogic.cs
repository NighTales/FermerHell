using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLogic : MonoBehaviour
{
    [SerializeField]private CapsuleCollider triggerCollider;
    [SerializeField, Range(0,5f)]private float delayTime = 1;
    [SerializeField,Range(0,30f)]private float durationTime = 10;
    [SerializeField] private bool isBuff = false;



    public void Init(float Range)
    {
        //если триггер будет дочерним объектом
        if (!isBuff)
        {
            
        triggerCollider.radius  =Range;
        triggerCollider.gameObject.SetActive(false);
        StartCoroutine(SpecialSkillMove());
        StartCoroutine(DeadSkill());
        
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
        triggerCollider.radius  =0;
        Destroy(this, 0.1f);
    }
}
