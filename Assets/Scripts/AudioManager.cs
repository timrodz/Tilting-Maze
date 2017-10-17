using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    
    public bool MUTE = false;

    [Header("Audio sources")]
    [SerializeField] private AudioSource m_MusicSource;
    [SerializeField] private AudioSource m_EffectSource;

    public Sound[] m_Sounds;

    public static void PlayEffect(ClipType _clipType)
    {
        if (AudioManager.Instance.MUTE)
        {
            return;
        }
        
        for (int i = 0; i < AudioManager.Instance.m_Sounds.Length; i++)
        {
            if (_clipType == AudioManager.Instance.m_Sounds[i].clipType)
            {
                AudioManager.Instance.m_EffectSource.pitch = AudioManager.Instance.m_Sounds[i].pitch;
                AudioManager.Instance.m_EffectSource.PlayOneShot(AudioManager.Instance.m_Sounds[i].clip, AudioManager.Instance.m_Sounds[i].volume);
            }
        }
    }

    public static void PlayMusic(ClipType _clipType)
    {
        if (AudioManager.Instance.MUTE)
        {
            return;
        }
        
        for (int i = 0; i < AudioManager.Instance.m_Sounds.Length; i++)
        {
            if (_clipType == AudioManager.Instance.m_Sounds[i].clipType)
            {
                AudioManager.Instance.m_EffectSource.loop = AudioManager.Instance.m_Sounds[i].loop;
                AudioManager.Instance.m_MusicSource.pitch = AudioManager.Instance.m_Sounds[i].pitch;
                AudioManager.Instance.m_MusicSource.PlayOneShot(AudioManager.Instance.m_Sounds[i].clip, AudioManager.Instance.m_Sounds[i].volume);
            }
        }
    }
    
    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    void OnValidate()
    {
        if (MUTE)
        {
            m_MusicSource.volume = 0;
        }
    }

}