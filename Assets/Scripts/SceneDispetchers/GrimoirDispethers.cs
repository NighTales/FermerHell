using UnityEngine;

public class GrimoirDispethers : MonoBehaviour
{
    [HideInInspector] public bool containsRedGrimoir;
    [HideInInspector] public bool containsBlueGrimoir;
    [HideInInspector] public bool containsGreenGrimoir;

    [HideInInspector] public GameObject redGrimoir;
    [HideInInspector] public GameObject blueGrimoir;
    [HideInInspector] public GameObject greenGrimoir;

    public void ActivateRedGrimoir() => containsRedGrimoir = true;
    public void ActivateBlueGrimoir() => containsBlueGrimoir = true;
    public void ActivateGreenGrimoir() => containsGreenGrimoir = true;
}
