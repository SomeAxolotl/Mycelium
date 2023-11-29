using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class DesignTracker : MonoBehaviour
{
    //Blendshape Management
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Mesh skinnedMesh;
    private Material capMaterial;
    private Material torsoMaterial;
    public UnityEngine.Color capColor;
    public UnityEngine.Color bodyColor;
    private bool blendCoroutineRunning = false;

    private void Start()
    {
        //foreach (Transform child in this.transform.Find("Model"))
        Transform capTransform = transform.Find("Cap");
        Transform torsoTransform = transform.Find("Torso");

        skinnedMeshRenderer = capTransform.gameObject.GetComponent<SkinnedMeshRenderer>();
        skinnedMesh = capTransform.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
        capMaterial = capTransform.gameObject.GetComponent<Renderer>().material;
        torsoMaterial = torsoTransform.gameObject.GetComponent<Renderer>().material;

        RefreshColors();
    }

    public void UpdateBlendshape(int sentienceLevel, int primalLevel, int vitalityLevel, int speedLevel)
    {
        //map the weight for Sentience
        float sentienceWeight = sentienceLevel * 100f/15f;

        //map the weight for Primal
        float primalWeight = primalLevel * 100f/15f;

        //map the weight for Vitality
        float vitalityWeight = vitalityLevel * 100f/15f;

        //map the weight for Speed
        float speedWeight = speedLevel * 100f/15f;

        if(blendCoroutineRunning) StopAllCoroutines();
        StartCoroutine(fadeBlend(sentienceWeight, primalWeight, vitalityWeight, speedWeight));
    }

    //BUG: CANNOT LOWER BLENDSHAPE VALUES: ONLY RAISES THEM
    IEnumerator fadeBlend(float sentience, float primal, float vitality, float speed)
    {   
        blendCoroutineRunning =  true;
        float startingSentience = skinnedMeshRenderer.GetBlendShapeWeight(0);
        float startingPrimal = skinnedMeshRenderer.GetBlendShapeWeight(1);
        float startingVitality = skinnedMeshRenderer.GetBlendShapeWeight(2);
        float startingSpeed = skinnedMeshRenderer.GetBlendShapeWeight(3);
        while(skinnedMeshRenderer.GetBlendShapeWeight(0) < sentience)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(0, skinnedMeshRenderer.GetBlendShapeWeight(0) + (sentience-startingSentience)/10f);
            yield return new WaitForSeconds(0.02f);
        }
        while(skinnedMeshRenderer.GetBlendShapeWeight(1) < primal)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(1, skinnedMeshRenderer.GetBlendShapeWeight(1) + (primal-startingPrimal)/10f);
            yield return new WaitForSeconds(0.02f);
        }
        while(skinnedMeshRenderer.GetBlendShapeWeight(2) < vitality)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(2, skinnedMeshRenderer.GetBlendShapeWeight(2) + (vitality-startingVitality)/10f);
            yield return new WaitForSeconds(0.02f);
        }
        while(skinnedMeshRenderer.GetBlendShapeWeight(3) < speed)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(3, skinnedMeshRenderer.GetBlendShapeWeight(3) + (speed-startingSpeed)/10f);
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
        skinnedMeshRenderer.SetBlendShapeWeight(0, sentienceWeight);
        skinnedMeshRenderer.SetBlendShapeWeight(1, primalWeight);
        skinnedMeshRenderer.SetBlendShapeWeight(2, vitalityWeight);
        skinnedMeshRenderer.SetBlendShapeWeight(3, speedWeight);
    }

    public void RefreshColors()
    {
        capMaterial.SetColor("_Color", capColor);
        torsoMaterial.SetColor("_Color", bodyColor);
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
