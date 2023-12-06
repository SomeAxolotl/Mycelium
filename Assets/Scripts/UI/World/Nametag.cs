using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Nametag : MonoBehaviour
{
    [SerializeField] private Canvas nametagCanvas;
    [SerializeField] private TMP_Text sporeNameText;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        nametagCanvas.GetComponent<Canvas>().worldCamera = mainCamera;
    }

    public void SetSporeNametagText(string name)
    {
        sporeNameText.text = name;
    }

    public void ShowNametag()
    {
        sporeNameText.color = new Color(255,255,255,1);
    }
    public void HideNametag()
    {
        sporeNameText.color = new Color(255,255,255,0);
    }

    void Update()
    {
        nametagCanvas.transform.rotation = Quaternion.LookRotation(nametagCanvas.transform.position - mainCamera.transform.position);
    }
}
