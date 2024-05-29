using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleAnimationEvents : MonoBehaviour
{
    //NOT BEING CALLED
    void Charge()
    {
        //ALFJADFJADLFJADKLFJADKL:FJ
    }

    void Step()
    {
        if (GlobalData.isAbleToPause)
        {
            SoundEffectManager.Instance.PlaySound("Beetle Walk", transform, 0, GetPitchMultiplier());
        }
    }

    public float GetPitchMultiplier()
    {
        float pitchModifier = 1 / transform.localScale.x;

        //Debug.Log(pitchModifier);

        return pitchModifier;
    }
}
