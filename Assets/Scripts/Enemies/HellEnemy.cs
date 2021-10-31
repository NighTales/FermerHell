using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class HellEnemy : Enemy
{
    [SerializeField] protected EnemyState state;
    [SerializeField, Tooltip("Целевой объект")] protected GameObject target;
    [SerializeField, Tooltip("Дальность атаки"), Range(1, 100)] protected float attackDistance = 5;
    [SerializeField, Tooltip("Дальность обзора"), Range(1, 100)] protected float visionDistance = 5;
    [SerializeField] protected Animator anim;
    protected NavMeshAgent agent;
    protected PhysicsPartController partController;
    protected bool stunned = true;
    protected NavMeshPath path;
    #region Initialization

    private void Awake()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    public virtual void Init(GameObject target)
    {
        this.target = target;
        path = new NavMeshPath();
        agent.destination = target.transform.position;
        agent.isStopped = true;
    }



    protected override void Start()
    {
        base.Start();
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        agent.speed = speed;
        if (target != null)
            Init(target);
        else
        {
            target = GameObject.FindGameObjectWithTag("Player");
            if (target != null)
                Init(target);
        }

        Born();
        // anim = GetComponent<Animator>();
        partController = GetComponent<PhysicsPartController>();
    }
    protected virtual IEnumerator Born()
    {
        anim.SetBool("WithoutLegs", false);
        yield return new WaitForSeconds(1);
        state = EnemyState.Iddle; // idlle
    }
    #endregion



    public void Stun(bool stunned)
    {
        agent.isStopped = stunned;
        this.stunned = stunned;
    }
    protected override void Update()
    {
        base.Update();
        if (!stunned)
        {

            switch (state)
            {
                case EnemyState.Iddle:
                    Iddle();
                    break;
                case EnemyState.MoveToTarget:
                    Move();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
                default:
                    break;
            }
        }
    }
    #region StateMethods

    protected virtual void Iddle()
    {
        agent.isStopped = true;
        anim.SetBool("Walk", false);

        if (target != null)
        {
            if ((target.transform.position - transform.position).magnitude < attackDistance)
            {
                state = EnemyState.Attack;
            }
            else if ((target.transform.position - transform.position).magnitude < visionDistance)
            {
                state = EnemyState.MoveToTarget;
            }
        }

    }


    protected virtual void Move()
    {
        agent.isStopped = false;
        if (target != null)
        {
            //agent.isStopped = false;
            if ((target.transform.position - transform.position).magnitude > visionDistance)
            {
                state = EnemyState.Iddle;
            }
            else if ((target.transform.position - transform.position).magnitude < attackDistance)
            {
                state = EnemyState.Attack;
            }
            else
            {
                anim.SetBool("Walk", true);
                var position = target.transform.position;
                agent.CalculatePath(position, path);
                agent.path = path;
            }
        }
        else if (!agent.isStopped)
        {
            agent.isStopped = true;
        }
    }

    protected override void OnSpeedChangeAction(float value)
    {

        agent.speed = speed;
        anim.speed = value;

    }

    public virtual void Attack()
    {
        agent.isStopped = true;
        anim.SetTrigger("Attack");   //???
        if ((target.transform.position - transform.position).magnitude > attackDistance && !stunned)//делать рейкаст и доставать компонент каждый раз???
        {
            state = EnemyState.MoveToTarget;
        }
    }



    #endregion
    public virtual IEnumerator SpecialMove()
    {
        yield return new WaitForSeconds(1);
    }


    public override void GetDamage(int damage)
    {
        Messenger.Broadcast(GameEvent.HIT);
        if (Health > 0)
        {
            Health -= damage;
            anim.SetInteger("Damage", damage);
            anim.SetTrigger("GetDamage");
        }
    }
    [Range(0f,10f)] public float DeadDelaySec = 1f;
    public override void Death()
    {
        anim.SetTrigger("Dead");

        StartCoroutine(DelayedDeth(DeadDelaySec));
    }
    protected IEnumerator DelayedDeth(float delaySec)
    {
        yield return new WaitForSeconds(delaySec);
        base.Death();
    }

}
