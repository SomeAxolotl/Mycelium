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

    [SerializeField] private List<UnityEngine.Color> defaultColors = new List<UnityEngine.Color>
    {
        UnityEngine.Color.gray, //add more
        UnityEngine.Color.magenta //add more
    };
    [SerializeField] private List<UnityEngine.Color> poisonColors = new List<UnityEngine.Color>
    {
        UnityEngine.Color.green, //add more
        UnityEngine.Color.magenta //add more
    };
    [SerializeField] private List<UnityEngine.Color> coralColors = new List<UnityEngine.Color>
    {
        UnityEngine.Color.red, //add more
        UnityEngine.Color.magenta //add more
    };
    [SerializeField] private List<UnityEngine.Color> cordycepsColors = new List<UnityEngine.Color>
    {
        UnityEngine.Color.blue, //add more
        UnityEngine.Color.magenta //add more
    };


    private void Start()
    {
        UpdateColors();
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

    public void CreateSpeciesPalette(string subspecies)
    {
        UnityEngine.Color capColor = UnityEngine.Color.black;
        UnityEngine.Color bodyColor = UnityEngine.Color.black;
        int randomColorIndex = 0;
        switch (subspecies)
        {
            case "Default":
                randomColorIndex = UnityEngine.Random.Range(0, defaultColors.Count);
                SetCapColor(defaultColors[randomColorIndex]);
                randomColorIndex = UnityEngine.Random.Range(0, defaultColors.Count);
                SetBodyColor(defaultColors[randomColorIndex]);
                break;

            case "Poison":
                randomColorIndex = UnityEngine.Random.Range(0, poisonColors.Count);
                SetCapColor(poisonColors[randomColorIndex]);
                randomColorIndex = UnityEngine.Random.Range(0, poisonColors.Count);
                SetBodyColor(poisonColors[randomColorIndex]);
                break;

            case "Coral":
                randomColorIndex = UnityEngine.Random.Range(0, coralColors.Count);
                SetCapColor(coralColors[randomColorIndex]);
                randomColorIndex = UnityEngine.Random.Range(0, coralColors.Count);
                SetBodyColor(coralColors[randomColorIndex]);
                break;

            case "Cordyceps":
                randomColorIndex = UnityEngine.Random.Range(0, cordycepsColors.Count);
                SetCapColor(cordycepsColors[randomColorIndex]);
                randomColorIndex = UnityEngine.Random.Range(0, cordycepsColors.Count);
                SetBodyColor(cordycepsColors[randomColorIndex]);
                break;
        }

        UpdateColors();
    }
}
