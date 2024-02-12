using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleAnimationEvents : MonoBehaviour
{
    void Charge()
    {
        SoundEffectManager.Instance.PlaySound("Beetle Charge", transform.position);
    }
}
