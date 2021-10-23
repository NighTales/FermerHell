using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarierBuilder : MonoBehaviour
{
    [SerializeField] private List<GameObject> ItemsForBariers;
    [SerializeField] private List<GameObject> ItemsdForColumns;
    [SerializeField, Min(0)] private float step;
    [SerializeField, Min(0)] private int count;
    [SerializeField] bool debug;

    private Transform lastOrigin;
    private List<Transform> previousOrigins;
    private Vector3 position;

#if UNITY_EDITOR

    [ContextMenu("Завершить работу")]
    public void Accept()
    {
        for (int i = 0; i < previousOrigins.Count; i++)
        {
            previousOrigins[i].parent = transform;
        }
        lastOrigin.parent = transform;
        DestroyImmediate(this);
    }

    [ContextMenu("Добавить указанное количество")]
    public void AddRange()
    {
        for (int i = 0; i < count; i++)
        {
            AddNew();
        }
    }

    [ContextMenu("Удалить всё")]
    public void RemoveAll()
    {
        lastOrigin = null;
        previousOrigins = null;
        DestroyImmediate(transform.GetChild(0).gameObject);
    }

    [ContextMenu("Добавить звено")]
    public void AddNew()
    {
        if(previousOrigins == null)
        {
            previousOrigins = new List<Transform>();
        }

        if (lastOrigin == null)
        {
            lastOrigin = transform;
            position = transform.position;
        }
        else
        {
            previousOrigins.Add(lastOrigin);
            position = lastOrigin.position + lastOrigin.forward * step;
        }

        if (ItemsdForColumns?.Count != 0)
        {
            lastOrigin = Instantiate(ItemsdForColumns[Random.Range(0, ItemsdForColumns.Count)],
                position, Quaternion.identity, lastOrigin).transform;
            lastOrigin.localRotation = Quaternion.Euler(Vector3.zero);
            Instantiate(ItemsForBariers[Random.Range(0, ItemsdForColumns.Count)],
                position, Quaternion.identity, lastOrigin).transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
        else
        {
            lastOrigin = Instantiate(ItemsForBariers[Random.Range(0, ItemsdForColumns.Count)],
                position, Quaternion.identity, lastOrigin).transform;
            lastOrigin.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    [ContextMenu("Удалить звено")]
    public void RemoveLast()
    {
        if(lastOrigin != null)
        {
            DestroyImmediate(lastOrigin.gameObject);
            if(previousOrigins?.Count > 0)
            {
                lastOrigin = previousOrigins[previousOrigins.Count-1];
                previousOrigins.RemoveAt(previousOrigins.Count - 1);
            }
        }
    }


    private void OnDrawGizmos()
    {
        if(debug)
        {
            Gizmos.color = Color.cyan;

            if (lastOrigin == null)
            {
                position = transform.position;
            }
            else
            {
                position = lastOrigin.position + lastOrigin.forward * step;
            }

            Gizmos.DrawSphere(position, 0.3f);

            Vector3 targetPos = position + (lastOrigin == null ? transform.forward : lastOrigin.forward) * step * count;

            Gizmos.DrawLine(position, targetPos);

            Gizmos.DrawSphere(targetPos, 0.3f);
        }
    }
#endif
}
