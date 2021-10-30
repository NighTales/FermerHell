using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Основной режим - клик - выстрел 
 * Альтернативный - переключение на стрельбу при удержании кнопки с накоплением скорости стрельбы
 */

public class Shotgun : Weapon
{
    [SerializeField, Range(1, 10)]
    private int bulletCount = 5;
    [SerializeField, Range(0.01f, 0.3f)]
    private float maxAngle = 0.15f;
    [SerializeField, Range(0.01f, 2)]
    private float minRecoilTime = 0.5f;
    [SerializeField, Range(1, 10)]
    private float maxRecoilTime = 1f;
    [SerializeField, Range(0.1f, 1)]
    private float step = 0.2f;


    bool altShoot;
    private float currentRecoilTime;
    PlayerBonusStat instant = PlayerBonusStat.Instant;
    private void Start()
    {
        altShoot = false;
        opportunityToShoot = true;
        ChangeShootType();
    }

    public override void FirstShoot()
    {
        if (altShoot)
        {
            if (Input.GetButton("Fire1"))
            {
                if (opportunityToShoot)
                {
                    InstanceListOfBulletWithRandomAngle(bulletCount / 2);
                    source.PlayOneShot(shoot);
                    anim.SetInteger("AltShoot", 2);
                    opportunityToShoot = false;
                    //pack.currentAmmo--;
                    //Messenger<int>.Broadcast(GameEvent.AMMO_ARE_CHANGED, pack.currentAmmo);
                    Invoke("ReturnOpportunityToShoot", currentRecoilTime);
                    currentRecoilTime = maxRecoilTime - 0.2f * instant.bonusPack[BonusType.FireRate].value; ;
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && opportunityToShoot)
            {
                InstanceListOfBulletWithRandomAngle(bulletCount);
                source.PlayOneShot(shoot);
                anim.SetTrigger("Shoot");
                opportunityToShoot = false;
                //Messenger<int>.Broadcast(GameEvent.AMMO_ARE_CHANGED, pack.currentAmmo);
            }
        }
    }
    public override void HideWeapon()
    {
        if (altShoot)
        {
            ChangeShootType();
        }
        base.HideWeapon();
    }

    public override void SecondShoot()
    {
        //if (Input.GetButtonDown("Fire2"))
        //{
        //    ChangeShootType();
        //}
    }

    private void StartReloadSound()
    {
        source.PlayOneShot(reload);
    }
    private void ReturnOpportunityToShoot()
    {
        opportunityToShoot = true;
    }
    private void ReturnAltShootToDefault()
    {
        if (altShoot)
        {
            anim.SetInteger("AltShoot", 1);
        }
    }
    private void ChangeShootType()
    {
        altShoot = !altShoot;
        anim.SetInteger("AltShoot", altShoot ? 1 : 0);
        opportunityToShoot = false;
    }

    private void InstanceListOfBulletWithRandomAngle(int countOfBullet)
    {
        for (int i = 0; i < countOfBullet; i++)
        {
            GameObject currentBullet = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
            if (i > 0)
                currentBullet.transform.forward = GetRandomVector(maxAngle + 0.1f * instant.bonusPack[BonusType.Area].value);
            int damageMultiplicator = damage + damage * instant.bonusPack[BonusType.Damage].value * 50 / 100;
            currentBullet.GetComponent<Bullet>().Init(bulletSpeed, pack.bulletLifeTime, damageMultiplicator, ignoreMask);
        }
    }
    private Vector3 GetRandomVector(float angle)
    {
        return shootPoint.forward + shootPoint.right * Random.Range(-angle, angle) + shootPoint.up * Random.Range(-angle, angle);
    }
}
