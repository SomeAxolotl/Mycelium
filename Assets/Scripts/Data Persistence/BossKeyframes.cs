using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static UnityEngine.Rendering.DebugUI;

public class BossKeyframes : MonoBehaviour
{
    public static BossKeyframes Instance;

    //Left Attack
    [HideInInspector] public string leftPath = "LeftArm/LeftArm_target";
    [HideInInspector] public Type leftType = typeof(Transform);

    [HideInInspector] public string LeftPropertyX = "m_LocalPosition.x";
    [HideInInspector] public Keyframe[] leftKeysX = new Keyframe[9];

    [HideInInspector] public string LeftPropertyZ = "m_LocalPosition.z";
    [HideInInspector] public Keyframe[] leftKeysZ = new Keyframe[9];

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        ResetLeftAttack();
    }

    public void ChangeKeyframe(string attack, string field, int keyframeIndex, float value)
    {
        switch(attack.ToLower())
        {
            case "left":
                if (field.ToLower() == "x")
                {
                    leftKeysX[keyframeIndex].value = value;
                }
                else if (field.ToLower() == "z")
                {
                    leftKeysZ[keyframeIndex].value = value;
                }
                break;

            case "right":
                break;

            case "smash":
                break;

            default:
                break;
        }
    }

    public AnimationCurve GetAnimationCurve(string attack, string field)
    {
        switch (attack.ToLower())
        {
            case "left":
                if (field.ToLower() == "x")
                {
                    return new AnimationCurve(leftKeysX);
                }
                else if (field.ToLower() == "z")
                {
                    return new AnimationCurve(leftKeysZ);
                }
                break;

            case "right":
                break;

            case "smash":
                break;

            default:
                break;
        }

        return null;
    }

    private void ResetLeftAttack()
    {
        //Left Attack X Position
        leftKeysX[0] = new Keyframe(0f, -3.71665f);
        leftKeysX[1] = new Keyframe(0.2833333f, -6.1605f);
        leftKeysX[2] = new Keyframe(0.55f, -8.8282f);
        leftKeysX[3] = new Keyframe(1.05f, -3.13575f);
        leftKeysX[4] = new Keyframe(1.516667f, -2.84715f);
        leftKeysX[5] = new Keyframe(1.783333f, -2.35135f);
        leftKeysX[6] = new Keyframe(1.95f, -1.98505f);
        leftKeysX[7] = new Keyframe(2.75f, -1.98505f);
        leftKeysX[8] = new Keyframe(3.1f, -4.3105f);

        //Left Attack Z Position
        leftKeysZ[0] = new Keyframe(0f, 2.45125f);
        leftKeysZ[1] = new Keyframe(0.2833333f, 1.05265f);
        leftKeysZ[2] = new Keyframe(0.55f, -1.68905f);
        leftKeysZ[3] = new Keyframe(1.05f, -5.3317f);
        leftKeysZ[4] = new Keyframe(1.516667f, -3.9331f);
        leftKeysZ[5] = new Keyframe(1.783333f, 2.0017f);
        leftKeysZ[6] = new Keyframe(1.95f, 5.27065f);
        leftKeysZ[7] = new Keyframe(2.75f, 5.27065f);
        leftKeysZ[8] = new Keyframe(3.1f, 5.7276f);

    }
}
