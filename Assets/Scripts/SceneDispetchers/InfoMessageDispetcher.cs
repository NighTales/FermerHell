using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoMessageDispetcher : MonoBehaviour
{
    [Tooltip("Панель сообщения сообщения")] public GameObject messageTip;
    [Tooltip("Всплывающий текст сообщения")] public Text messageText;
    [Tooltip("Время появления текста"), Range(0, 2)] public float startTime = 1;
    [Tooltip("Время угасания текста"), Range(0, 2)] public float endTime = 1;
    [Tooltip("Задержка между двумя сообщениями в очереди")] public float messagesDelayTime = 7;

    private List<MessageQueueItem> queueItems;

    [HideInInspector] public bool inPause;

    private bool alreadyWorks;
    private float delayTimer;

    private void Awake()
    {
        queueItems = new List<MessageQueueItem>();
        messageText.CrossFadeAlpha(0, 0, true);
        messageTip.SetActive(false);
        delayTimer = 0;
        alreadyWorks = false;
    }

    public void AddMessage(MessageQueueItem messageQueueItem)
    {
        if(!queueItems.Contains(messageQueueItem))
        {
            queueItems.Add(messageQueueItem);

            if (!alreadyWorks)
            {
                messageTip.SetActive(true);
                alreadyWorks = true;
                StartCoroutine(ShowMessages());
            }
        }
    }

    private IEnumerator ShowMessages()
    {
        while (queueItems.Count > 0)
        {
            delayTimer = 0;
            MessageQueueItem item = queueItems[0];
            messageText.text = item.messageText;
            messageText.CrossFadeAlpha(1, startTime, true);

            yield return new WaitForSeconds(startTime);

            while (delayTimer < messagesDelayTime)
            {
                if(!inPause)
                {
                    delayTimer += Time.deltaTime;

                    if(item.keys?.Count != 0)
                    {
                        if(CheckButton(item))
                        {
                            break;
                        }
                    }

                    yield return null;
                }
            }

            messageText.CrossFadeAlpha(0, endTime, true);

            yield return new WaitForSeconds(endTime);

            queueItems.Remove(queueItems[0]);
        }

        yield return new WaitForSeconds(endTime);

        alreadyWorks = false;
        messageTip.SetActive(false);
    }

    private bool CheckButton(MessageQueueItem item)
    {
        for (int i = 0; i < item.keys.Count; i++)
        {
            if(Input.GetKeyDown(item.keys[i]))
            {
                return true;
            }
        }
        return false;
    }
}

[System.Serializable]
public class MessageQueueItem
{
    [Tooltip("Текст сообщения, которое будет появляться")] public string messageText;
    [Tooltip("Кнопки, которые нужно нажать, чтобы сообщение скрылось само")]
    public List<KeyCode> keys;
}

