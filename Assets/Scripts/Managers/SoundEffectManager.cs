using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance;

    [SerializeField] private AudioMixerGroup audioMixerGroup;
    //[SerializeField] private float minDistance = 5f;

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

    [SerializeField] private List<SoundEffect> soundEffects;

    public void PlaySound(string clipName, Vector3 position)
    {   
        foreach (SoundEffect sfx in soundEffects)
        {
            if (clipName == sfx.sfxName)
            {
                int randomNumber = Random.Range(0, sfx.sfxSounds.Count);
                AudioSource audioSource = PlayClipAtPointAndGetSource(sfx.sfxSounds[randomNumber], position, sfx.sfxVolume);
                audioSource.outputAudioMixerGroup = audioMixerGroup;
                audioSource.dopplerLevel = 0;

                float randomPitchModifier = sfx.sfxBasePitchChange + Random.Range(-sfx.sfxPitchRange, sfx.sfxPitchRange);
                audioSource.pitch += randomPitchModifier;
            }
        }
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

    [System.Serializable]
    class SoundEffect
    {
        [SerializeField] public string sfxName = "[Name]";
        [SerializeField] public float sfxVolume = 1f;
        [SerializeField] public float sfxBasePitchChange = 0f;
        [SerializeField] public float sfxPitchRange = 0.25f;
        [SerializeField] public List<AudioClip> sfxSounds = new List<AudioClip>();
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
