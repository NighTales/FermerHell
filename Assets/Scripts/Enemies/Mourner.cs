using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mourner : HellEnemy
{
    [SerializeField]  private List<Ghost> ghosts; 
    [SerializeField]  private List<Ghost> ghostsPack;
    private int deadGhostsCount = 4;
   [SerializeField] private Vector3 buffTarget;
   [SerializeField]private float iddleDistance = 0.5f;
   [SerializeField]  private bool finded = false;
   private bool isReadyForAttack = true;
   [SerializeField, Tooltip("перезарядка призыва"), Range(1, 10)]private float readyTime = 4;
    public void Init(GameObject target)
    {
        
        base.Init(target);
        deadGhostsCount = 4;
        buffTarget = new Vector3();
        buffTarget.x = 999;
    }
    protected override void Update()
    {
        if (deadGhostsCount == 4 && !stunned && isReadyForAttack)
        {
            isReadyForAttack = false;
            deadGhostsCount = 0;
            Attack();
        }
        base.Update();
    }

    public void GhostDeadReactor()
    {
        deadGhostsCount++;
    }

    #region StateMethods

    protected override void Iddle()
    {
        agent.isStopped = true;
        anim.SetBool("Walk", false);

        if (target != null)
        {
            if ((target.transform.position - transform.position).magnitude < visionDistance)
            {
                state = EnemyState.MoveToTarget;
            }
        }
    }


    public void FindTarget()
    {
        finded = false;
        Vector3 vec =transform.position+ transform.position - target.transform.position;
        
        Vector3 normVector3 = vec / vec.magnitude;
        float distance = visionDistance - vec.magnitude * 2;
        
        NavMeshPath path = new NavMeshPath();
        
        while (true)
        {
            Vector3 vec1 = new Vector3(Random.Range(transform.position.x,vec.x),transform.position.y,Random.Range(transform.position.z,vec.z));
            if (agent.CalculatePath(vec1, path))
            {
                
                buffTarget= vec1 ;
                finded = true;
                return;
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
            else
            {
                if (finded && (buffTarget - transform.position).magnitude < iddleDistance)
                {
                    finded = false;
                }
                else
                {
                    FindTarget();
                    anim.SetBool("Walk", true);
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
        anim.SetTrigger("Attack");
    }


    public override IEnumerator SpecialMove()
    {
        for (int i = 0; i < 4; i++)
        {
            bool check = true;
            while (check)
            {
             int r = Random.Range(0, 5);
             if (!ghosts.Contains(ghostsPack[r]))
             {
                 ghosts.Add(ghostsPack[r]);
                 check = false;
             }
            }
            
        }
        Instantiate(ghosts[0], transform.position + Vector3.forward*2, transform.rotation).Init(target,this);
        Instantiate(ghosts[1], transform.position + Vector3.left*2, transform.rotation).Init(target,this);
        Instantiate(ghosts[2], transform.position + Vector3.right*2, transform.rotation).Init(target,this);
        Instantiate(ghosts[3], transform.position + Vector3.back*2, transform.rotation).Init(target,this);
        ghosts.Clear();
        yield return new WaitForSeconds(readyTime);
        isReadyForAttack = true;
    }

    #endregion
}