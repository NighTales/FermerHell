using UnityEngine;
using UnityEngine.Events;

public class EventInvokeModule : LogicModule
{
    [SerializeField]
    [Tooltip("Что будет происходить при активации модуля")]
    private UnityEvent logicEvent;

    /// <summary>
    /// Активировать модуль
    /// </summary>
    [ContextMenu("Активировать модуль")]
    public override void ActivateModule()
    {
        logicEvent?.Invoke();
        DeleteMeFromActors();
        Destroy(gameObject);
    }

    public override void ReturnToDefaultState()
    {

    }
}
