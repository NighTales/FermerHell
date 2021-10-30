using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disolve : MonoBehaviour
{
    public List<MeshRenderer> disolveMeshes;
    public float timeToDisolve = 5;
    public float lifetime = 1;
    public float refreshRate = 0.05f;
    [Range(0, 1)]
    public float minDis = 0f;
    [Range(0, 1)]
    public float maxDis = 2f;

    private List<Material> disolveMaterials = new List<Material>();
    private bool fullyGrow;
    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i< disolveMeshes.Count; i++)
        {
            for (int j=0; j< disolveMeshes[i].materials.Length; j++)
            {
                if (disolveMeshes[i].materials[j].HasProperty("disolve_amount_"))
                {
                    disolveMeshes[i].materials[j].SetFloat("disolve_amount_", minDis);
                    disolveMaterials.Add(disolveMeshes[i].materials[j]);
                }
            }
        }
        StartCoroutine(DisolveUpdate());
    }

    IEnumerator DisolveUpdate ()
    {
        while (true)
        {
            for (int i = 0; i < disolveMaterials.Count; i++)
            {
                StartCoroutine(Disolve_mat(disolveMaterials[i]));
            }
            
            yield return new WaitForSeconds(lifetime);
        }
    }

    IEnumerator Disolve_mat (Material mat)
    {
        float disValue = mat.GetFloat("disolve_amount_");
        if (!fullyGrow)
        {
            while (disValue < maxDis)
            {
                disValue += 1 / (timeToDisolve / refreshRate);
                mat.SetFloat("disolve_amount_", disValue);
                yield return new WaitForSeconds(refreshRate);
            }
        }
        else
        {
            while (disValue > minDis)
            {
                disValue -= 1 / (timeToDisolve / refreshRate);
                mat.SetFloat("disolve_amount_", disValue);
                yield return new WaitForSeconds(refreshRate);
            }
        }

        if (disValue >= maxDis)
        {
            fullyGrow = true;
        }
        else
        {
            fullyGrow = false;
        }
    }
}
