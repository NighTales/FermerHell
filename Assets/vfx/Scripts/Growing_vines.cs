using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growing_vines : MonoBehaviour
{
    public List<MeshRenderer> growVinesMeshes;
    public float timeToGrow = 5;
    public float lifetime = 1;
    public float refreshRate = 0.05f;
    [Range(0, 1)]
    public float minGrow = 0.02f;
    [Range(0, 1)]
    public float maxGrow = 0.97f;

    private List<Material> growVinesMaterials = new List<Material>();
    private bool fullyGrow;
    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i<growVinesMeshes.Count; i++)
        {
            for (int j=0; j<growVinesMeshes[i].materials.Length; j++)
            {
                if (growVinesMeshes[i].materials[j].HasProperty("Grow_"))
                {
                    growVinesMeshes[i].materials[j].SetFloat("Grow_", minGrow);
                    growVinesMaterials.Add(growVinesMeshes[i].materials[j]);
                }
            }
        }
        StartCoroutine(GrowVinesUpdate());
    }

    IEnumerator GrowVinesUpdate ()
    {
        while (true)
        {
            for (int i = 0; i < growVinesMaterials.Count; i++)
            {
                StartCoroutine(GrowVines(growVinesMaterials[i]));
            }
            
            yield return new WaitForSeconds(lifetime);
        }
    }

    IEnumerator GrowVines (Material mat)
    {
        float growValue = mat.GetFloat("Grow_");
        if (!fullyGrow)
        {
            while (growValue<maxGrow)
            {
                growValue += 1 / (timeToGrow / refreshRate);
                mat.SetFloat("Grow_", growValue);
                yield return new WaitForSeconds(refreshRate);
            }
        }
        else
        {
            while (growValue > minGrow)
            {
                growValue -= 1 / (timeToGrow / refreshRate);
                mat.SetFloat("Grow_", growValue);
                yield return new WaitForSeconds(refreshRate);
            }
        }

        if (growValue >= maxGrow)
        {
            fullyGrow = true;
        }
        else
        {
            fullyGrow = false;
        }
    }
}
