using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteAlways]
public class RainRippleScript : MonoBehaviour
{
    [SerializeField] float speedOfAnimation = 16;

    List<float> frameIndex = new List<float>();
    List<float> delay = new List<float>();
    List<float> timer = new List<float>();

    int seed;

    private void OnEnable()
    {
        seed = -2147483648;

        //Reset Frame Index
        frameIndex.Clear();

        frameIndex.Add(0);
        frameIndex.Add(0);
        frameIndex.Add(0);

        //Reset Delay
        delay.Clear();

        delay.Add(UnityEngine.Random.Range(0.0f, 1.0f));
        delay.Add(UnityEngine.Random.Range(1.0f, 2.0f));
        delay.Add(UnityEngine.Random.Range(2.0f, 3.0f));

        //Reset Timer
        timer.Clear();

        timer.Add(0f);
        timer.Add(0f);
        timer.Add(0f);

        Debug.Log("Reset Ripple Lists");
    }

    void Update()
    {
        AdvanceFramesAndWait(0);
        //UnityEngine.Random.InitState(seed);
        //seed += 1;

        AdvanceFramesAndWait(1);
        //UnityEngine.Random.InitState(seed);
        //seed += 1;

        AdvanceFramesAndWait(2);
        //UnityEngine.Random.InitState(seed);
        //seed += 1;
    }

    private void AdvanceFramesAndWait(int i)
    {
        //Debug.Log("Timer: " + timer[i] + "\tDelay: " + delay[i]);
        //Debug.Log("Delay for " + i + " is: " + delay[i]);

        if (timer[i] >= delay[0])
        {
            frameIndex[i] += speedOfAnimation * Time.deltaTime;
        }

        if (frameIndex[i] >= 16)
        {
            
            frameIndex[i] = 0;
            timer[i] = 0;
            //delay[i] = UnityEngine.Random.Range(2.0f + i, i * 2 + 3f);
            delay[i] = UnityEngine.Random.Range(2f * i, 2f * i + 2f);
        }

        if (frameIndex[i] > 0 && frameIndex[i] < 2)
        {
            Shader.SetGlobalFloat("_RippleScale" + i, UnityEngine.Random.Range(2.0f, 5.0f));
        }

        timer[i] += Time.deltaTime;
        Shader.SetGlobalFloat("_RippleArrayFrame" + i, Mathf.Floor(frameIndex[i]));
    }
}