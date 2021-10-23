using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Этот модуль позволяет менять значение у аниматоров, которые имеют параметр типа bool.
/// Хорошо подходит для особых дверей, ламп, и других объектов, имеющих два состояния,
/// которые нельзя реализовать другими модулями.
/// Метод Use() будет переключать значение параметра аниматора с именем parameterName
/// </summary>
[HelpURL("https://docs.google.com/document/d/1kqO3xYCQ1up9p3t4TUZaJ7MWjeZNVtIPAS2hEu_xe7E/edit?usp=sharing")]
public class BoolAnimActivator : LogicModule
{
    [Header("Настройки AnimActivator")]
    [Tooltip("Аниматор должен содержать параметр Active (bool)"), SerializeField]
    private List<Animator> animObjects;

    [Tooltip("Название параметра аниматора, с которым нужно работать"), SerializeField]
    private string paramterName = "Active";

    [Tooltip("Начальное состояние"), SerializeField]
    private bool startActive;

    private bool currentActive;

    private void Start()
    {
        ReturnToDefaultState();
    }

    [ContextMenu("Переключить модуль")]
    public override void ActivateModule()
    {
        currentActive = !currentActive;
        SetActiveForAll(currentActive);
        used = !used;
    }
    [ContextMenu("Вернуть модуль в исходное состояние")]
    public override void ReturnToDefaultState()
    {
        used = false;
        currentActive = startActive;
        SetActiveForAll(startActive);
    }
    public void SetActiveForAll(bool value)
    {
        for (int i = 0; i < animObjects.Count; i++)
        {
            if(animObjects[i] != null)
            {
                animObjects[i].SetBool(paramterName, value);
            }
            else
            {
                Debug.LogError("Элемент " + i + " равен null. Вероятно, была утеряна ссылка. Источник :"
                    + gameObject.name);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position, 0.3f);
            for (int i = 0; i < animObjects.Count; i++)
            {
                if (animObjects[i] != null)
                {
                    Gizmos.DrawLine(transform.position, animObjects[i].transform.position);
                }
                else
                {
                    Debug.LogError("Элемент " + i + " равен null. Вероятно, была утеряна ссылка. Источник :"
                        + gameObject.name);
                }
            }
        }
    }
}
