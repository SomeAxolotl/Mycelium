using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeAnimationEvents : MonoBehaviour
{
    void Footstep1()
    {
        SoundEffectManager.Instance.PlaySound("Footstep 1", transform.position);
    }

    void Footstep2()
    {
        SoundEffectManager.Instance.PlaySound("Footstep 2", transform.position);
    }

    void Swing()
    {
        SoundEffectManager.Instance.PlaySound("Slash", transform.position);
    }

    void Pant()
    {
        SoundEffectManager.Instance.PlaySound("Panting", transform.position);
    }
}
