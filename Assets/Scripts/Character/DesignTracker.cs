using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class DesignTracker : MonoBehaviour
{
    //Blendshape Management
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    private Mesh skinnedMesh;
    public UnityEngine.Color capColor;
    public UnityEngine.Color bodyColor;
    private bool blendCoroutineRunning = false;
    private List<int> UpdateTypes;

    private void Start()
    {
        UpdateColors();
    }

    public void UpdateBlendshape(int sentienceLevel, int primalLevel, int vitalityLevel, int speedLevel)
    {
        //map the weight for Sentience
        float sentienceWeight = sentienceLevel * 100f/10f;

        //map the weight for Primal
        float primalWeight = primalLevel * 100f/10f;

        //map the weight for Vitality
        float vitalityWeight = vitalityLevel * 100f/10f;

        //map the weight for Speed
        float speedWeight = speedLevel * 100f/10f;

        int totalLevel = sentienceLevel + primalLevel + vitalityLevel + speedLevel;

        if(blendCoroutineRunning) StopAllCoroutines();
        StartCoroutine(fadeBlend(sentienceWeight, primalWeight, vitalityWeight, speedWeight));
    }

    //BUG: CANNOT LOWER BLENDSHAPE VALUES: ONLY RAISES THEM
    IEnumerator fadeBlend(float sentience, float primal, float vitality, float speed)
    {   
        blendCoroutineRunning =  true;
        float startingSentience = skinnedMeshRenderer.GetBlendShapeWeight(2);
        float startingPrimal = skinnedMeshRenderer.GetBlendShapeWeight(3);
        float startingVitality = skinnedMeshRenderer.GetBlendShapeWeight(1);
        float startingSpeed = skinnedMeshRenderer.GetBlendShapeWeight(0);
        while(skinnedMeshRenderer.GetBlendShapeWeight(2) < sentience)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(2, skinnedMeshRenderer.GetBlendShapeWeight(2) + (sentience-startingSentience)/10f);
            yield return new WaitForSeconds(0.02f);
        }
        while(skinnedMeshRenderer.GetBlendShapeWeight(3) < primal)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(3, skinnedMeshRenderer.GetBlendShapeWeight(3) + (primal-startingPrimal)/10f);
            yield return new WaitForSeconds(0.02f);
        }
        while(skinnedMeshRenderer.GetBlendShapeWeight(1) < vitality)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(1, skinnedMeshRenderer.GetBlendShapeWeight(1) + (vitality-startingVitality)/10f);
            yield return new WaitForSeconds(0.02f);
        }
        while(skinnedMeshRenderer.GetBlendShapeWeight(0) < speed)
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

    public void UpdateColors()
    {
        Material[] materials = skinnedMeshRenderer.materials;
        materials[0].SetColor("_Color", capColor);
        materials[1].SetColor("_Color", bodyColor);
    }

    public void SetCapColor(UnityEngine.Color color)
    {
        capColor = color;
    }
    public void SetBodyColor(UnityEngine.Color color)
    {
        bodyColor = color;
    }
}
