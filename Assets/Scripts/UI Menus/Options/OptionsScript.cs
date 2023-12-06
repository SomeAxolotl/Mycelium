using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Xml.Linq;

public class OptionsScript : MonoBehaviour
{
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] Volume gammaVolume;

    [Header("The Sliders")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider gammaSlider;

    ColorAdjustments colorAdjust;

    private void Awake()
    {
        gammaVolume.profile.TryGet(out colorAdjust);

        LoadOptionValues();
    }

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

    public void SetGammaValue(float sliderValue)
    {
        colorAdjust.postExposure.value = sliderValue;
    }

    public void SaveOptionValues()
    {
        float masterValue = masterSlider.value;
        float bgmValue = bgmSlider.value;
        float sfxValue = sfxSlider.value;
        float gammaValue = gammaSlider.value;

        PlayerPrefs.SetFloat("MasterVolumeValue", masterValue);
        PlayerPrefs.SetFloat("BGMVolumeValue", bgmValue);
        PlayerPrefs.SetFloat("SFXVolumeValue", sfxValue);
        PlayerPrefs.SetFloat("GammaValue", gammaValue);

        LoadOptionValues();
    }

    void LoadOptionValues()
    {
        float masterValue = PlayerPrefs.GetFloat("MasterVolumeValue", 1);
        float bgmValue = PlayerPrefs.GetFloat("BGMVolumeValue", 1);
        float sfxValue = PlayerPrefs.GetFloat("SFXVolumeValue", 1);
        float gammaValue = PlayerPrefs.GetFloat("GammaValue", 0);

        masterSlider.value = masterValue;
        bgmSlider.value = bgmValue;
        sfxSlider.value = sfxValue;
        gammaSlider.value = gammaValue;

        SetMasterVolume(masterValue);
        SetBGMVolume(bgmValue);
        SetSFXVolume(sfxValue);
        SetGammaValue(gammaValue);
    }
}
