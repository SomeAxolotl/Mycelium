using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleAnimationEvents : MonoBehaviour
{
    //NOT BEING CALLED
    void Charge()
    {
        if (GlobalData.isAbleToPause)
        {
            SoundEffectManager.Instance.PlaySound("Beetle Charge", transform.position);
        }
    }
}
