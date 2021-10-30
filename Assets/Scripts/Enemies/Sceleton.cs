using System;
using System.Collections;
using System.Collections.Generic;
using Al_AI.Scripts;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Sceleton : HellEnemy
{
    [Range(1, 500)] public int suicideHealth = 50;

    //suicide parameters
    protected bool suicideKey;

    [SerializeField, Range(1, 20), Tooltip("Скорость бега при суицидной атаке")]
    protected float suicidespeed = 20;

    [SerializeField, Tooltip("Таймер для взрыва"), Range(1, 10)]
    protected float suicideTimer = 5;

   [SerializeField] protected float elapsedTimeAfterSuicideMode = 0f;


    public override void GetDamage(int damage) //че за хуйня блять? 
    {
        Messenger.Broadcast(GameEvent.HIT);
        if(Health > 0)
        {
            Health -= damage;
            
            
            
            if (Health > suicideHealth)
            {
                anim.SetInteger("Damage", damage);
                anim.SetTrigger("GetDamage");
            }
            else
            {
                
                //false false
                //true false
                //false true
                //true true
                if (!suicideKey && !anim.GetBool("WithoutLegs")) //полузатратно
                {
                    anim.SetBool("WithoutLegs", true);
                 

                    if (target != null)
                    {
                        partController.FirstPartImpulse(target.transform.position + Vector3.up * 1.8f);
                    }
                }
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        if (suicideKey)
        {
            elapsedTimeAfterSuicideMode += Time.deltaTime;
        }
        
    }
    #region StateMethods

    public override void Attack()
    {
        base.Attack();
        if (suicideKey)
        {
            elapsedTimeAfterSuicideMode = 5;
            Death();
        }
    }

    #endregion

    public override void Death()
    {
        afterDeadEvent?.Invoke();
        // Messenger<int>.Broadcast(GameEvent.ENEMY_HIT, scoreForWin);
        LootSpawn();

        // Messenger.Broadcast(GameEvent.ENEMY_DEAD);
        if (suicideKey && elapsedTimeAfterSuicideMode >= 1)
        {
            postDeadDecal.tag = "AllFire";
            GameObject deadDecal = Instantiate(postDeadDecal, transform.position, Quaternion.identity);
            deadDecal.GetComponent<Decal>().Init(1);
            deadDecal.GetComponent<ExplosionZone>().ChangeRange(elapsedTimeAfterSuicideMode * attackDistance,
                damage * elapsedTimeAfterSuicideMode);
            var mainModule = deadDecal.GetComponent<ParticleSystem>().main;
            mainModule.startSize = new ParticleSystem.MinMaxCurve(elapsedTimeAfterSuicideMode * attackDistance,
                elapsedTimeAfterSuicideMode * attackDistance);
        }

        Messenger<int>.Broadcast(GameEvent.ENEMY_HIT, 30);
        Destroy(gameObject);
    }

    #region Special

    public override IEnumerator SpecialMove()
    {
        suicideKey = true;
        
        basespeed = suicidespeed;
        speed = suicidespeed;
        OnTakeDebuffSpeed(slowvalue);
        yield return new WaitForSeconds(suicideTimer);
        Death(); //Health-=100;
    }

    #endregion
}