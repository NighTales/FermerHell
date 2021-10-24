using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPartController : MonoBehaviour
{
    [SerializeField] private List<PhysicsPart> FirstPartGroup;
    [SerializeField] private List<PhysicsPart> SecondPartGroup;

    public void FirstPartImpulse(Vector3 originPosition)
    {
        Vector3 direction;

        for (int i = 0; i < FirstPartGroup.Count; i++)
        {
            FirstPartGroup[i].tag = "Untagged";
        
            direction = (FirstPartGroup[i].transform.position - originPosition).normalized;
            FirstPartGroup[i].ImpulseToPart(direction);
        }
    }

    public void SecondPartImpulse(Vector3 originPosition)
    {
        Vector3 direction;

        for (int i = 0; i < SecondPartGroup.Count; i++)
        {
            SecondPartGroup[i].tag = "Untagged";
            direction = (SecondPartGroup[i].transform.position - originPosition).normalized;
            SecondPartGroup[i].ImpulseToPart(direction);
        }
    }
}
