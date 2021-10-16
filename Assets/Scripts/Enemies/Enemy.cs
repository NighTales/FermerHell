using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState
{
    Iddle,
    MoveToTarget,
    Attack,
    Recoil,
}


[RequireComponent(typeof(Rigidbody))]
public abstract class Enemy : AliveController
{
    [SerializeField, Range(1,100), Tooltip("Количество очков, получаемое за победу над врагом")] protected int scoreForWin = 1;
    [SerializeField, Range(1, 100)] protected int damage = 10;
    [SerializeField] protected List<GameObject> lootPrefabs;
    [SerializeField] protected GameObject postDeadDecal;
    [SerializeField] protected GameObject afterFightLoot;
    private Rigidbody rb;

    protected virtual void Start()
    {
        Health = maxHealth;
        rb = GetComponent<Rigidbody>();
        ReturnRB();
    }

    protected virtual void OnFightAction()
    {
       // Messenger<int>.Broadcast(GameEvent.ENEMY_HIT, scoreForWin);
        Vector3 dir = new Vector3(Random.Range(-0.05f, 0.05f), 2, Random.Range(-0.05f, 0.05f));
        Instantiate(afterFightLoot, transform.position + dir, Quaternion.identity).GetComponent<Rigidbody>()
            .AddForce(dir, ForceMode.Impulse);
        Instantiate(postDeadDecal, transform.position, Quaternion.identity).GetComponent<Decal>().Init(2);
       // Messenger.Broadcast(GameEvent.ENEMY_DEAD);
        Destroy(gameObject);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
           // Messenger.Broadcast(GameEvent.HIT);
            GetDamage(other.GetComponent<Bullet>().damage);
        }
        else if(other.CompareTag("Fire"))
        {
            ExplosionZone zone = other.GetComponent<ExplosionZone>();
            if(zone != null)
            {
                if(rb == null)
                {
                    rb = GetComponent<Rigidbody>();
                }
                rb.useGravity = true;
                rb.isKinematic = false;

                Vector3 dir = other.transform.position - (transform.position + Vector3.up);
                rb.AddForce(dir.normalized * zone.force, ForceMode.Impulse);
                Invoke("ReturnRB", 1);
                GetDamage(zone.damage);
            }
        }
        else if(other.CompareTag("Turret"))
        {
            other.GetComponent<Turret>().AddTarget(transform);
        }
        else if(other.CompareTag("DeadZone"))
        {
            Death();
        }
        else if (other.CompareTag("Blade"))
        {
         //   Messenger.Broadcast(GameEvent.HIT);
            OnFightAction();
        }
        else if (other.CompareTag("Burn"))
        {
            
        }
    }
    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Turret"))
        {
            other.GetComponent<Turret>().RemoveTarget(transform);
        }
    }

    public override void Death()
    {
       // Messenger<int>.Broadcast(GameEvent.ENEMY_HIT, scoreForWin);
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
       // Messenger.Broadcast(GameEvent.ENEMY_DEAD);
        Destroy(gameObject);
    }
    protected void ReturnRB()
    {
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = true;
    }
}
