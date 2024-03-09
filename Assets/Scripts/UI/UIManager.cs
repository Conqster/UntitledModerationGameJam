using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Variables
    [SerializeField] GameObject currentActiveObject;
    [Header("Audio")]
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Toggle musicToggle;
    [SerializeField] Toggle sfxToggle;

    // Events
    public static event Action<float, float> OnAudioUpdatedEvent;
    //public static void OnAudioUpdated(float musicVolume, float sfxVolume) { OnAudioUpdatedEvent?.Invoke(musicVolume, sfxVolume); }
    #endregion
    public void ChangeObject(GameObject objectToActivate)
    {
        currentActiveObject.SetActive(false);

        currentActiveObject = objectToActivate;

        currentActiveObject.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Start()
    {
        UpdateAudio();
    }

    public void UpdateAudio()
    {
        float musicVolume;
        float sfxVolume;

        if(musicToggle.isOn)
        {
            musicVolume = musicSlider.value;
        }
        else
        {
            musicVolume = 0;
        }

        if(sfxToggle.isOn)
        {
            sfxVolume = sfxSlider.value;
        }
        else
        {
            sfxVolume = 0;
        }

        OnAudioUpdatedEvent?.Invoke(musicVolume, sfxVolume);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
