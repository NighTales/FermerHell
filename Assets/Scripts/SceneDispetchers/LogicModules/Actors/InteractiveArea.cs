using UnityEngine;

/// <summary>
/// Зона, при входе в которую LogicModuleReactor будет запускать импульс
/// </summary>
[RequireComponent(typeof(Collider))]
[HelpURL("https://docs.google.com/document/d/1rNTvU6cSC5O2jJi-u_8kSjc9r6-mcXEnwI3DBH8gFAA/edit?usp=sharing")]
public class InteractiveArea : LogicActor
{
    [Header("Настройки TriggerReactor")]
    [Tooltip("Реагировать только на вход")] public bool enterOnly;
    [Tooltip("Отключаться после первого срабатывания")] public bool once;

    private void Start()
    {
        tag = LogicModuleReactor.interactiveTag;
    }

    /// <summary>
    /// Передать сигнал следующим модулям
    /// </summary>
    [ContextMenu("Активировать модуль")]
    public override void ActivateModule()
    {
        ActivateAllNextModules();
        used = !used;
        if (once)
        {
            DeleteMeFromActors();
            Destroy(gameObject, 1);
//            gameObject.SetActive(false);
        }
    }

    public override void ReturnToDefaultState()
    {
        used = false;
        foreach (var item in nextModules)
        {
            item?.ReturnToDefaultState();
        }
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, 0.3f);
            for (int i = 0; i < nextModules.Count; i++)
            {
                if (nextModules[i] != null)
                {
                    Gizmos.DrawLine(transform.position, nextModules[i].transform.position);
                }
                else
                {
                    Debug.LogError("Элемент " + i + " равен null. Вероятно, была утеряна ссылка. Источник :" + gameObject.name);
                }
            }
        }
    }
}