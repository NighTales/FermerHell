using UnityEngine;


/// <summary>
/// даёт сигнал случайному модулю из набора
/// </summary>
[HelpURL("https://docs.google.com/document/d/1elo1YsFfbbhLMpHGXa8g3z7TVpwf1LCiTqJkcKCFaak/edit?usp=sharing")]
public class RandomActionModule : LogicActor
{
    [Tooltip("Одноразовый")] public bool once;
    [Tooltip("С удалением использованного модуля")] public bool withRemoving;

    public override void ReturnToDefaultState()
    {
        used = false;
    }
    public override void ActivateModule()
    {
        if (nextModules != null && nextModules.Count > 0)
        {
            used = true;

            int index = Random.Range(0, nextModules.Count);
            nextModules[index].ActivateModule();

            if (withRemoving)
            {
                nextModules.RemoveAt(index);
            }

            if(once)
            {
                DeleteMeFromActors();
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.3f);
            for (int i = 0; i < nextModules.Count; i++)
            {
                if (nextModules[i] != null)
                {
                    Gizmos.DrawLine(transform.position, nextModules[i].transform.position);
                    Gizmos.DrawSphere(Vector3.Lerp(transform.position, nextModules[i].transform.position,
                        Vector3.Distance(nextModules[i].transform.position, transform.position) / 2), 0.3f);
                }
                else
                {
                    Debug.LogError("Элемент " + i + " равен null. Вероятно, была утеряна ссылка. Источник :" + gameObject.name);
                }
            }
        }
    }
}
