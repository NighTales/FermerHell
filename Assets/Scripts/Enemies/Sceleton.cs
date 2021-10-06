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

    [SerializeField, Tooltip("Целевой объект")] private GameObject target;
    [SerializeField, Tooltip("Дальность атаки"), Range(1, 10)] private float attackDistance = 5;

    // private Animator anim;
    private NavMeshAgent agent;

    //suicide parameters
    private bool suicideKey;
    [SerializeField, Range(1, 20), Tooltip("Скорость бега при суицидной атаке")] private float suicidespeed = 20;
    [SerializeField, Tooltip("Таймер для взрыва"), Range(1, 10)] private float suicideTimer = 5;
    private float elapsedTimeAfterSuicideMode = 0f;

    
    
    
    
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
        
    }

    #endregion
    
    private void Update()
    {
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

    #region StateMethods

    private IEnumerator Born()
    {
        yield return new WaitForSeconds(1);
        state = EnemyState.MoveToTarget;
    }
    private void Move()
    {
        if(target != null)
        {
            agent.isStopped = false;
            if ((target.transform.position - transform.position).magnitude <attackDistance)
            {
                state = EnemyState.Attack;
            }
            else
            {
               // anim.SetBool("Move", true);
                
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
        }
         else if (suicideKey)
         {
             elapsedTimeAfterSuicideMode = 5;
             Death();//Health-=100;
         }
         else if (!suicideKey)
         {
             //код для обычной атаки        
         }
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
            else if (_health < maxHealth / 4 && !suicideKey)
            {
                suicideKey = true;
                StartCoroutine(SpecialMove());
            }
        }
    }
    public override void Death()
    {
        Messenger<int>.Broadcast(GameEvent.ENEMY_HIT, scoreForWin);
        int cycleCount = Random.Range(0, 3);

        if (cycleCount > lootPrefabs.Count)
            cycleCount = lootPrefabs.Count;

        while (cycleCount > 0)
        {
            int number = Random.Range(0, lootPrefabs.Count);
            Vector3 dir = new Vector3(Random.Range(-0.05f, 0.05f), 2, Random.Range(-0.05f, 0.05f));
            Instantiate(lootPrefabs[number], transform.position + dir, Quaternion.identity).GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
            lootPrefabs.Remove(lootPrefabs[number]);
            cycleCount--;
        }
        Instantiate(postDeadDecal, transform.position, Quaternion.identity).GetComponent<Decal>().Init(2);
        Messenger.Broadcast(GameEvent.ENEMY_DEAD);
        if (suicideKey && elapsedTimeAfterSuicideMode>=1)
        {
            Explosion(1*elapsedTimeAfterSuicideMode,damage*elapsedTimeAfterSuicideMode);
        }
        Destroy(gameObject);
    }

    #region Special
    public void Explosion(float range, float damage)
    {
        
    }
    public IEnumerator SpecialMove()
    {
        agent.speed = suicidespeed;
        yield return new WaitForSeconds(suicideTimer);
        Death();//Health-=100;
    }
    

    #endregion
    
}
