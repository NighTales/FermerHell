using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : Enemy
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField, Tooltip("Расстояние, на котором носитель скрипта будет реагировать на препятствие (длина луча)")] protected float obstacleRange = 5.0f;
    [SerializeField, Tooltip("Слои, которые не будут считаться препятствиями")] protected LayerMask ignoreMask;
    [SerializeField, Tooltip("Место появления снаряда")] protected Transform shootPoint;

    protected bool recharge = false;

    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
        RayShoot();
    }

    public virtual void RayShoot()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 2, out hit))
        {

            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.GetComponent<PlayerCharacter>())
            {
                if (!recharge)
                {
                    shootPoint.LookAt(hitObject.transform.position + Vector3.up);   
                    GameObject currentBullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation) as GameObject;
                    currentBullet.GetComponent<Bullet>().Init(50, 5, damage, ignoreMask);
                    recharge = true;
                    Invoke("StopRecharge", 1);
                }
            }
            else if (hit.distance < obstacleRange)
            {
                float angle = Random.Range(-110, 110);
                transform.Rotate(0, angle, 0);
            }
        }
    }

    /// <summary>
    /// Вызывается с помощью Invoke
    /// </summary>
    private void StopRecharge() => recharge = false;
}
