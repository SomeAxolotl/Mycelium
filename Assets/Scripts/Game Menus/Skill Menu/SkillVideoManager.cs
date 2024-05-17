using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SkillVideoManager : MonoBehaviour
{
    public static SkillVideoManager Instance;

    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] RawImage rawVideoImage;
    [SerializeField] List<SkillVideo> skillVideos = new List<SkillVideo>();

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

    public void PlaySkillVideo(SkillManager.StatSkills skillName)
    {
        rawVideoImage.gameObject.SetActive(true);

        bool skillVideoFound = false;
        foreach (SkillVideo skillVideo in skillVideos)
        {
            if (skillName == skillVideo.skillName)
            {
                skillVideoFound = true;

                videoPlayer.clip = skillVideo.skillVideoClip;
                videoPlayer.Play();
            }
        }

        if (!skillVideoFound)
        {
            Debug.LogError($"No skill video found with the name: {skillName})");
        }
    }
    public void StopSkillVideo()
    {
        rawVideoImage.gameObject.SetActive(false);

        videoPlayer.Stop();
    }

    [System.Serializable]
    class SkillVideo
    {
        public VideoClip skillVideoClip;
        public SkillManager.StatSkills skillName;
    }
}
