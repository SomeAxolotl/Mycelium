using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance;

    [SerializeField] private AudioMixerGroup audioMixerGroup;

    [SerializeField] private List<SoundEffect> soundEffects;

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

    //Overload that DOESNT child the sound (it WONT follow the object)
    public void PlaySound(string clipName, Vector3 position, float volumeModifier = 0f, float pitchMultiplier = 1f, float maxDistance = 25f)
    {   
        foreach (SoundEffect sfx in soundEffects)
        {
            if (clipName == sfx.sfxName)
            {
                int randomNumber = Random.Range(0, sfx.sfxSounds.Count);
                AudioSource audioSource = PlayClipAtPointAndGetSource(sfx.sfxSounds[randomNumber], position, sfx.sfxVolume + volumeModifier);
                audioSource.outputAudioMixerGroup = audioMixerGroup;
                audioSource.maxDistance = maxDistance;
                audioSource.dopplerLevel = 0;

                float randomPitchModifier = sfx.sfxBasePitchChange + Random.Range(-sfx.sfxPitchRange, sfx.sfxPitchRange);
                audioSource.pitch += randomPitchModifier;
                audioSource.pitch *= pitchMultiplier;
            }
        }
    }

    AudioSource PlayClipAtPointAndGetSource(AudioClip clip, Vector3 position, float volume)
    {
      GameObject gameObj = new GameObject("One shot audio");
      gameObj.transform.position = position;
      AudioSource audioSource = (AudioSource) gameObj.AddComponent(typeof (AudioSource));
      audioSource.clip = clip;
      audioSource.spatialBlend = 1f;
      audioSource.volume = volume;
      audioSource.Play();
      Object.Destroy((Object) gameObj, clip.length * ((double) Time.timeScale < 0.009999999776482582 ? 0.01f : Time.timeScale));
      return audioSource;
    }

    //Overload that DOES child the sound (it WILL follow the object)
    public void PlaySound(string clipName, Transform targetTransform, float volumeModifier = 0f, float pitchMultiplier = 1f, float maxDistance = 50f)
    {   
        foreach (SoundEffect sfx in soundEffects)
        {
            if (clipName == sfx.sfxName)
            {
                int randomNumber = Random.Range(0, sfx.sfxSounds.Count);
                AudioSource audioSource = PlayClipAtPointAndGetSource(sfx.sfxSounds[randomNumber], targetTransform, sfx.sfxVolume + volumeModifier);
                audioSource.outputAudioMixerGroup = audioMixerGroup;
                audioSource.maxDistance = maxDistance;
                audioSource.dopplerLevel = 0;

                float randomPitchModifier = sfx.sfxBasePitchChange + Random.Range(-sfx.sfxPitchRange, sfx.sfxPitchRange);
                audioSource.pitch += randomPitchModifier;
                audioSource.pitch *= pitchMultiplier;
            }
        }
    }

    AudioSource PlayClipAtPointAndGetSource(AudioClip clip, Transform targetTransform, float volume)
    {
      GameObject gameObj = new GameObject("One shot audio");
      gameObj.transform.parent = targetTransform;
      gameObj.transform.localPosition = Vector3.zero;
      AudioSource audioSource = (AudioSource) gameObj.AddComponent(typeof (AudioSource));
      audioSource.clip = clip;
      audioSource.spatialBlend = 1f;
      audioSource.volume = volume;
      audioSource.Play();
      Object.Destroy((Object) gameObj, clip.length * ((double) Time.timeScale < 0.009999999776482582 ? 0.01f : Time.timeScale));
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
}

#if UNITY_EDITOR
[CustomEditor(typeof(SoundEffectManager))]
public class SoundEffectManagerEditor : Editor
{
    SerializedProperty soundEffects;

    int previewSfxIndex = 0;
    AudioSource previewAudioSource;

    GUIStyle headerStyle;

    void OnEnable()
    {
        soundEffects = serializedObject.FindProperty("soundEffects");
        
        GameObject audioSourceGameObject = new GameObject("AudioSourcePreview");
        previewAudioSource = audioSourceGameObject.AddComponent<AudioSource>();
        audioSourceGameObject.hideFlags = HideFlags.HideAndDontSave;
    }

    void OnDisable()
    {
        // Clean up the temporary AudioSource
        if (previewAudioSource != null)
        {
            DestroyImmediate(previewAudioSource.gameObject);
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Color headerColor;
        ColorUtility.TryParseHtmlString("#2fc256", out headerColor);
        headerStyle = new GUIStyle(EditorStyles.largeLabel)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 15,
            fixedHeight = 20,
            normal = { textColor = headerColor }
        };

        EditorGUILayout.LabelField("Preview", headerStyle);
        EditorGUILayout.Space();

        List<string> soundEffectNames = new List<string>();
        for (int i = 0; i < soundEffects.arraySize; i++)
        {
            soundEffectNames.Add(soundEffects.GetArrayElementAtIndex(i).FindPropertyRelative("sfxName").stringValue);
        }

        previewSfxIndex = EditorGUILayout.Popup("Sound Effect:", previewSfxIndex, soundEffectNames.ToArray());

        if (GUILayout.Button("Preview"))
        {
            SerializedProperty soundEffect = soundEffects.GetArrayElementAtIndex(previewSfxIndex);

            previewAudioSource.volume = soundEffect.FindPropertyRelative("sfxVolume").floatValue;
            previewAudioSource.dopplerLevel = 0;

            float randomPitchModifier = soundEffect.FindPropertyRelative("sfxBasePitchChange").floatValue + Random.Range(-soundEffect.FindPropertyRelative("sfxPitchRange").floatValue, soundEffect.FindPropertyRelative("sfxPitchRange").floatValue);
            previewAudioSource.pitch = 1.0f + randomPitchModifier;

            int randomIndex = Random.Range(0, soundEffect.FindPropertyRelative("sfxSounds").arraySize);
            previewAudioSource.clip = soundEffect.FindPropertyRelative("sfxSounds").GetArrayElementAtIndex(randomIndex).objectReferenceValue as AudioClip;;

            previewAudioSource.Play();
        }

        Divider();

        EditorGUILayout.LabelField("Configuration", headerStyle);
        EditorGUILayout.Space();

        base.OnInspectorGUI();

        serializedObject.ApplyModifiedProperties();
    }

    void Divider()
    {
        int padding = 20;

        EditorGUILayout.Space(padding);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        //EditorGUILayout.Space(padding);
    }
}
#endif
