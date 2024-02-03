using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class RainRippleScript : MonoBehaviour
{
    [SerializeField] float speedOfAnimation;

    float frameIndex = 0;

    bool doRippleRandomize = false;


    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalFloat("_RippleArrayFrame", Mathf.Floor(frameIndex));

        frameIndex += speedOfAnimation * Time.deltaTime;

        if (frameIndex >= 16)
        {
            frameIndex = 0;
        }

        if (frameIndex > 0 && frameIndex < 2)
        {
            Shader.SetGlobalFloat("_RippleScale", Random.Range(2.0f, 5.0f));
        }
    }
}