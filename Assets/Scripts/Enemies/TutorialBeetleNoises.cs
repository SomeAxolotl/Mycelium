using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBeetleNoises : MonoBehaviour
{
    [SerializeField] float noiseCutoffDistance = 25f;
    [SerializeField] float noiseInterval = 4f;

    IEnumerator Start()
    {
        while (!IsNearPlayer() && GlobalData.isAbleToPause)
        {
            SoundEffectManager.Instance.PlaySound("Beetle Charge", transform, 0, GetComponent<BeetleAnimationEvents>().GetPitchMultiplier());
            Debug.Log("playing beetle sound");
            yield return new WaitForSeconds(noiseInterval);
        }
    }

    bool IsNearPlayer()
    {
        if (Vector3.Distance(transform.position, GameObject.FindWithTag("currentPlayer").transform.position) < noiseCutoffDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
