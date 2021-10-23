using System.Collections;
using UnityEngine;

/// <summary>
/// Смена музыки МОДУЛЬ БЕЗ ВОЗМОЖНОСТИ ВОЗВРАТА
/// </summary>
[HelpURL("https://docs.google.com/document/d/1kfJsXE3iFn7baFFrVuhQTfkOs4BCT9-RLBPXc7h8gH0/edit?usp=sharing")]
public class MusicMixer : LogicModule
{
    [SerializeField, Tooltip("Аудиоклип, который сменит изначальный")] private AudioClip newMusic;

    [SerializeField, Tooltip("Источник звука")] private AudioSource source;

    [SerializeField, Tooltip("Свойство Loop после срабатываия")] private bool loop =true;

    [SerializeField, Tooltip("Резкая смена без заглушения")] private bool hardChange = false;

    public override void ReturnToDefaultState()
    {

    }

    /// <summary>
    /// Сменить музыку
    /// </summary>
    [ContextMenu("Сменить музыку")]
    public override void ActivateModule()
    {
        if (hardChange)
        {
            ChangeClip();
        }
        else
        {
            StartCoroutine(ChangeMusicCoroutine());
        }
    }

    private IEnumerator ChangeMusicCoroutine()
    {
        while(source.volume > 0)
        {
            source.volume -= Time.deltaTime;
            yield return null;
        }
        source.volume = 0;
        ChangeClip();
        while (source.volume < 1)
        {
            source.volume += Time.deltaTime;
            yield return null;
        }
        source.volume = 1;
    }
    private void ChangeClip()
    {
        if (source.isPlaying)
        {
            source.Stop();
        }
        source.clip = newMusic;
        source.Play();
        source.loop = loop;
    }

    private void OnDrawGizmos()
    {
        if(debug)
        {
            if(source == null)
            {
                Debug.LogError("Не передан источник звука в " + gameObject.name);
                return;
            }
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, source.transform.position);
        }
    }
}
