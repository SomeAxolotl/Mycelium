using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Transform target;
    public Image crosshairImage;
    // Start is called before the first frame update
    void Start()
    {
        crosshairImage = GetComponent<Image>();
        crosshairImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(target.position);
        }
    }
}
