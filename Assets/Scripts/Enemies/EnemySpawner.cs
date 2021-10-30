using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : LogicModule
{
    [SerializeField] private List<EnemySpawnItem> enemiesItems;
    [SerializeField] private EnemyGroup group;

    public override void ActivateModule()
    {
        for (int i = 0; i < enemiesItems.Count; i++)
        {
            HellEnemy enemy = Instantiate(enemiesItems[i].enemy, enemiesItems[i].point.position, enemiesItems[i].point.rotation, group.transform).GetComponent<HellEnemy>();
            enemy.afterDeadEvent.AddListener(group.ActivateModule);
        }
    }
}

[System.Serializable]
public class EnemySpawnItem
{
    public GameObject enemy;
    public Transform point;
}
