using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    [SerializeField] float slowestStopPoint = 0.1f;
    [SerializeField] float damageToMillisecondsForSpeedUp = 15f;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            HitStop(20);
        }
    }

    public void HitStop(float damage)
    {
        StartCoroutine(HitStopCoroutine(damage));
    }

    IEnumerator HitStopCoroutine(float damage)
    {
        float speedUpDuration = (damage * damageToMillisecondsForSpeedUp) / 1000f;

        Time.timeScale = slowestStopPoint;

        //Speed Up
        float t = 0f;
        while (t < speedUpDuration)
        {
            //Debug.Log(t);
            float easedT = Mathf.Pow((t/speedUpDuration), 2);
            Time.timeScale = Mathf.Lerp(slowestStopPoint, 1.0f, easedT);

            t += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 1.0f;
    }
}
