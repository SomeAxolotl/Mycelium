using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickbugAnimationEvents : MonoBehaviour
{
    void Shoot()
    {
        SoundEffectManager.Instance.PlaySound("Stickbug Shoot", transform.position);
    }
}
