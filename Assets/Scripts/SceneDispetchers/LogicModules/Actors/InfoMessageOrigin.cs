using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoMessageOrigin : MonoBehaviour
{
    [SerializeField, Tooltip("���������, ������� ����� ��������� � �������")]
    private List<MessageQueueItem> messages;

    [SerializeField]
    private InfoMessageDispetcher messageDispetcher;

    [ContextMenu("��������� ���������")]
    public void SendMessages()
    {
        foreach (var item in messages)
        {
            messageDispetcher.AddMessage(item);
        }

        Destroy(gameObject, Time.deltaTime);
    }
}
