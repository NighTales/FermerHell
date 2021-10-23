using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Данный модуль применяется для отрисовки обратного отсчёта таймеров.
/// </summary>
[HelpURL("https://docs.google.com/document/d/1gQ_vJVpk89QuWdROQjf08JViKf99TlnCcyzoEMrPbQI/edit?usp=sharing")]
public class TimerUI : MonoBehaviour
{
    [SerializeField, Tooltip("UI-текст, где будет отображаться обратный отсчёт")] private Text timerText;
    [SerializeField, Tooltip("Панель таймера")] private GameObject timerPanel;

    private void Start()
    {
        CloseTimer();
    }

    public void DrawTimerValue(string value)
    {
        if(!timerPanel.activeSelf)
        {
            timerPanel.SetActive(true);
        }
        timerText.text = value;
    }
    public void CloseTimer()
    {
        timerPanel.SetActive(false);
        timerText.text = string.Empty;
    }
}
