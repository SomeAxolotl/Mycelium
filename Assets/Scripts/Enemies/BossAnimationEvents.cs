using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEvents : MonoBehaviour
{
    void Finish()
    {
        GameObject.Find("CreditsPlayer").GetComponent<CreditsPlayer>().StartPlayCredits();
        GameObject boss = GameObject.Find("Rival Colony Leader");
        ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", GameObject.Find("Rival Colony Leader").GetComponent<BossHealth>().nutrientDrop, 0.1f, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
        //boss.SetActive(false);
    }
}