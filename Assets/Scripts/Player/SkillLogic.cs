using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLogic : MonoBehaviour
{
    [SerializeField]private Collider TriggerCollider;
    private float delayTime;
    private float durationTime;


    public void Init(float Range)
    {
        //если триггер будет дочерним объектом
        TriggerCollider.gameObject.transform.localScale  =new Vector3( Range,1,Range);
        StartCoroutine(SpecialSkillMove());
        StartCoroutine(DeadSkill());
        
    }

    private IEnumerator SpecialSkillMove()
    {
        //здесь должны запускаться все эффекты
        yield return new WaitForSeconds(delayTime);
        TriggerCollider.isTrigger = true;
    }
    private IEnumerator DeadSkill()
    {
        yield return new WaitForSeconds(durationTime);
    }
}
