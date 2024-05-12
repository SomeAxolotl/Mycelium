using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;

    private CinemachineImpulseListener impulseListener;

    private float defaultShakeForce = 1f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        try
        {
            impulseListener = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineImpulseListener>();
        }
        catch
        {
            Debug.LogWarning("Couldn't find the Impulse Listener. Camera Shakes won't be possible. If we are on the Main Menu, then this is fine.");
        }
    }

    public void ShakeCamera(CinemachineImpulseSource impulseSource)
    {
        impulseListener.m_ReactionSettings.m_Duration = impulseSource.m_ImpulseDefinition.m_ImpulseDuration;

        impulseSource.GenerateImpulseWithForce(defaultShakeForce);

        RumbleManager.Instance.RumblePulse(impulseSource.m_ImpulseDefinition.m_ImpulseDuration, defaultShakeForce/3, defaultShakeForce/3);
    }

    public void ShakeCamera(CinemachineImpulseSource impulseSource, float forceOverride)
    {
        impulseListener.m_ReactionSettings.m_Duration = impulseSource.m_ImpulseDefinition.m_ImpulseDuration;

        impulseSource.GenerateImpulseWithForce(forceOverride);

        RumbleManager.Instance.RumblePulse(impulseSource.m_ImpulseDefinition.m_ImpulseDuration, defaultShakeForce/3, defaultShakeForce/3);
    }
}
