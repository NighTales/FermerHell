﻿using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Задаёт указанным объектам значение activeSalfe, равное state
/// </summary>
[HelpURL("https://docs.google.com/document/d/1GP4_m0MzOF8L5t5pZxLChu3V_TFIq1czi1oJQ2X5kpU/edit?usp=sharing")]
public class GameObjectActivator : LogicModule
{
    [Tooltip("Объекты, которые будут переключены. TargetState для каждого после переключения" +
        "сменится на противоположный. Модуль можно будет использовать повторно для обратного эффекта.")]
    [SerializeField]
    private List<StateContainer> targets;

    private void Awake()
    {
        foreach (var item in targets)
        {
            item.defaultValue = item.targetGO.activeSelf;
        }
    }

    /// <summary>
    /// Переключить активность указанных объектов
    /// </summary>
    [ContextMenu("Переключить объекты")]
    public override void ActivateModule()
    {
        SetStateForAll();
        used = true;
    }
    /// <summary>
    /// Переключить объекты в состояние по умолчанию
    /// </summary>
    [ContextMenu("Переключить объекты в состояние по умолчанию")]
    public override void ReturnToDefaultState()
    {
        used = false;
        foreach (var item in targets)
        {
            item.targetState = item.defaultValue;
            item.targetGO.SetActive(item.defaultValue);
        }
    }

    private void SetStateForAll()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null)
            {
                targets[i].targetGO.SetActive(targets[i].targetState);
                targets[i].targetState = !targets[i].targetState;
            }
        }
    }
    private void OnDrawGizmos()
    {
        if(debug)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(transform.position, 0.3f);

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null && targets[i].targetGO != null)
                {
                    if (targets[i].targetState)
                    {
                        Gizmos.color = Color.green;
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                    }
                    Gizmos.DrawLine(transform.position, targets[i].targetGO.transform.position);
                }
            }
        }
    }
}

[System.Serializable]
public class StateContainer
{
    [Tooltip("Объект, которому нужно задать состояние")] public GameObject targetGO;
    [Tooltip("Целевое состояние. Если отмечено, объект будет включен")] public bool targetState = false;
    [HideInInspector] public bool defaultValue;
}
