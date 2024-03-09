using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get; private set;}
    [SerializeField] Sound[] sounds;

    float musicVolumeMultiplier = 1f;
    float sfxVolumeMultiplier = 1f;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        InitialiseAudio();
        UIManager.OnAudioUpdatedEvent += UpdateAudio;
        PlaySound("Test Audio");
    }

    public void InitialiseAudio()
    {
        foreach(var s in sounds)
        {
            if(s.source == null)
                s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.pitch = s.pitch;

            if (s.music)
            {
                s.source.volume = s.volume * musicVolumeMultiplier;
            }
            else
            {
                s.source.volume = s.volume * sfxVolumeMultiplier;
            }

            s.source.loop = s.loop;
        }

    }

    void UpdateAudio(float musicVolume, float sfxVolume)
    {
        musicVolumeMultiplier = musicVolume;
        sfxVolumeMultiplier = sfxVolume;

        InitialiseAudio();
    }

    public static void PlaySound(string name)
    {
        Sound s = Array.Find(Instance.sounds, s => s.name == name);
        if (s == null) return;

        s.source.Play();
    }
}
