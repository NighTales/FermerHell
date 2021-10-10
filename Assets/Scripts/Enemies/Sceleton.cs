using System;
using System.Collections;
using System.Collections.Generic;
using Al_AI.Scripts;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Sceleton : HellEnemy
{
    [Range(1,500)] public int suicideHealth = 50;

    //suicide parameters
    protected bool suicideKey;
    [SerializeField, Range(1, 20), Tooltip("Скорость бега при суицидной атаке")] protected float suicidespeed = 20;
    [SerializeField, Tooltip("Таймер для взрыва"), Range(1, 10)] protected float suicideTimer = 5;
    protected float elapsedTimeAfterSuicideMode = 0f;
    
    #region Initialization

    #endregion
    
    
    public override void GetDamage(int damage)
    {
        if(Health > 0)
        {
            Health -= damage;
            if (Health>suicideHealth)
            {
                
                anim.SetInteger("Damage",damage);
                anim.SetTrigger("GetDamage");
            }
            else
            {
                

                if (!anim.GetBool("WithoutLegs")) // затратно
                {

                    anim.SetBool("WithoutLegs",true);
                    //suicideKey = true; //Могу ли я это использовать?


                    if (target != null)
                    {
                        partController.FirstPartImpulse(target.transform.position + Vector3.up * 1.8f);
                    }
                }
            }
        }
        
    }
    

    #region StateMethods
    public override void Attack()
    {
        base.Attack();
        elapsedTimeAfterSuicideMode = 5;
        Death();
    }
    #endregion

    public override void Death()
    {
       // Messenger<int>.Broadcast(GameEvent.ENEMY_HIT, scoreForWin);
        int cycleCount = Random.Range(0, 3);

        if (cycleCount > lootPrefabs.Count)
            cycleCount = lootPrefabs.Count;
        if (target != null)
        {
            partController.SecondPartImpulse(transform.position );
        }
        while (cycleCount > 0)
        {
            int number = Random.Range(0, lootPrefabs.Count);
            Vector3 dir = new Vector3(Random.Range(-0.05f, 0.05f), 2, Random.Range(-0.05f, 0.05f));
            Instantiate(lootPrefabs[number], transform.position + dir, Quaternion.identity).GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
            lootPrefabs.Remove(lootPrefabs[number]);
            cycleCount--;
        }
        
       // Messenger.Broadcast(GameEvent.ENEMY_DEAD);
        if (suicideKey && elapsedTimeAfterSuicideMode>=1)
        {
            
            GameObject deadDecal = Instantiate(postDeadDecal, transform.position, Quaternion.identity);
            deadDecal.GetComponent<Decal>().Init(1);
            deadDecal.GetComponent<ExplosionZone>().ChangeRange(elapsedTimeAfterSuicideMode*attackDistance, damage*elapsedTimeAfterSuicideMode);


        }
        Destroy(gameObject);
    }

    #region Special
    public IEnumerator SpecialMove()
    {
        suicideKey = true;
        agent.speed = suicidespeed;
        yield return new WaitForSeconds(suicideTimer);
        Debug.Log("5 секунд прошло");
        Death();//Health-=100;
    }
    #endregion
    
}
