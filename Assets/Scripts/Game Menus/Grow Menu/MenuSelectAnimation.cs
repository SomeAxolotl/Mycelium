using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuSelectAnimation : MonoBehaviour
{
    [SerializeField] private bool startSelected = false;
    [SerializeField] private Animator animator;
    [SerializeField] private Color selectedColor;

    private TMP_Text text;
    private Color originalColor;
    
    private void OnEnable()
    {
        if (name == "Skills")
        {
            originalColor = new Color(108f/255f, 222f/255f, 205f/255f);
        }
        else if (GetComponent<TMP_Text>() != null)
        {
            text = GetComponent<TMP_Text>();

            originalColor = new Color(108f / 255f, 222f / 255f, 205f / 255f);
        }

        if(startSelected) animator.SetBool("Selected", true);
    }
    public void PlaySelectAnimation()
    {
        animator.SetBool("Selected", true);

        StartChangeTextColor(selectedColor);
    }
    public void PlayDeselectAnimation()
    {
        animator.SetBool("Selected", false);

        StartChangeTextColor(originalColor);
    }

    private void StartChangeTextColor(Color newColor)
    {
        StopAllCoroutines();

        if(name == "Skills")
        {
            StartCoroutine(ChangeColor(newColor, transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>()));
            StartCoroutine(ChangeColor(newColor, transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>()));

            StartCoroutine(ChangeColor(newColor, transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>()));
            StartCoroutine(ChangeColor(newColor, transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>()));

            StartCoroutine(ChangeColor(newColor, transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>()));
            StartCoroutine(ChangeColor(newColor, transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>()));
        }
        else if (text != null)
        {
            StartCoroutine(ChangeColor(newColor, text));
        }
    }

    IEnumerator ChangeColor(Color newColor, TMP_Text text)
    {
        float totalTime = 0.1f;
        float elapsedTime = 0f;
        float t = 0f;

        Color startingColor = text.color;

        while (elapsedTime < totalTime)
        {
            t = elapsedTime / totalTime;

            text.color = Color.Lerp(startingColor, newColor, t);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        text.color = newColor;
    }
}
