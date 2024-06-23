using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Audio;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;
using System.Xml.Linq;

public class OptionsScript : MonoBehaviour
{
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] Volume gammaVolume;
    SensitivityManager sensitivityManager;

    [Header("The Sliders")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider gammaSlider;
    [SerializeField] Slider renderSlider;
    [SerializeField] Slider sensitivitySlider;

    ColorAdjustments colorAdjust;

    private void Awake()
    {
        gammaVolume.profile.TryGet(out colorAdjust);
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            sensitivityManager = Camera.main.GetComponent<SensitivityManager>();
        }

        LoadOptionValues();
    }

    public void SetMasterVolume(float sliderValue)
    {
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);

        //Debug.Log("Master");
    }

    public void SetBGMVolume(float sliderValue)
    {
        masterMixer.SetFloat("BGMVolume", Mathf.Log10(sliderValue) * 20);
        masterMixer.SetFloat("AmbienceVolume", Mathf.Log10(sliderValue) * 20);

        //Debug.Log("BGM");
    }

    public void SetSFXVolume(float sliderValue)
    {
        masterMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);

        //Debug.Log("SFX");
    }

    public void SetGammaValue(float sliderValue)
    {
        colorAdjust.postExposure.value = sliderValue;
    }

    public void SetRenderValue(float sliderValue)
    {
        CameraCulldistance cameraCulling = Camera.main.GetComponent<CameraCulldistance>();

        cameraCulling.SetRenderDistance(sliderValue);
    }

    public void SetSensitivityValue(float sliderValue)
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            sensitivityManager.sensitivity = sliderValue;
            sensitivityManager.UpdateCamera();
        }
    }


    public void SaveOptionValues()
    {
        float masterValue = masterSlider.value;
        float bgmValue = bgmSlider.value;
        float sfxValue = sfxSlider.value;
        float gammaValue = gammaSlider.value;
        float renderValue = renderSlider.value;
        float sensitivityValue = sensitivitySlider.value;

        PlayerPrefs.SetFloat("MasterVolumeValue", masterValue);
        PlayerPrefs.SetFloat("BGMVolumeValue", bgmValue);
        PlayerPrefs.SetFloat("SFXVolumeValue", sfxValue);
        PlayerPrefs.SetFloat("GammaValue", gammaValue);
        PlayerPrefs.SetFloat("RenderValue", renderValue);
        PlayerPrefs.SetFloat("SensitivityValue", sensitivityValue);

        LoadOptionValues();
    }

    void LoadOptionValues()
    {
        float masterValue = PlayerPrefs.GetFloat("MasterVolumeValue", 1);
        float bgmValue = PlayerPrefs.GetFloat("BGMVolumeValue", 1);
        float sfxValue = PlayerPrefs.GetFloat("SFXVolumeValue", 1);
        float gammaValue = PlayerPrefs.GetFloat("GammaValue", 0);
        float renderValue = PlayerPrefs.GetFloat("RenderValue", 550f);
        float sensitivityValue = PlayerPrefs.GetFloat("SensitivityValue", 0.5f);

        masterSlider.value = masterValue;
        bgmSlider.value = bgmValue;
        sfxSlider.value = sfxValue;
        gammaSlider.value = gammaValue;
        renderSlider.value = renderValue;
        sensitivitySlider.value = sensitivityValue;

        SetMasterVolume(masterValue);
        SetBGMVolume(bgmValue);
        SetSFXVolume(sfxValue);
        SetGammaValue(gammaValue);
        SetRenderValue(renderValue);
        SetSensitivityValue(sensitivityValue);
    }
}