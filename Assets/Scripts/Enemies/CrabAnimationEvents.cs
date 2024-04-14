using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabAnimationEvents : MonoBehaviour
{
    void CrabSmash()
    {
        if (GlobalData.isAbleToPause)
        {
            ParticleManager.Instance.SpawnParticles("SmashParticle", transform.Find("SmashParticleHolder").position, Quaternion.Euler(-90,0,0));
            SoundEffectManager.Instance.PlaySound("Explosion", transform.position);
        }
    }
}
