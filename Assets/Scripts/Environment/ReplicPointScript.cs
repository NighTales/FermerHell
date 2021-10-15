using System.Collections.Generic;
using UnityEngine;

public class ReplicPointScript : MonoBehaviour
{
    [HideInInspector]public ReplicDispether replicDispether;
    public List<ReplicItem> replicas;

    public void PlayReplicas()
    {
        replicDispether.AddInList(replicas);

        float delayTime = 0;

        foreach (var item in replicas)
        {
            delayTime += item.clip.length;
        }
        delayTime += 2;
        Destroy(gameObject, delayTime + 1);
    }
}
