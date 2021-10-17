using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Модуль, который может активировать другие модули
/// </summary>
public abstract class LogicActor : LogicModule
{
    [Header("Эктор")]
    [Tooltip("Объекты, у которых будет вызываться метод USE()")]
    public List<LogicModule> nextModules;

    private void Awake()
    {
        AddThisActorToNextModules();
    }

    private void AddThisActorToNextModules()
    {
        foreach (var item in nextModules)
        {
            item.Actors.Add(this);
        }
    }

    /// <summary>
    /// Передать сигнал всем следующим модулям
    /// </summary>
    protected void ActivateAllNextModules()
    {
        for (int i = 0; i < nextModules.Count; i++)
        {
            if (nextModules[i] != null)
            {
                nextModules[i].ActivateModule();
            }
            else
            {
                Debug.LogError("Элемент " + i + " равен null. Вероятно, была утеряна ссылка. Источник :" + gameObject.name);
            }
        }
    }

    /// <summary>
    /// Вернуть все модули в исходное состояние
    /// </summary>
    protected void AllNextModulesToDefaultState()
    {
        for (int i = 0; i < nextModules.Count; i++)
        {
            if (nextModules[i] != null)
            {
                nextModules[i].ReturnToDefaultState();
            }
            else
            {
                Debug.LogError("Элемент " + i + " равен null. Вероятно, была утеряна ссылка. Источник :" + gameObject.name);
            }
        }
    }
}
