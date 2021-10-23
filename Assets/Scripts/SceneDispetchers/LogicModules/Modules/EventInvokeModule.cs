using UnityEngine;
using UnityEngine.Events;

public class EventInvokeModule : LogicModule
{
    [SerializeField]
    [Tooltip("��� ����� ����������� ��� ��������� ������")]
    private UnityEvent logicEvent;

    /// <summary>
    /// ������������ ������
    /// </summary>
    [ContextMenu("������������ ������")]
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
