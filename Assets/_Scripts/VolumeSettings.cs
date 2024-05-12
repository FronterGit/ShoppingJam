using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour {
    [SerializeField] private AudioMixer mixer;

    [SerializeField] private Slider slider;


    private void Start() {
        SetMainVolume();
        SetSFXVolume();
    }

    public void SetMainVolume() {
        float volume = slider.value;
        mixer.SetFloat("music", Mathf.Log10(volume) * 20);
    }
    
    public void SetSFXVolume() {
        float volume = slider.value;
        mixer.SetFloat("sfx", Mathf.Log10(volume) * 20);

    }
}