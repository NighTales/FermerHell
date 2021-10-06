using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TestDamageScript : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private UnityEvent _damageEvent;
    public int Damage;

    [ContextMenu("Ебаш")]public void SetDamage()
    {
        _enemy.GetDamage(Damage);
    }
}
