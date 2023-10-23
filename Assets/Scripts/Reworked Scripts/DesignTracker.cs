using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignTracker : MonoBehaviour
{
    //Blendshape Management
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Mesh skinnedMesh;
    private Material capMaterial;
    private Material torsoMaterial;
    [SerializeField] Color capColor;
    [SerializeField] Color bodyColor;
    private bool blendCoroutineRunning = false;

    private void Start()
    {
        //foreach (Transform child in this.transform.Find("Model"))
        Transform capTransform = transform.Find("Model").Find("Cap");
        Transform torsoTransform = transform.Find("Model").Find("Torso");

        skinnedMeshRenderer = capTransform.gameObject.GetComponent<SkinnedMeshRenderer>();
        Debug.Log(skinnedMeshRenderer);
        skinnedMesh = capTransform.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
        capMaterial = capTransform.gameObject.GetComponent<Renderer>().material;
        torsoMaterial = torsoTransform.gameObject.GetComponent<Renderer>().material;

        UpdateCapColor(capColor);
        UpdateBodyColor(bodyColor);
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


        /*for(int i=0;i<blendShapeCount;i++)
            {
            skinnedMeshRenderer.SetBlendShapeWeight(i,statLevelWeights[i]);
            }*/

    }

    //BUG: CANNOT LOWER BLENDSHAPE VALUES: ONLY RAISES THEM
    IEnumerator fadeBlend(float sentience, float primal, float vitality, float speed)
    {   
        blendCoroutineRunning =  true;
        float startingSentience = skinnedMeshRenderer.GetBlendShapeWeight(0);
        float startingPrimal = skinnedMeshRenderer.GetBlendShapeWeight(1);
        float startingVitality = skinnedMeshRenderer.GetBlendShapeWeight(2);
        float startingSpeed = skinnedMeshRenderer.GetBlendShapeWeight(3);
        Debug.Log("Initial Blendshape Weights:\nSentience: " + skinnedMeshRenderer.GetBlendShapeWeight(0) + "\nPrimal: " + skinnedMeshRenderer.GetBlendShapeWeight(1) + "\nVitality: " + skinnedMeshRenderer.GetBlendShapeWeight(2) + "\nSpeed: " + skinnedMeshRenderer.GetBlendShapeWeight(3));
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
        Debug.Log("Blendshape Weights:\nSentience: " + skinnedMeshRenderer.GetBlendShapeWeight(0) + "\nPrimal: " + skinnedMeshRenderer.GetBlendShapeWeight(1) + "\nVitality: " + skinnedMeshRenderer.GetBlendShapeWeight(2) + "\nSpeed: " + skinnedMeshRenderer.GetBlendShapeWeight(3));
        blendCoroutineRunning = false;
    }

    private void UpdateCapColor(Color color)
    {
        capMaterial.SetColor("_Color", color);
    }
    private void UpdateBodyColor(Color color)
    {
        torsoMaterial.SetColor("_Color", color);
    }
}
