using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEventReactor : MonoBehaviour
{
    [SerializeField] protected HellEnemy _hellEnemy;

    public void StartStun()
    {
        _hellEnemy.Stun(true);
    }
    public void StopStun()
    {
        
        _hellEnemy.Stun(false);
    }

    public void SpecialMove()
    {
        StartCoroutine(_hellEnemy.SpecialMove());
    }
}
