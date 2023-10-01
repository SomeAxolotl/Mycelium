using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextCode : MonoBehaviour
{
    public Text Nametext;
    public Text ObjectiveText;
    private bool isTextVisible = false;
    // Start is called before the first frame update
    void Start()
    {
        Nametext.enabled = false;
        ObjectiveText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isTextVisible = !isTextVisible;
            Nametext.enabled = isTextVisible;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            isTextVisible = !isTextVisible;
            ObjectiveText.enabled = isTextVisible;
        }
    }
}
