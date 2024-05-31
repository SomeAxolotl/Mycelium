using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    [SerializeField] float slowestStopPoint = 0.1f;
    [SerializeField] float secondsTilSpeedup = 0.1f;

    public static HitStopManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void HitStop()
    {
        StartCoroutine(HitStopCoroutine());
    }

    IEnumerator HitStopCoroutine()
    {
        Time.timeScale = slowestStopPoint;

        //Speed Up
        float t = 0f;
        while (t < secondsTilSpeedup)
        {
            //Debug.Log(t);
            //float easedT = Mathf.Pow((t/secondsTilSpeedup), 2);
            //Time.timeScale = Mathf.Lerp(slowestStopPoint, 1.0f, easedT);

            t += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 1.0f;
    }
}
