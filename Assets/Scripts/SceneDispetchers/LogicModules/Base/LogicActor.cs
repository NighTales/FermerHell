using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������, ������� ����� ������������ ������ ������
/// </summary>
public abstract class LogicActor : LogicModule
{
    [Header("�����")]
    [Tooltip("�������, � ������� ����� ���������� ����� USE()")]
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
    /// �������� ������ ���� ��������� �������
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
                Debug.LogError("������� " + i + " ����� null. ��������, ���� ������� ������. �������� :" + gameObject.name);
            }
        }
    }

    /// <summary>
    /// ������� ��� ������ � �������� ���������
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
                Debug.LogError("������� " + i + " ����� null. ��������, ���� ������� ������. �������� :" + gameObject.name);
            }
        }
    }
}
