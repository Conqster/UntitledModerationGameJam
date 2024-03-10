using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get; private set;}
    [SerializeField] Sound[] sounds;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioMixerGroup musicAudioMixerGroup;
    [SerializeField] AudioMixerGroup SoundEffectsAudioMixerGroup;

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
            //s.source.pitch = s.pitch;

            if (s.music)
            {
                //s.source.volume = s.volume * musicVolumeMultiplier;
                s.source.outputAudioMixerGroup = musicAudioMixerGroup;
            }
            else
            {
                //s.source.volume = s.volume * sfxVolumeMultiplier;
                s.source.outputAudioMixerGroup = SoundEffectsAudioMixerGroup;
            }

            s.source.loop = s.loop;
        }

    }

    void UpdateAudio(float musicVolume, float sfxVolume)
    {
        musicVolumeMultiplier = musicVolume;
        sfxVolumeMultiplier = sfxVolume;

        audioMixer.SetFloat("Music Volume", musicVolume);
        audioMixer.SetFloat("Sound Effects Volume", sfxVolume);

    }

    public static void PlaySound(string name)
    {
        Sound s = Array.Find(Instance.sounds, s => s.name == name);
        if (s == null) return;

        s.source.Play();
    }
}
