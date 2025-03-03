using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider voiceVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Image muteVolumeButton;

    [SerializeField] Sprite mutedImage;
    [SerializeField] Sprite unmutedImage;

    float newVolume;
    float oldVolume;
    bool muted;

    private void Start()
    {
        muted = false;
        oldVolume = 0f;
        InitializeSliders();
    }
    
    #region Slider Controls
    public void ChangeMasterVolume ()
    {  
        newVolume = masterVolumeSlider.value;
        newVolume = Mathf.Log10(newVolume) * 20; // needed because sliders are liniar but volume is logarithmic
        audioMixer.SetFloat("MasterVolume", newVolume);
        if (muted)
        {
            muted = !muted;
            muteVolumeButton.sprite = unmutedImage;
        }
    }

    public void ChangeMusicVolume()
    {
        newVolume = musicVolumeSlider.value;
        newVolume = Mathf.Log10(newVolume) * 20; // needed because sliders are liniar but volume is logarithmic
        audioMixer.SetFloat("MusicVolume", newVolume);
        if (muted)
        {
            muted = !muted;
            muteVolumeButton.sprite = unmutedImage;
        }
    }

    public void ChangeVoiceVolume()
    {
        newVolume = voiceVolumeSlider.value;
        newVolume = Mathf.Log10(newVolume) * 20; // needed because sliders are liniar but volume is logarithmic
        audioMixer.SetFloat("VoiceVolume", newVolume);
        if (muted)
        {
            muted = !muted;
            muteVolumeButton.sprite = unmutedImage;
        }
    }

    public void ChangeSfxVolume()
    {
        newVolume = sfxVolumeSlider.value;
        newVolume = Mathf.Log10(newVolume) * 20; // needed because sliders are liniar but volume is logarithmic
        audioMixer.SetFloat("SFXVolume", newVolume);
        if (muted)
        {
            muted = !muted;
            muteVolumeButton.sprite = unmutedImage;
        }
    }
    #endregion

    public void MuteSwitch()
    {
        if (muted)
        {
            //change button sprite to mute sprite
            muteVolumeButton.sprite = unmutedImage;
            audioMixer.SetFloat("MasterVolume", oldVolume);
        }
        else
        {
            //change button sprite to unmute sprite
            muteVolumeButton.sprite = mutedImage;
            audioMixer.GetFloat("MasterVolume", out oldVolume);
            Debug.Log(oldVolume);
            audioMixer.SetFloat("MasterVolume", -80f);
        }
        muted = !muted;
    }

    private void InitializeSliders()
    {
        float volume;

        // Master Volume
        if (audioMixer.GetFloat("MasterVolume", out volume))
        {
            masterVolumeSlider.value = Mathf.Pow(10, volume / 20f);
            muted = volume <= -80f;
        }

        // Music Volume
        if (audioMixer.GetFloat("MusicVolume", out volume))
        {
            musicVolumeSlider.value = Mathf.Pow(10, volume / 20f);
        }

        // SFX Volume
        if (audioMixer.GetFloat("SFXVolume", out volume))
        {
            sfxVolumeSlider.value = Mathf.Pow(10, volume / 20f);
        }

        // Voice Volume
        if (audioMixer.GetFloat("VoiceVolume", out volume))
        {
            voiceVolumeSlider.value = Mathf.Pow(10, volume / 20f);
        }

        // Mute Button
        muteVolumeButton.sprite = muted ? mutedImage : unmutedImage;
    }
}
