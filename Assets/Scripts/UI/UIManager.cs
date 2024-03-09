using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Variables
    [SerializeField] protected GameObject currentActiveObject;
    [SerializeField] protected GameObject rootMenu;
    [Header("Audio")]
    [SerializeField] GameObject musicToggleAndSlider;
    [SerializeField] GameObject sfxToggleAndSlider;

    Slider musicSlider;
    Slider sfxSlider;
    Toggle musicToggle;
    Toggle sfxToggle;

    // Events
    public static event Action<float, float> OnAudioUpdatedEvent;
    //public static void OnAudioUpdated(float musicVolume, float sfxVolume) { OnAudioUpdatedEvent?.Invoke(musicVolume, sfxVolume); }
    #endregion
    public void ChangeObject(GameObject objectToActivate)
    {
        if(currentActiveObject != null)
            currentActiveObject.SetActive(false);

        currentActiveObject = objectToActivate;

        currentActiveObject.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    protected virtual void Start()
    {
        musicSlider = musicToggleAndSlider.GetComponentInChildren<Slider>();
        musicToggle = musicToggleAndSlider.GetComponentInChildren<Toggle>();

        sfxSlider = sfxToggleAndSlider.GetComponentInChildren <Slider>();
        sfxToggle = sfxToggleAndSlider.GetComponentInChildren <Toggle>();
        UpdateAudio();
    }

    protected virtual void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(currentActiveObject != rootMenu)
            {
                ChangeObject(rootMenu);
            }
        }
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
