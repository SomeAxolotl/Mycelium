using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class bogedfix : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if (GlobalData.gameIsStarting == false)
        {
            GetComponent<VideoPlayer>().enabled = false;
        }
    }
}
