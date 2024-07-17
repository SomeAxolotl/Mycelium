using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinding : EnemyAttributeBase
{
    private float blindingDuration = 5f; // Duration of the blinding effect in seconds
    private bool isBlindingActive = false;

    protected override void OnInitialize() { }

    public void ApplyBlindingEffect()
    {
        if (!isBlindingActive)
        {
            StartCoroutine(BlindingEffectCoroutine());
        }
    }

    private IEnumerator BlindingEffectCoroutine()
    {
        isBlindingActive = true;
        FadeManager.Instance.StartFade(blindingDuration);
        yield return new WaitForSeconds(blindingDuration);
        isBlindingActive = false;
    }
}
