using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSporeAnimation : MonoBehaviour
{
    [SerializeField] private float growDuration = 2f;
    [SerializeField] private float spinSpeed = 1f;
    
   public void StartGrowAnimation()
   {
        StartCoroutine(GrowAnimation());
        StartCoroutine(SpinAnimation());
   }
   IEnumerator GrowAnimation()
   {
    
    NewSporeCam sporeCam = GameObject.Find("GrowCamera").GetComponent<NewSporeCam>();
    sporeCam.SwitchCamera("GrowCamera");

    Vector3 originalSize = transform.localScale;

    transform.localScale = Vector3.zero; 

    float growCounter = 0f;
    while (growCounter < growDuration)
    {
        float t = growCounter / growDuration;

        transform.localScale = Vector3.Lerp(Vector3.zero, originalSize, t);
        yield return null;
        growCounter += Time.deltaTime;
    }
    transform.localScale = originalSize;
    sporeCam.SwitchCamera("Main Camera");
   }

   IEnumerator SpinAnimation()
   {
        float growCounter = 0f;
        while (growCounter < growDuration)
        {
            float t = growCounter = 0f;
            while (growCounter < growDuration)
            {
                transform.rotation *= Quaternion.Euler(0f, spinSpeed, 0f);

                yield return null;
                growCounter += Time.deltaTime;
            }
        }
   }
}
