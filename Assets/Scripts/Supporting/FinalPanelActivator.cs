using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPanelActivator : MonoBehaviour
{
    [SerializeField] private UIScript uIScript;
    [SerializeField] private string message;

    public void ShowFinalPanel()
    {
        uIScript.ShowFinalPanel(message);
        Destroy(gameObject);
    }
}
