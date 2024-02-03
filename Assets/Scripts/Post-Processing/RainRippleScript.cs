using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class RainRippleScript : MonoBehaviour
{
    [SerializeField] float speedOfAnimation;

    float frameIndex = 0;


    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalFloat("_RippleArrayFrame", Mathf.Floor(frameIndex));

        frameIndex += speedOfAnimation * Time.deltaTime;

        if (frameIndex >= 16)
        {
            frameIndex = 0;
        }
        
    }
}