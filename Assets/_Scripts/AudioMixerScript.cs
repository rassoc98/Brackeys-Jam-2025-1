using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerScript : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;

    [SerializeField]
    private Slider Master, BGM, SFX;

    private void Start()
    {
        InitializePlayerPrefs();
        GetPlayerPrefs();
    }

    private void InitializePlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("MASTER"))
            PlayerPrefs.SetFloat("MASTER", 0.8f);

        if (!PlayerPrefs.HasKey("BGM"))
            PlayerPrefs.SetFloat("BGM", 0.8f);

        if (!PlayerPrefs.HasKey("SFX"))
            PlayerPrefs.SetFloat("SFX", 0.8f);
    }

    private void GetPlayerPrefs()
    {
        // set sliders to correct values
        Master.value = PlayerPrefs.GetFloat("MASTER");
        BGM.value = PlayerPrefs.GetFloat("BGM");
        SFX.value = PlayerPrefs.GetFloat("SFX");

        // set volumes in audiomixer
        SetMasterVolume(Master.value);
        SetBGMVolume(BGM.value);
        SetSFXVolume(SFX.value);
    }

    public void SetMasterVolume(float value)
    {
        PlayerPrefs.SetFloat("MASTER", value);
        mixer.SetFloat("MASTER", MathF.Log10(value) * 20f); // approx conversion of linear slider to logarithmic dB
    }

    public void SetBGMVolume(float value)
    {
        PlayerPrefs.SetFloat("BGM", value);
        mixer.SetFloat("BGM", MathF.Log10(value) * 20f);

    }

    public void SetSFXVolume(float value)
    {
        PlayerPrefs.SetFloat("SFX", value);
        mixer.SetFloat("SFX", MathF.Log10(value) * 20f);
    }
}
