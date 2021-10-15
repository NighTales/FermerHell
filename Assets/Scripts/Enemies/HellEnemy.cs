using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class HellEnemy : Enemy
{
    [SerializeField] protected EnemyState state;
    [SerializeField, Tooltip("Целевой объект")] protected GameObject target;
    [SerializeField, Tooltip("Дальность атаки"), Range(1, 10)] protected float attackDistance = 5;
    [SerializeField, Tooltip("Дальность обзора"), Range(1, 100)] protected float visionDistance = 5;
    [SerializeField] protected Animator anim;
    protected NavMeshAgent agent;
    protected PhysicsPartController partController;
    [SerializeField, Tooltip("Скорость"), Range(1, 10)] public float speed = 5;
    
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
        agent.speed = speed;
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
    protected virtual IEnumerator Born()
    {
        anim.SetBool("WithoutLegs",false);
        yield return new WaitForSeconds(1);
        state = EnemyState.Iddle; // idlle
    }
    #endregion
    
    public  void Stun(bool stunned)
    {
        agent.isStopped = stunned;
    }
    protected virtual void Update()
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
        if(target != null)
        {
            //agent.isStopped = false;
            if ((target.transform.position - transform.position).magnitude >visionDistance)
            {
                state = EnemyState.Iddle;
            }
            else if ((target.transform.position - transform.position).magnitude <attackDistance)
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
    
    public virtual void Attack()
    {
        agent.isStopped = true;
        anim.SetTrigger("Attack");   //???
        if ((target.transform.position - transform.position).magnitude > attackDistance)//делать рейкаст и доставать компонент каждый раз???
        {
            state = EnemyState.MoveToTarget;
        }
    }
    


    #endregion
    
    
}
