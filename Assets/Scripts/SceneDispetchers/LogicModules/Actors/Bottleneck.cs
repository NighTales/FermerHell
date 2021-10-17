using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Данный класс используется, когда нужно, чтобы все actors сработали перед тем, как сигнал пошёл дальше.
/// Одноразовый модуль!
/// </summary>
[HelpURL("https://docs.google.com/document/d/1elo1YsFfbbhLMpHGXa8g3z7TVpwf1LCiTqJkcKCFaak/edit?usp=sharing")]
public class Bottleneck : LogicActor {

    [SerializeField]
    private List<LogicModule> previousModules;

    /// <summary>
    /// Проверить, все ли источники подают сигнал. В случае полной подачи активировать следующие модули
    /// </summary>
    public override void ActivateModule()
    {
        StartCoroutine(CheckAllActors(1));
    }

    private IEnumerator CheckAllActors(float delay)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < previousModules.Count; i++)
        {
            if(previousModules[i] == null || previousModules[i].used)
            {
                previousModules.RemoveAt(i);
                i--;
            }
        }

        if (previousModules.Count == 0)
        {
            ActivateAllNextModules();
            Destroy(gameObject, Time.deltaTime);
        }
    }

    public override void ReturnToDefaultState()
    {

    }
    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.3f);
            for (int i = 0; i < nextModules.Count; i++)
            {
                if (nextModules[i] != null)
                {
                    Gizmos.DrawLine(transform.position, nextModules[i].transform.position);
                }
                else
                {
                    Debug.LogWarning("Элемент " + i + " равен null. Вероятно, была утеряна ссылка. Источник :" + gameObject.name);
                }
            }
        }
    }
}

