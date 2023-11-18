using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
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

    private List<AudioSource> audioSources = new List<AudioSource>();

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
        }

        int randomNumber = Random.Range(0, clipList.Count);
        AudioSource.PlayClipAtPoint(clipList[randomNumber], position, clipVolume);
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

    void Update()
    {
        if (Input.GetKeyDown("g"))
        {
            PlaySound("impact", GameObject.FindWithTag("currentPlayer").transform.position);
        }   
    }
}
