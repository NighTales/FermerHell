using System;
using System.Collections;
using System.Collections.Generic;
using Al_AI.Scripts;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Sceleton : Enemy
{
    [SerializeField] private EnemyState state;
    [Range(1,500)] public int suicideHealth = 50;
    [SerializeField, Tooltip("Целевой объект")] private GameObject target;
    [SerializeField, Tooltip("Дальность атаки"), Range(1, 10)] private float attackDistance = 5;

    [SerializeField] private Animator anim;
    private NavMeshAgent agent;

    //suicide parameters
    private bool suicideKey;
    [SerializeField, Range(1, 20), Tooltip("Скорость бега при суицидной атаке")] private float suicidespeed = 20;
    [SerializeField, Tooltip("Таймер для взрыва"), Range(1, 10)] private float suicideTimer = 5;
    private float elapsedTimeAfterSuicideMode = 0f;

    private PhysicsPartController partController;
    
    
    
    
    #region Initialization

    public void Init(GameObject target)
    {
        this.target = target;
        agent.destination = target.transform.position;
        agent.isStopped = true;
    }

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        if (target != null)
            Init(target);
        else
        {
            target =  GameObject.FindGameObjectWithTag("Player");
            if (target != null)
                Init(target);
        }

        Born();
        // anim = GetComponent<Animator>();
        partController = GetComponent<PhysicsPartController>();
    }

    #endregion
    
    private void Update()
    {
        //Debug.Log(agent.isStopped);
        if (suicideKey)
        {
            elapsedTimeAfterSuicideMode += Time.deltaTime;
        }
        switch (state)
        {
            case EnemyState.MoveToTarget:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Born:
                agent.isStopped = true;
                StartCoroutine(Born());
                break;
            case EnemyState.Stun:
                agent.isStopped = true;
                break;
            default:
                break;
        }
        
    }
    
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

    private IEnumerator Born()
    {
        anim.SetBool("WithoutLegs",false);
        yield return new WaitForSeconds(1);
        state = EnemyState.MoveToTarget; // idlle
    }
    private void Move()
    {
        if(target != null)
        {
            //agent.isStopped = false;
            if ((target.transform.position - transform.position).magnitude <attackDistance)
            {
                state = EnemyState.Attack;
            }
            else
            {
               anim.SetBool("Walk", true);
               agent.destination = target.transform.position;
            }
        }
        else if(!agent.isStopped)
        {
            agent.isStopped = true;
        }
    }
    public void Attack()
    {
        agent.isStopped = true;
         if ((target.transform.position - transform.position).magnitude > attackDistance)
        {
            state = EnemyState.MoveToTarget;
        }else if (suicideKey)
         {
             anim.SetTrigger("Attack");
             elapsedTimeAfterSuicideMode = 5;
             Death();//Health-=100;
         }
         else if (!suicideKey)
         {
             anim.SetTrigger("Attack");
             //код для обычной атаки        
         }
    }
    

    public void Stun(bool stunned)
    {
        
        agent.isStopped = stunned;
    }
    #endregion
    public override int Health // скорее всего дополнять, либо же менять get damage
    {
        get => _health;
        set
        {
            _health = value;
            if (_health <= 0)
                Death();
            else if (_health > maxHealth)
                _health = maxHealth;
            else
            {
                

            }
        }
    }
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
