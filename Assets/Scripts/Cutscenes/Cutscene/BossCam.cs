using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;

public class BossCam : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vCam;

    private CinemachineTrackedDolly vCamDolly;
    private CinemachineBrain mainBrain;
    private GameObject boss;
    private Transform bossHead;
    private GameObject playerParent;
    // Start is called before the first frame update
    void Start()
    {
        vCamDolly = vCam.GetCinemachineComponent<CinemachineTrackedDolly>();
        mainBrain = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineBrain>();
        boss = GameObject.Find("Rival Colony Leader");
        bossHead = GameObject.Find("Rival Colony Leader").transform.Find("Armature.001").GetChild(0).GetChild(1).GetChild(0).GetChild(0);
        vCam.m_LookAt = bossHead;
        playerParent = GameObject.FindWithTag("PlayerParent");
    }
    public void StartBossCutscene()
    {
        StartCoroutine(BossCutscene());
    }
    IEnumerator BossCutscene()
    {
        float time = 16f;
        float elapsedTime = 0f;
        float t = 0f;

        float oldBlendTime = mainBrain.m_DefaultBlend.m_Time;
        float newBlendTime = 1f;
        
        vCam.enabled = true;
        GlobalData.isAbleToPause = false;
        mainBrain.m_DefaultBlend.m_Time = newBlendTime;

        while (elapsedTime < time)
        {
            t = elapsedTime / time;

            vCamDolly.m_PathPosition = Mathf.Lerp(0f, 1f, t);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        vCamDolly.m_PathPosition = 1;

        vCam.enabled = false;
        GlobalData.isAbleToPause = true;
        yield return new WaitForSeconds(newBlendTime);
        playerParent.GetComponent<PlayerController>().playerActionsAsset.Player.Enable();
        playerParent.GetComponent<PlayerAttack>().playerActionsAsset.Player.Enable();
        boss.GetComponent<TempMovement>().enabled = true;
        boss.GetComponent<MonsterBossAttack>().enabled = true;
        boss.GetComponent<Animator>().SetBool("IsAttacking", false);
        mainBrain.m_DefaultBlend.m_Time = oldBlendTime;
    }
}
