using System;
using UnityEngine;


/// <summary>
/// когда данный модуль активируется, то запускается обратный отсчёт, после которого он передаёт сигнал следующим модулям
/// </summary>
[HelpURL("https://docs.google.com/document/d/1gQ_vJVpk89QuWdROQjf08JViKf99TlnCcyzoEMrPbQI/edit?usp=sharing")]
public class Timer : LogicActor
{
    [Space(20), Tooltip("Время в секундах, через которое сигнал будет передан другим модулям"), Range(0, 3600)]
    [SerializeField]
    private float time;

    [SerializeField]
    [Tooltip("Запускать сразу")]
    private bool activeOnStart = false;
    
    [Tooltip("Отрисовывать время")]
    [SerializeField]
    private bool useTimerUI;


    private event Action<string> timerChanged;
    private event Action stopTimerEvent;

    private float currentTime;
    private string lastTimerString = string.Empty;

    private void Start()
    {
        if(useTimerUI)
        {
            TimerUI timerUI = FindObjectOfType<TimerUI>();

            timerChanged += timerUI.DrawTimerValue;
            stopTimerEvent += timerUI.CloseTimer;
        }

        if(activeOnStart)
        {
            ActivateModule();
        }
    }
    private void Update()
    {
        if(used && activeOnStart)
        {
            if(currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                if(useTimerUI)
                    DrawTimerInvoke();
            }
            else
            {
                used = true;
                activeOnStart = false;
                ActivateAllNextModules();
                currentTime = 0;
                if(useTimerUI)
                    stopTimerEvent?.Invoke();
                ReturnToDefaultState();
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.white;
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

    /// <summary>
    /// Запустить таймер
    /// </summary>
    public override void ActivateModule()
    {
        activeOnStart = true;
        used = true;
        currentTime = time;
    }
    public override void ReturnToDefaultState()
    {
        currentTime = time;
        used = false;
        currentTime = 0;
    }

    private void DrawTimerInvoke()
    {
        string currentTimerString = (int)(currentTime / 60) + ":" + (int)(currentTime % 60);
        
        if(!currentTimerString.Equals(lastTimerString))
        {
            lastTimerString = currentTimerString;
            timerChanged?.Invoke(lastTimerString);
        }
    }
}
