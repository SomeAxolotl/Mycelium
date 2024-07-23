using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Blinding;

public class Blinding : EnemyAttributeBase, IBlindingEffect
{
    private float blindingDuration = 5f; // Duration of the blinding effect in seconds
    private bool isBlindingActive = false;
    private Coroutine blindingCoroutine;

    protected override void OnInitialize() { }

    public void ApplyBlindingEffect(float duration)
    {
        if (blindingCoroutine != null)
        {
            StopCoroutine(blindingCoroutine);
        }
        blindingCoroutine = StartCoroutine(BlindingEffectCoroutine(1f));
    }

    private IEnumerator BlindingEffectCoroutine(float duration)
    {
        isBlindingActive = true;
        HUDController hudController = GameObject.Find("HUD").GetComponent<HUDController>();

        if (hudController != null)
        {
            yield return hudController.StartCoroutine(hudController.BlackFade(true));
            yield return new WaitForSeconds(1f);
            yield return hudController.StartCoroutine(hudController.BlackFade(false));
        }

        isBlindingActive = false;
        blindingCoroutine = null;
    }
    public interface IBlindingEffect
    {
        void ApplyBlindingEffect(float duration);
    }
}
