using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;
    public AudioSource musicSource;
    public AudioClip[] audioClips;

    public float sfxVolume = 1f;
    public Slider sfxVolumeSlider;
    public float musicVolume = 1f;
    public Slider musicVolumeSlider;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(string clipName, bool music = false)
    {
        if (audioSource == null || musicSource == null) return;
        if (music)
        {
            musicSource.Play();
        }
        else
        {
            foreach (AudioClip clip in audioClips)
            {
                if (clip.name == clipName)
                {
                    audioSource.clip = clip;
                    audioSource.Play();
                    return;
                }
            }
        }
    }

    public void SetSFXVolume()
    {
        if (sfxVolumeSlider == null) sfxVolumeSlider = GameObject.FindWithTag("SFXSlider").GetComponent<Slider>();
        sfxVolume = sfxVolumeSlider.value;
        audioSource.volume = sfxVolume;
    }

    public void SetMusicVolume()
    {
        if (musicVolumeSlider == null) musicVolumeSlider = GameObject.FindWithTag("MusicSlider").GetComponent<Slider>();
        musicVolume = musicVolumeSlider.value;
        musicSource.volume = musicVolume;
    }
}