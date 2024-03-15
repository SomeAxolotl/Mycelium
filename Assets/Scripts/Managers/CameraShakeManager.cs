using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;

    private CinemachineImpulseListener.ImpulseReaction impulseReaction;

    private float defaultShakeForce = 1f;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        try
        {
            impulseReaction = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineImpulseListener.ImpulseReaction>();
        }
        catch
        {
            Debug.LogWarning("Couldn't find the Impulse Listener. Camera Shakes won't be possible. If we are on the Main Menu, then this is fine.");
        }
    }

    public void ShakeCamera(CinemachineImpulseSource impulseSource)
    {
        impulseReaction.m_Duration = impulseSource.m_ImpulseDefinition.m_ImpulseDuration;

        impulseSource.GenerateImpulseWithForce(defaultShakeForce);
    }

    public void ShakeCamera(CinemachineImpulseSource impulseSource, float forceOverride)
    {
        impulseReaction.m_Duration = impulseSource.m_ImpulseDefinition.m_ImpulseDuration;

        impulseSource.GenerateImpulseWithForce(forceOverride);
    }
}
