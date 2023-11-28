using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SetGamma : MonoBehaviour
{
    [SerializeField] Volume gammaVolume;

    ColorAdjustments colorAdjust;

    private void Awake()
    {
        gammaVolume.profile.TryGet(out colorAdjust);
    }

    public void SetGammaValue(float sliderValue)
    {
        colorAdjust.postExposure.value = sliderValue;
    }
}
