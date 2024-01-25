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
