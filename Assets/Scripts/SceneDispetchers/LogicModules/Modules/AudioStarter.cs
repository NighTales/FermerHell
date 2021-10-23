using UnityEngine;

/// <summary>
/// Запускает звук
/// </summary>
[RequireComponent(typeof(AudioSource))]
[HelpURL("https://docs.google.com/document/d/1KvZrJaeB3djcaFz6FYaxvHR5gRoObAMHyKPD-WQJ9HY/edit?usp=sharing")]
public class AudioStarter : LogicModule
{
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Loop = true - Включить/выключить динамик
    /// Loop = false - Включить динамик
    /// </summary>
    [ContextMenu("Переключить динамик")]
    public override void ActivateModule()
    {
        if (source.loop)
        {
            used = true;
            if (source.isPlaying)
            {
                source.Stop();
                used = false;
                return;
            }
        }
        source.Play();
    }

    /// <summary>
    /// Выключить динамик
    /// </summary>
    [ContextMenu("Выключить динамик")]
    public override void ReturnToDefaultState()
    {
        if (source.isPlaying)
        {
            used = false;
            source.Stop();
        }
    }
}
