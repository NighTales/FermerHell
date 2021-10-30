using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GrimoirDispethers : MonoBehaviour
{
    public bool containsRedGrimoir = false;
    public bool containsBlueGrimoir = false;
    public bool containsGreenGrimoir = false;

    public GameObject redGrimoir;
    public GameObject blueGrimoir;
    public GameObject greenGrimoir;

    public List<GameObject> LootList = new List<GameObject>();

    public GameObject R_Charge;
    public GameObject G_Charge;
    public GameObject B_Charge;

    public void ActivateRedGrimoir()
    {
        containsRedGrimoir = true;
        LootList.Add(R_Charge);
    }
    public void ActivateBlueGrimoir()
    {
        containsBlueGrimoir = true;
        LootList.Add(B_Charge);
    }
    public void ActivateGreenGrimoir()
    {
        containsGreenGrimoir = true;
        LootList.Add(G_Charge);
    }


    private void FixedUpdate()
    {
        if (redGrimoir == null)
        {
            ActivateRedGrimoir();
        }
        if (blueGrimoir == null)
        {
            ActivateBlueGrimoir();
        }
        if (greenGrimoir == null)
        {
            ActivateGreenGrimoir();
        }
    }


}
