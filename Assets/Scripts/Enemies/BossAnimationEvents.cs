using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEvents : MonoBehaviour
{
    void IntroAnim()
    {
        GameObject boss = GameObject.Find("Rival Colony Leader");
        boss.GetComponent<MonsterBossAttack>().enabled = true;
        GameObject player = GameObject.FindWithTag("PlayerParent");
        player.GetComponent<PlayerController>().playerActionsAsset.Player.Enable();
        player.GetComponent<PlayerAttack>().playerActionsAsset.Player.Enable();
        GameObject colliderObj = GameObject.Find("StartAnimBoss");
        colliderObj.SetActive(false);
    }
    
    void Finish()
    {
        GameObject.Find("CreditsPlayer").GetComponent<CreditsPlayer>().StartPlayCredits();
        GameObject boss = GameObject.Find("Rival Colony Leader");
        ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", GameObject.Find("Rival Colony Leader").GetComponent<BossHealth2>().nutrientDrop, 0.1f, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
        boss.SetActive(false);
    }
}