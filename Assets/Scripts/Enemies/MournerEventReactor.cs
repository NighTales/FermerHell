using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MournerEventReactor : AnimEventReactor
{
    [SerializeField] protected Mourner _hellEnemyMourner;
    public void StartProcess()
    {
        _hellEnemyMourner.Process(true);
    }
    public void StopProcess()
    {
        _hellEnemyMourner.Process(false);
    }
}
