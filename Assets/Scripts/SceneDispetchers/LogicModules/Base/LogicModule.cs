using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Базовый модуль, который может выполнять действия
/// </summary>
public abstract class LogicModule: MonoBehaviour
{
    [Header("Модуль")]
    [Tooltip("Добавить вспомогательную отрисовку"), SerializeField] protected bool debug;

    /// <summary>
    /// Экторы, которые могут активировать этот модуль
    /// </summary>
    public List<LogicActor> Actors
    {
        get
        {
            if (_actors == null)
                _actors = new List<LogicActor>();

            return _actors;
        }
    }
    private List<LogicActor> _actors;

    [HideInInspector] public bool used;

    /// <summary>
    /// выполнить действие
    /// </summary>
    public abstract void ActivateModule();

    /// <summary>
    /// перевести в исходное состояние
    /// </summary>
    public virtual void ReturnToDefaultState()
    {

    }

    /// <summary>
    /// Уделить этот модуль из всех источников, с ним работающих
    /// </summary>
    public void DeleteMeFromActors()
    {
        foreach (var item in Actors)
        {
            item.nextModules.Remove(this);
        }
    }
}
