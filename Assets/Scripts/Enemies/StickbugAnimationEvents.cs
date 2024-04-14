using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickbugAnimationEvents : MonoBehaviour
{
    void Shoot()
    {
        if (GlobalData.isAbleToPause)
        {
            SoundEffectManager.Instance.PlaySound("Stickbug Shoot", transform.position);
        }
    }
}
