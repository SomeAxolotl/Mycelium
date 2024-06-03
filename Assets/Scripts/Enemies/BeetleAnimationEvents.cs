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
            SoundEffectManager.Instance.PlaySound("Beetle Walk", transform, GetVolumeModifier(), GetPitchMultiplier());
        }
    }

    public float GetPitchMultiplier()
    {
        float pitchModifier = (1 / transform.localScale.x) + 0.15f;

        //Debug.Log(pitchModifier);

        return pitchModifier;
    }

    public float GetVolumeModifier()
    {
        float volumeModifier = (transform.localScale.x - 1) + 0.2f;

        //Debug.Log(gameObject.name + " volume modifier: " + volumeModifier);

        return volumeModifier;
    }
}
