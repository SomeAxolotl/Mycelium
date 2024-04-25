using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CarcassMuffling : MonoBehaviour
{
    [SerializeField] List<AudioMixerSnapshot> muffleSnapshots;

    [SerializeField] float transitionTime = 0.5f;

    int sporeCountForMuffleStage2 = 2;
    int sporeCountForMuffleStage3 = 5;

    IEnumerator Start()
    {
        yield return null;

        CalculateMuffleSnapshot();
    }

    public void CalculateMuffleSnapshot()
    {
        int characterAmount = GameObject.FindWithTag("PlayerParent").GetComponent<SwapCharacter>().characters.Count;

        AudioMixerSnapshot selectedSnapshot;
        float usedTransitionTime;
        if (characterAmount >= sporeCountForMuffleStage3)
        {
            selectedSnapshot = muffleSnapshots[2];
            usedTransitionTime = transitionTime;
        }
        else if (characterAmount >= sporeCountForMuffleStage2)
        {
            selectedSnapshot = muffleSnapshots[1];
            usedTransitionTime = transitionTime;
        }
        else
        {
            selectedSnapshot = muffleSnapshots[0];
            usedTransitionTime = 0;
        }

        selectedSnapshot.TransitionTo(usedTransitionTime);
        GlobalData.currentAudioMixerSnapshot = selectedSnapshot;
    }
}
