using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumOnLeg : HellEnemy
{
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField, Tooltip("Место появления снаряда")] protected Transform shootPoint;
    [SerializeField, Tooltip("Слои, которые не будут считаться препятствиями")] protected LayerMask ignoreMask;
    public override IEnumerator SpecialMove()
    {
        
        GameObject currentBullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation) as GameObject;
        Vector3 Direction = currentBullet.transform.forward;
        Direction.y = 0;
        currentBullet.transform.forward = Direction;
        currentBullet.GetComponent<Bullet>().Init(50, 5, damage, ignoreMask);
        Vector3 pos = (target.transform.position - transform.position).normalized;
        pos.y = 0;
        transform.forward = pos;
        yield return new WaitForSeconds(0);
    }
}
