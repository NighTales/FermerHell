using System.Collections;
using UnityEngine;

/// <summary>
/// При активации начинает в случайном порядке удалять дочерние объекты с  интервалом в destroyDelay секунд.
/// Удаляет, пока количество оставшихся объектов привышает minimalDestroyingObjectsCount, после чего удаляется сам.
/// ОДНОРАЗОВЫЙ МОДУЛЬ
/// </summary>
[HelpURL("https://docs.google.com/document/d/1RMamVxE-yUpSfsPD_dEa4-Ak1qu6NTo83qY1O4XLxUY/edit?usp=sharing")]
public class DestroyModule : LogicModule
{
    [Tooltip("Задержка между удалениями"), SerializeField, Min(0)]
    private float destroyDelay;

    [Tooltip("Количество дочерних объектов, после которого будет удаляться модуль"), SerializeField, Min(0)]
    private int minimalDestroyingObjectsCount;

    private Transform myTransform;

    private void Awake()
    {
        myTransform = transform;
    }

    /// <summary>
    /// Начать удаление дочерних объектов
    /// </summary>
    [ContextMenu("Начать удаление объектов")]
    public override void ActivateModule()
    {
        StartCoroutine(DestroyRandomChildObjectCoroutine());
    }

    private IEnumerator DestroyRandomChildObjectCoroutine()
    {
        while (myTransform.childCount > minimalDestroyingObjectsCount)
        {
            int index = Random.Range(0, myTransform.childCount - 1);
            Destroy(myTransform.GetChild(index).gameObject);
            yield return new WaitForSeconds(destroyDelay);
        }
        DeleteMeFromActors();
        Destroy(gameObject, Time.deltaTime);
    }
}
