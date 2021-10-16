using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mourner : HellEnemy
{
    [SerializeField]  private List<Ghost> ghosts;
    [SerializeField] private int deadGhostsCount = 0;
   [SerializeField] private Vector3 buffTarget;
   [SerializeField]private float iddleDistance = 0.5f;

    public void Init(GameObject target)
    {
        base.Init(target);
        buffTarget = new Vector3();
        buffTarget.x = 999;
    }
    protected virtual void Update()
    {
        if (deadGhostsCount == 4)
        {
            Attack();
        }

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

    public void GhostDeadReactor()
    {
        deadGhostsCount++;
    }

    #region StateMethods

    protected override void Iddle()
    {
        agent.isStopped = true;
        //anim.SetBool("Walk", false);

        if (target != null)
        {
            if ((target.transform.position - transform.position).magnitude < visionDistance)
            {
                state = EnemyState.MoveToTarget;
            }
        }
    }

    [SerializeField]  private bool finded = false;
    public void FindTarget()
    {
        finded = false;
        Vector3 vec = transform.position - target.transform.position;
        Vector3 normVector3 = vec / vec.magnitude;
        float distance = visionDistance - vec.magnitude * 2;
        NavMeshPath path = new NavMeshPath();
        float num = 1;
        while (true)
        {
            if (agent.CalculatePath(transform.position + normVector3 * distance * num, path))
            {
                
                buffTarget=  transform.position + normVector3 * distance * num;
                finded = true;
                return;
            }
            else if (num > 10)
            {
                Debug.Log("!");
                return;
            }

            {
                num += 0.5f;
            }
        }
    }

    protected override void Move()
    {
        agent.isStopped = false;
        if (target != null)
        {
            if ((target.transform.position - transform.position).magnitude > visionDistance)
            {
                state = EnemyState.Iddle;
            }
            else if (!finded)
            {
                FindTarget();
            }
            else
            {
                if ((buffTarget - transform.position).magnitude < iddleDistance)
                {
                    finded = false;
                }
                else
                {
                   // anim.SetBool("Walk", true);
                    agent.destination = buffTarget;
                }
            }
        }
        else if (!agent.isStopped)
        {
            agent.isStopped = true;
        }
    }

    public override void Attack()
    {
        agent.isStopped = true;
        anim.SetTrigger("Attack"); //???
        if ((target.transform.position - transform.position).magnitude >
            attackDistance) //делать рейкаст и доставать компонент каждый раз???
        {
            state = EnemyState.MoveToTarget;
        }
    }

    #endregion
}