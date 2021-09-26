using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MyTools : MonoBehaviour
{
    protected bool MyGetComponent<T>(out T component, GameObject obj)
    {
        component = obj.GetComponent<T>();
        if (component != null)
        {
            return true;
        }

        return false;
    }

    protected void PlayThisClip(AudioSource sound, AudioClip audioClip)
    {
        if (sound.isPlaying)
        {
            sound.Stop();
        }
        sound.clip = audioClip;
        sound.Play();
    }
}
