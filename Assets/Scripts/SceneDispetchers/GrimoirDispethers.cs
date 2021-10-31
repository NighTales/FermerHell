using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class GrimoirDispethers : MonoBehaviour
{
    [SerializeField]
    Grimoir Instant = Grimoir.Instant;

    public GameObject R_Charge;
    public GameObject G_Charge;
    public GameObject B_Charge;
    private void Start()
    {
        Instant.LootList.Remove(R_Charge);
        Instant.LootList.Remove(G_Charge);
        Instant.LootList.Remove(B_Charge);
    }
    public void ActivateRedGrimoir()
    {
        Instant.LootList.Add(R_Charge);
    }
    public void ActivateBlueGrimoir()
    {
        Instant.LootList.Add(B_Charge);
    }
    public void ActivateGreenGrimoir()
    {
        Instant.LootList.Add(G_Charge);
    }
}
[Serializable]
public class Grimoir
{
    public static Grimoir Instant
    {
        get
        {
            if (instant == null)
                instant = new Grimoir();
            return instant;
        }
    }
    private static Grimoir instant;
    private Grimoir() { }

    public List<GameObject> LootList = new List<GameObject>();

   
}
