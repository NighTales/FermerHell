using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEventReactor : MonoBehaviour
{
    [SerializeField]private Sceleton _sceletonEnemy;

    public void StartStun()
    {
        _sceletonEnemy.Stun(true);
    }
    public void StopStun()
    {
        
        _sceletonEnemy.Stun(false);
    }

    public void SpecialMove()
    {
        StartCoroutine(_sceletonEnemy.SpecialMove());
    }
  
}
