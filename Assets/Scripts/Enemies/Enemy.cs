using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public enum EnemyState
{
    Iddle,
    MoveToTarget,
    Attack,
    Recoil,
}


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class Enemy : AliveController
{
    [SerializeField, Range(1, 100), Tooltip("Количество очков, получаемое за победу над врагом")] protected int scoreForWin = 1;
    [SerializeField, Range(1, 100)] protected int damage = 10;
    [SerializeField] protected List<GameObject> lootPrefabs;
    [SerializeField] protected GameObject postDeadDecal;
    [SerializeField] protected GameObject afterFightLoot;
    private Rigidbody rb;
    protected float speed = 5f;

    [SerializeField, Range(1, 100), Tooltip("Стандартная Скорость")] protected float basespeed;

    public UnityEvent afterDeadEvent;

    [SerializeField] public Dictionary<BonusType, int> buffeeds;
    private float dOTTime = 0f;


    protected virtual void Start()
    {
        buffeeds = new Dictionary<BonusType, int>();
        buffeeds.Add(BonusType.Speed, 0);
        buffeeds.Add(BonusType.DOT, 0);
        speed = basespeed;
        Health = maxHealth;
        rb = GetComponent<Rigidbody>();
        ReturnRB();
    }
    protected virtual void Update()
    {
        if (dOTTime > 0)
        {
            dOTTime -= Time.deltaTime;
        }
    }
    protected virtual void OnFightAction()
    {
        // Messenger<int>.Broadcast(GameEvent.ENEMY_HIT, scoreForWin);
        Vector3 dir = new Vector3(Random.Range(-0.05f, 0.05f), 2, Random.Range(-0.05f, 0.05f));
        Instantiate(afterFightLoot, transform.position + dir, Quaternion.identity).GetComponent<Rigidbody>()
            .AddForce(dir, ForceMode.Impulse);
        if (postDeadDecal != null)
            Instantiate(postDeadDecal, transform.position, Quaternion.identity).GetComponent<Decal>().Init(2);
        // Messenger.Broadcast(GameEvent.ENEMY_DEAD);
        Destroy(gameObject);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            // Messenger.Broadcast(GameEvent.HIT);
            GetDamage(other.GetComponent<Bullet>().damage);
        }
        else if (other.CompareTag("Fire"))
        {
            ExplosionZone zone = other.GetComponent<ExplosionZone>();
            if (zone != null)
            {
                if (rb == null)
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
        else if (other.CompareTag("FriendFire"))
        {
            ExplosionZone zone = other.GetComponent<ExplosionZone>();
            if (zone != null)
            {
                if (rb == null)
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
        // else if(other.CompareTag("Turret"))
        // {
        //     other.GetComponent<Turret>().AddTarget(transform);
        // }
        // else if(other.CompareTag("DeadZone"))
        // {
        //     Death();
        // // }
        // else if (other.CompareTag("Blade"))
        // {
        //  //   Messenger.Broadcast(GameEvent.HIT);
        //     OnFightAction();
        // }
        // else if (other.CompareTag("Burn"))
        // {
        //     buffeeds[BonusType.DOT]++;
        //     OnTakeDebuffDOT(other.gameObject.GetComponent<Buff>().buffvalue,other.gameObject.GetComponent<Buff>().time);
        // } 
        // else if (other.CompareTag("Slow"))
        // {
        //     OnTakeDebuffSpeed(other.gameObject.GetComponent<Buff>().buffvalue);
        // }
    }
    protected void OnTriggerExit(Collider other)
    {
        // if (other.CompareTag("Turret"))
        // {
        //     other.GetComponent<Turret>().RemoveTarget(transform);
        // }
    }

    public override void Death()
    {
        afterDeadEvent?.Invoke();
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
        if (postDeadDecal != null)
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


    #region Buffs

    public void StartBuff(BonusType type, int value, float time)
    {
        switch (type)
        {
            case BonusType.Speed:
                OnTakeDebuffSpeed(value);
                break;
            case BonusType.DOT:
                OnTakeDebuffDOT(value, time);
                break;
            default:
                break;
        }
    }

    protected int slowvalue = 0;
    protected void OnTakeDebuffSpeed(int value)
    {
        slowvalue = value;
        float actionvalue = speed;
        if (buffeeds[BonusType.Speed] == 0)
        {
            speed = basespeed;
        }
        else
        {
            speed = basespeed - (basespeed * (float)value / 100);
        }
        OnSpeedChangeAction(speed / actionvalue);
    }

    protected virtual void OnSpeedChangeAction(float value)
    {

    }

    protected void OnTakeDebuffDOT(int value, float time)// думай леха думай
    {
        if (buffeeds[BonusType.DOT] == 0)
        {
            dOTTime = time;
        }
        else
        {
            StartCoroutine(DOTDeBuffTic(value));
        }
    }

    public IEnumerator DOTDeBuffTic(int value)
    {
        do
        {
            GetDamage(value);
            yield return new WaitForSeconds(1);

        } while (dOTTime > 0 || buffeeds[BonusType.DOT] != 0);
    }


    #endregion

    public override void GetDamage(int damage)
    {
        Messenger.Broadcast(GameEvent.HIT);
        if (Health > 0)
            Health -= damage;
    }
}
