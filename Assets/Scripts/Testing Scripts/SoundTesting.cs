using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTesting : MonoBehaviour
{
    [SerializeField] private float impactSoundVolume;
    [SerializeField] private float impactDelay;
    [SerializeField] private List<AudioClip> impactSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> slashSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> stabSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> smashSounds = new List<AudioClip>();
    private List<AudioSource> audioSources = new List<AudioSource>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Component component in GetComponents<Component>())
        {
            AudioSource audioSource = component as AudioSource;

            if (audioSource != null)
            {
                audioSources.Add(audioSource);
            }
        }

        Debug.Log("Audio Sources: " + audioSources.Count);
    }

    public void ImpactButton()
    {
        PlayRandomSound(impactSounds, impactSoundVolume);
    }

    public void SlashSwingButton()
    {
        PlayRandomSound(slashSounds);
    }

    public void StabSwingButton()
    {
        PlayRandomSound(stabSounds);
    }

    public void SmashSwingButton()
    {
        PlayRandomSound(smashSounds);
    }

    public void SlashBugButton()
    {
        PlayRandomSound(slashSounds);
        StartCoroutine(PlayRandomSoundOnDelay(impactSounds, impactSoundVolume, impactDelay));
    }

    public void StabBugButton()
    {
        PlayRandomSound(stabSounds);
        StartCoroutine(PlayRandomSoundOnDelay(impactSounds, impactSoundVolume, impactDelay));
    }

    public void SmashBugButton()
    {
        PlayRandomSound(smashSounds);
        StartCoroutine(PlayRandomSoundOnDelay(impactSounds, impactSoundVolume, impactDelay));
    }

    private void PlayRandomSound(List<AudioClip> clipList, float volume = 1.0f)
    {
        int randomNumber = Random.Range(0, clipList.Count - 1);
        GetFirstAvailableAudioSource().PlayOneShot(clipList[randomNumber], volume);
    }

    IEnumerator PlayRandomSoundOnDelay(List<AudioClip> clipList, float volume = 1.0f, float delay = 0.25f)
    {
        int randomNumber = Random.Range(0, clipList.Count - 1);
        yield return new WaitForSeconds(delay);
        GetFirstAvailableAudioSource().PlayOneShot(clipList[randomNumber], volume);
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
    }
}
