using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class DesignTracker : MonoBehaviour
{
    //Blendshape Management
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    private Mesh skinnedMesh;
    public UnityEngine.Color capColor;
    public UnityEngine.Color bodyColor;
    private bool blendCoroutineRunning = false;
    private List<int> UpdateTypes;
    private int LevelCap = 15-1;                    //Blendshapes are based off starting at level 0, so this reflects the value of the max level minus one
    private float sentienceWeight = 0;
    private float primalWeight = 0;
    private float vitalityWeight = 0;
    private float speedWeight = 0;
    public int MouthOption;
    public int EyeOption;
    public Texture2D EyeTexture;
    public Texture2D MouthTexture;
    [SerializeField] private bool PlaySFX = true;
    private bool sfxEnabled=false;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip secretHighLevelSoundClip;

    [SerializeField] Texture2D blinkTexture;
    public bool canBlink = true;

    //Ryan's nutrient glow stuff
    private Coroutine nutrientGlow;

    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            UpdateColorsAndTexture();                   //<---- taken care of by SporeManager.cs in the carcass
        }

        StartCoroutine(RandomBlinks());
    }

    public void UpdateBlendshape(int sentienceLevel, int primalLevel, int vitalityLevel, int speedLevel)
    {
        sentienceLevel--;
        primalLevel--;
        vitalityLevel--;
        speedLevel--;
        
        //New Level Cap, if any are over 15
        if(Mathf.Max(sentienceLevel,primalLevel,vitalityLevel,speedLevel)>LevelCap)
            LevelCap = Mathf.Max(sentienceLevel,primalLevel,vitalityLevel,speedLevel);

        int totalLevel = sentienceLevel + primalLevel + vitalityLevel + speedLevel;
        if(totalLevel<=LevelCap){
            //map the weight for Sentience
            sentienceWeight = sentienceLevel * 100f/LevelCap;

            //map the weight for Primal
            primalWeight = primalLevel * 100f/LevelCap;

            //map the weight for Vitality
            vitalityWeight = vitalityLevel * 100f/LevelCap;

            //map the weight for Speed
            speedWeight = speedLevel * 100f/LevelCap;
        }
        //Secret Ultimate Mode, if you're close to capping all 4 skills.
        else if(sentienceLevel>=15 && speedLevel>=15 && primalLevel >=15 && vitalityLevel >= 15){
            //if (sfxEnabled == true) audioSource.PlayOneShot(secretHighLevelSoundClip);
            //map the weight for Sentience
            sentienceWeight = -20;

            //map the weight for Primal
            primalWeight = -20;

            //map the weight for Vitality
            vitalityWeight = -20;

            //map the weight for Speed
            speedWeight = -20;
        }

        //If spore level total is above the level cap, adjust based on the level total. No cap can go past 100 ever.
        else{
            //map the weight for Sentience
            sentienceWeight = sentienceLevel*sentienceLevel/totalLevel * 100f/LevelCap;

            //map the weight for Primal
            primalWeight = primalLevel*primalLevel/totalLevel * 100f/LevelCap;

            //map the weight for Vitality
            vitalityWeight = vitalityLevel*vitalityLevel/totalLevel * 100f/LevelCap;

            //map the weight for Speed
            speedWeight = speedLevel*speedLevel/totalLevel * 100f/LevelCap;
        }
        if(blendCoroutineRunning) StopAllCoroutines();
        StartCoroutine(fadeBlend(sentienceWeight, primalWeight, vitalityWeight, speedWeight));
        sfxEnabled = PlaySFX;
    }

    IEnumerator fadeBlend(float sentience, float primal, float vitality, float speed)
    {   
        blendCoroutineRunning =  true;
        float startingSentience = skinnedMeshRenderer.GetBlendShapeWeight(2);
        float startingPrimal = skinnedMeshRenderer.GetBlendShapeWeight(3);
        float startingVitality = skinnedMeshRenderer.GetBlendShapeWeight(1);
        float startingSpeed = skinnedMeshRenderer.GetBlendShapeWeight(0);
        //Adjusting Sentience
        if(startingSentience<sentience)
            while(skinnedMeshRenderer.GetBlendShapeWeight(2) < sentience)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(2, skinnedMeshRenderer.GetBlendShapeWeight(2) + (sentience-startingSentience)/10f);
                yield return new WaitForSeconds(0.02f);
            }
        else
            while(skinnedMeshRenderer.GetBlendShapeWeight(2) > sentience)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(2, skinnedMeshRenderer.GetBlendShapeWeight(2) + (sentience-startingSentience)/10f);
                yield return new WaitForSeconds(0.02f);
            }
        //Adjusting Primal
        if(startingPrimal<primal)
            while(skinnedMeshRenderer.GetBlendShapeWeight(3) < primal)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(3, skinnedMeshRenderer.GetBlendShapeWeight(3) + (primal-startingPrimal)/10f);
                yield return new WaitForSeconds(0.02f);
            }
        else
            while(skinnedMeshRenderer.GetBlendShapeWeight(3) > primal)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(3, skinnedMeshRenderer.GetBlendShapeWeight(3) + (primal-startingPrimal)/10f);
                yield return new WaitForSeconds(0.02f);
            }
        //Adjusting Vitality
        if(startingVitality<vitality)
            while(skinnedMeshRenderer.GetBlendShapeWeight(1) < vitality)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(1, skinnedMeshRenderer.GetBlendShapeWeight(1) + (vitality-startingVitality)/10f);
                yield return new WaitForSeconds(0.02f);
            }
        else
            while(skinnedMeshRenderer.GetBlendShapeWeight(1) > vitality)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(1, skinnedMeshRenderer.GetBlendShapeWeight(1) + (vitality-startingVitality)/10f);
                yield return new WaitForSeconds(0.02f);
            }
        //Adjusting Speed
        if(startingSpeed<speed)
            while(skinnedMeshRenderer.GetBlendShapeWeight(0) < speed)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(0, skinnedMeshRenderer.GetBlendShapeWeight(0) + (speed-startingSpeed)/10f);
                yield return new WaitForSeconds(0.02f);
            }
        else
            while(skinnedMeshRenderer.GetBlendShapeWeight(0) > speed)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(0, skinnedMeshRenderer.GetBlendShapeWeight(0) + (speed-startingSpeed)/10f);
                yield return new WaitForSeconds(0.02f);
            }
        blendCoroutineRunning = false;
    }

    public void ForceUpdateBlendshaped(float sentienceLevel, float primalLevel, float vitalityLevel, float speedLevel)
    {
        //map the weight for Sentience
        float sentienceWeight = sentienceLevel * 100f/15f;

        //map the weight for Primal
        float primalWeight = primalLevel * 100f/15f;

        //map the weight for Vitality
        float vitalityWeight = vitalityLevel * 100f/15f;

        //map the weight for Speed
        float speedWeight = speedLevel * 100f/15f;
        skinnedMeshRenderer.SetBlendShapeWeight(2, sentienceWeight);
        skinnedMeshRenderer.SetBlendShapeWeight(3, primalWeight);
        skinnedMeshRenderer.SetBlendShapeWeight(1, vitalityWeight);
        skinnedMeshRenderer.SetBlendShapeWeight(0, speedWeight);
    }

    public void UpdateColorsAndTexture()
    {
        Material[] materials = skinnedMeshRenderer.materials;
        materials[0].SetColor("_Color", capColor);
        if(EyeTexture!=null)
            materials[1].SetTexture("_Secondary_Texture", EyeTexture);
        if(MouthTexture!=null)
            materials[1].SetTexture("_Tertiary_Texture", MouthTexture);
        materials[1].SetColor("_Color", bodyColor);
    }

    private IEnumerator RandomBlinks()
    {
        while (canBlink)
        {
            yield return new WaitForSeconds(Random.Range(2f, 4f));

            yield return StartCoroutine(Blink());
        }
    }

    public IEnumerator Blink()
    {
        if (canBlink) CloseEyes();
        yield return new WaitForSeconds(Random.Range(0.2f, 0.4f));
        if (canBlink) OpenEyes();
    }

    public void CloseEyes()
    {
        Material[] materials = skinnedMeshRenderer.materials;
        materials[1].SetTexture("_Secondary_Texture", blinkTexture);
    }
    public void OpenEyes()
    {
        Material[] materials = skinnedMeshRenderer.materials;
        materials[1].SetTexture("_Secondary_Texture", EyeTexture);
    }

    public void SetCapColor(UnityEngine.Color color)
    {
        capColor = color;
        UpdateColorsAndTexture();
    }
    public void SetBodyColor(UnityEngine.Color color)
    {
        bodyColor = color;
        UpdateColorsAndTexture();
    }

    public void ResetLevelCap()
    {
        LevelCap = 15;
    }

    public void StartNutrientGlow()
    {
        Material[] materials = skinnedMeshRenderer.materials;

        try
        {
            StopCoroutine(nutrientGlow);
        }
        catch
        {

        }

        nutrientGlow = StartCoroutine(NutrientGlow(materials[0]));
    }

    IEnumerator NutrientGlow(Material material)
    {
        float elapsedTime = 0f;
        float t = 0f;
        float time = 0.1f;

        float intensity = 3;
        float factor = Mathf.Pow(2, intensity);
        UnityEngine.Color startingColor = material.GetColor("_Tertiary_Color");
        UnityEngine.Color glowColor = new UnityEngine.Color(0.1f * factor, 1f * factor, 0.1f * factor);
        UnityEngine.Color whiteColor = new UnityEngine.Color(1, 1, 1);
        UnityEngine.Color finalColor;

        while (elapsedTime < time)
        {
            t = elapsedTime / time;

            finalColor = UnityEngine.Color.Lerp(startingColor, glowColor, t);
            material.SetColor("_Tertiary_Color", finalColor);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        elapsedTime = 0f;
        t = 0f;
        time = 0.3f;
        startingColor = material.GetColor("_Tertiary_Color");

        while (elapsedTime < time)
        {
            t = elapsedTime / time;

            finalColor = UnityEngine.Color.Lerp(startingColor, whiteColor, t);
            material.SetColor("_Tertiary_Color", finalColor);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }
    }
}
