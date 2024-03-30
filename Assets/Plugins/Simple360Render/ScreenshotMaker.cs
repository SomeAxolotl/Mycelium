using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScreenshotMaker : MonoBehaviour
{
    private enum FileType { JPEG, PNG }

    [SerializeField][Min(1)][Tooltip("Must be a power of 2 and not be bigger than 8192")] private int width;
    [SerializeField] private FileType fileType;
    [SerializeField] private Camera cameraToUse;


    public void TakeScreenshot()
    {
        //if(Application.isPlaying == false)
        //{
        //    Debug.LogError("Must be in Play Mode to take 360 Screenshot");
        //    return;
        //}

        if(Mathf.CeilToInt(Mathf.Log(width, 2) / Mathf.Log(2, 2)) != Mathf.FloorToInt(Mathf.Log(width, 2) / Mathf.Log(2, 2)))
        {
            Debug.LogError("Width must be a power of 2");
            return;
        }
        else if(width > 8192)
        {
            Debug.LogError("Width must not be bigger than 8192");
            return;
        }

        if (cameraToUse == null)
        {
            Debug.LogError("cameraToUse is null");
            return;
        }

        bool type = false;
        if (fileType == FileType.JPEG)
        {
            type = true;
        }

        byte[] pictureBytes = I360Render.Capture(width, type, cameraToUse, true);

        if(pictureBytes != null)
        {
            string filePath = Application.dataPath + "/Plugins/Simple360Render/Screenshots/360Shot_" + Timestamp() + (type ? ".jpeg" : ".png");
            File.WriteAllBytes(filePath, pictureBytes);
            Debug.Log("360 Screenshot saved to" + filePath, gameObject);
        }
    }

    private string Timestamp()
    {
        string timestamp = DateTime.Now.ToString();

        timestamp = timestamp.Replace("/", "_");
        timestamp = timestamp.Replace(":", "-");

        return timestamp;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ScreenshotMaker))]
class ScreenshotMakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Setup Stuff
        base.OnInspectorGUI();

        var screenshotMaker = (ScreenshotMaker)target;
        if (screenshotMaker == null) return;

        //Actual Stuff
        if (GUILayout.Button("Capture Screenshot"))
        {
            screenshotMaker.TakeScreenshot();
        }


    }
}
#endif
