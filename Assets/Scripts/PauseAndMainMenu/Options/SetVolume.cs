using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    [SerializeField] AudioMixer masterMixer;

    public void SetMasterVolume(float sliderValue)
    {
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);

        Debug.Log("Master");
    }

    public void SetBGMVolume(float sliderValue)
    {
        masterMixer.SetFloat("BGMVolume", Mathf.Log10(sliderValue) * 20);

        Debug.Log("BGM");
    }

    public void SetSFXVolume(float sliderValue)
    {
        masterMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);

        Debug.Log("SFX");
    }
}
