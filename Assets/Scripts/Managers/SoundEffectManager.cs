using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance;

    [SerializeField] private AudioMixerGroup audioMixerGroup;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    [Header("Impact SFX")]
    [SerializeField] private float impactVolume = 1f;
    [SerializeField] private List<AudioClip> impactSounds = new List<AudioClip>();
    
    [Header("Slash SFX")]
    [SerializeField] private float slashVolume = 1f;
    [SerializeField] private List<AudioClip> slashSounds = new List<AudioClip>();
    
    [Header("Stab SFX")]
    [SerializeField] private float stabVolume = 1f;
    [SerializeField] private List<AudioClip> stabSounds = new List<AudioClip>();
    
    [Header("Smash SFX")]
    [SerializeField] private float smashVolume = 1f;
    [SerializeField] private List<AudioClip> smashSounds = new List<AudioClip>();

    [Header("Projectile SFX")]
    [SerializeField] private float projectileVolume = 1f;
    [SerializeField] private List<AudioClip> projectileSounds = new List<AudioClip>();

    [Header("Explosion SFX")]
    [SerializeField] private float explosionVolume = 1f;
    [SerializeField] private List<AudioClip> explosionSounds = new List<AudioClip>();

    [Header("Beetle Charge SFX")]
    [SerializeField] private float beetleChargeVolume = 1f;
    [SerializeField] private List<AudioClip> beetleChargeSounds = new List<AudioClip>();

    [Header("Stickbug Shoot SFX")]
    [SerializeField] private float stickbugShootVolume = 1f;
    [SerializeField] private List<AudioClip> stickbugShootSounds = new List<AudioClip>();

    [Header("Footstep 1 SFX")]
    [SerializeField] private float footstep1Volume = 1f;
    [SerializeField] private List<AudioClip> footstep1Sounds = new List<AudioClip>();

    [Header("Footstep 2 SFX")]
    [SerializeField] private float footstep2Volume = 1f;
    [SerializeField] private List<AudioClip> footstep2Sounds = new List<AudioClip>();

    [Header("Panting SFX")]
    [SerializeField] private float pantingVolume = 1f;
    [SerializeField] private List<AudioClip> pantingSounds = new List<AudioClip>();

    [Header("Pickup SFX")]
    [SerializeField] private float pickupVolume = 1f;
    [SerializeField] private List<AudioClip> pickupSounds = new List<AudioClip>();

    [Header("Hurt SFX")]
    [SerializeField] private float hurtVolume = 1f;
    [SerializeField] private List<AudioClip> hurtSounds = new List<AudioClip>();

    [Header("Damaged SFX")]
    [SerializeField] private float damagedVolume = 1f;
    [SerializeField] private List<AudioClip> damagedSounds = new List<AudioClip>();

    [Header("UIMove SFX")]
    [SerializeField] private float uiMoveVolume = 1f;
    [SerializeField] private List<AudioClip> uiMoveSounds = new List<AudioClip>();

    [Header("UISelect SFX")]
    [SerializeField] private float uiSelectVolume = 1f;
    [SerializeField] private List<AudioClip> uiSelectSounds = new List<AudioClip>();

    public void PlaySound(string clipName, Vector3 position)
    {   
        List<AudioClip> clipList = new List<AudioClip>();
        float clipVolume = 1f;
        switch (clipName)
        {
            case "Impact":
                clipList = impactSounds;
                clipVolume = impactVolume;
                break;

            case "Slash":
                Debug.Log("slash sound");
                clipList = slashSounds;
                clipVolume = slashVolume;
                break;

            case "Stab":
                clipList = stabSounds;
                clipVolume = stabVolume;
                break;

            case "Smash":
                clipList = smashSounds;
                clipVolume = smashVolume;
                break;

            case "UIMove":
                clipList = uiMoveSounds;
                clipVolume = uiMoveVolume;
                break;

            case "UISelect":
                clipList = uiSelectSounds;
                clipVolume = uiSelectVolume;
                break;

            case "Projectile":
                clipList = projectileSounds;
                clipVolume = projectileVolume;
                break;

            case "Explosion":
                clipList = explosionSounds;
                clipVolume = explosionVolume;
                break;

            case "Beetle Charge":
                clipList = beetleChargeSounds;
                clipVolume = beetleChargeVolume;
                break;

            case "Stickbug Shoot":
                clipList = stickbugShootSounds;
                clipVolume = stickbugShootVolume;
                break;

            case "Footstep 1":
                clipList = footstep1Sounds;
                clipVolume = footstep1Volume;
                break;

            case "Footstep 2":
                clipList = footstep2Sounds;
                clipVolume = footstep2Volume;
                break;

            case "Panting":
                clipList = pantingSounds;
                clipVolume = pantingVolume;
                break;

            case "Pickup":
                clipList = pickupSounds;
                clipVolume = pickupVolume;
                break;

            case "Hurt":
                clipList = hurtSounds;
                clipVolume = hurtVolume;
                break;
            
            case "Damaged":
                clipList = damagedSounds;
                clipVolume = damagedVolume;
                break;

            default:
                Debug.LogWarning($"PlaySound: Unrecognized clipName '{clipName}'");
                return;

        }

        int randomNumber = Random.Range(0, clipList.Count);
        AudioSource audioSource = PlayClipAtPointAndGetSource(clipList[randomNumber], position, clipVolume);
        audioSource.outputAudioMixerGroup = audioMixerGroup;
    }

    AudioSource PlayClipAtPointAndGetSource(AudioClip clip, Vector3 position, float volume)
    {
      GameObject gameObject = new GameObject("One shot audio");
      gameObject.transform.position = position;
      AudioSource audioSource = (AudioSource) gameObject.AddComponent(typeof (AudioSource));
      audioSource.clip = clip;
      audioSource.spatialBlend = 1f;
      audioSource.volume = volume;
      audioSource.Play();
      Object.Destroy((Object) gameObject, clip.length * ((double) Time.timeScale < 0.009999999776482582 ? 0.01f : Time.timeScale));
      return audioSource;
    }

    //These are from when I was trying to use Reflection to dynamically change variable references
    /*List<AudioClip> GetClipList(string clipName)
    {
        switch (clipName)
        {
            case "impact":
                return 
        }

        List<AudioClip> clipList = (List<AudioClip>)this.GetType().GetField(clipName + "Sounds").GetValue(this);
        return clipList;
    }

    float GetClipVolume(string clipName)
    {
        float clipVolume = (float)this.GetType().GetField(clipName + "Volume").GetValue(this);
        return clipVolume;
    }

    private AudioSource GetFirstAvailableAudioSource()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                return audioSources[i];
            }
        }
        Debug.Log("NO AVAILABLE AUDIO SOURCES (ADD MORE)");
        return null;
    }*/
}
