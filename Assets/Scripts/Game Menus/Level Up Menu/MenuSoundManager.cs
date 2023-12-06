using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundManager : MonoBehaviour
{

    public void PlayUIMoveSound()
    {
        
        SoundEffectManager.Instance.PlaySound("UIMove", GameObject.FindWithTag("MainCamera").transform.position);
        
    }
    public void PlayUISelectSound()
    {
    
        SoundEffectManager.Instance.PlaySound("UISelect", GameObject.FindWithTag("MainCamera").transform.position);

    }
}
