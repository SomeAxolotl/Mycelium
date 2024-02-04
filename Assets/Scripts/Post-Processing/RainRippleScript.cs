using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class RainRippleScript : MonoBehaviour
{
    [SerializeField] float speedOfAnimation = 16;

    float frameIndex = 0;
    float numOfFrames = 15;
    float delay = 2.0f;

    bool doRippleRandomize = false;


    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalFloat("_RippleArrayFrame", Mathf.Floor(frameIndex));

        frameIndex = AdvanceFramesAndWait(frameIndex, "_RippleScale"); 
    }

    private float AdvanceFramesAndWait(float index, string shaderVariableName)
    {
        index += speedOfAnimation * Time.deltaTime;

        if (index >= 16)
        {
            index = 0;
        }

        if (index > 0 && index < 2)
        {
            Shader.SetGlobalFloat(shaderVariableName, Random.Range(2.0f, 5.0f));
        }

        return index;
    }
}