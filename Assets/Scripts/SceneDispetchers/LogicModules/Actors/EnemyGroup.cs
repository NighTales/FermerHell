using System.Collections;
using UnityEngine;

public class EnemyGroup : LogicActor
{
    public override void ActivateModule()
    {
        StartCoroutine(CheckGroup());
    }

    private IEnumerator CheckGroup()
    {
        yield return new WaitForSeconds(1);

        if(transform.childCount == 0)
        {
            ActivateAllNextModules();
            Destroy(gameObject, 1);
        }

    }
}
