using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoMessageOrigin : MonoBehaviour
{
    [SerializeField, Tooltip("Сообщения, которые будут добавлены в очередь")]
    private List<MessageQueueItem> messages;

    [SerializeField]
    private InfoMessageDispetcher messageDispetcher;

    [ContextMenu("Отправить сообщения")]
    public void SendMessages()
    {
        foreach (var item in messages)
        {
            messageDispetcher.AddMessage(item);
        }

        Destroy(gameObject, Time.deltaTime);
    }
}
