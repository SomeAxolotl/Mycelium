using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;

public class BossCam : MonoBehaviour
{
    public static BossCam Instance;

    [SerializeField] private CinemachineVirtualCamera introVCam;
    [SerializeField] private CinemachineVirtualCamera deathVCam;

    private CinemachineTrackedDolly introVCamDolly;
    private CinemachineTrackedDolly deathVCamDolly;
    private CinemachineBrain mainBrain;
    private GameObject boss;
    private Animator bossAnimator;
    private Transform bossHead;
    private GameObject playerParent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        introVCamDolly = introVCam.GetCinemachineComponent<CinemachineTrackedDolly>();
        deathVCamDolly = deathVCam.GetCinemachineComponent<CinemachineTrackedDolly>();

        mainBrain = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineBrain>();
        boss = GameObject.Find("Rival Colony Leader");
        bossAnimator = boss.GetComponent<Animator>();
        bossHead = boss.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        introVCam.m_LookAt = bossHead;
        deathVCam.m_LookAt = bossHead;

        playerParent = GameObject.FindWithTag("PlayerParent");

        transform.position = boss.transform.position;
    }
    public void StartBossIntroCutscene()
    {
        StartCoroutine(BossIntroCutscene());
    }

    public void StartBossDeathCutscene()
    {
        StartCoroutine(BossDeathCutscene());
    }

    IEnumerator BossIntroCutscene()
    {
        SoundEffectManager.Instance.PlaySound("Boss Intro", GameObject.FindWithTag("Camtracker").transform, 0, 1, 200);

        float oldBlendTime = mainBrain.m_DefaultBlend.m_Time;
        float newBlendTime = 1f;
        
        introVCam.enabled = true;
        GlobalData.isAbleToPause = false;

        mainBrain.m_DefaultBlend.m_Time = newBlendTime;

        while (bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("Intro") == true)
        {
            introVCamDolly.m_PathPosition = Mathf.Lerp(0f, 1f, bossAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);

            yield return null;
        }

        GameObject.Find("BackgroundMusicPlayer").GetComponent<AudioSource>().Play();

        introVCamDolly.m_PathPosition = 1;

        introVCam.enabled = false;
        GlobalData.isAbleToPause = true;

        yield return new WaitForSeconds(newBlendTime);
        mainBrain.m_DefaultBlend.m_Time = oldBlendTime;

        playerParent.GetComponent<PlayerController>().playerActionsAsset.Player.Enable();
        playerParent.GetComponent<PlayerAttack>().playerActionsAsset.Player.Enable();
        boss.GetComponent<TempMovement>().enabled = true;
        boss.GetComponent<MonsterBossAttack>().enabled = true;
        boss.GetComponent<Animator>().SetBool("IsAttacking", false);
    }

    IEnumerator BossDeathCutscene()
    {
        float oldBlendTime = mainBrain.m_DefaultBlend.m_Time;
        float newBlendTime = 1f;
        float tOffset = 0.3f;
        CinemachineComposer composer = deathVCam.GetCinemachineComponent<CinemachineComposer>();

        while (bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death") == false)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, boss.transform.eulerAngles.y + 180, transform.eulerAngles.z);
            yield return null;
        }

        deathVCam.enabled = true;
        GlobalData.isAbleToPause = false;

        mainBrain.m_DefaultBlend.m_Time = newBlendTime;

        while (bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death") == true)
        {
            deathVCamDolly.m_PathPosition = Mathf.Lerp(0f, 1f, bossAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime + tOffset);
            composer.m_TrackedObjectOffset.y = Mathf.Lerp(0f, 4f, bossAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime + tOffset);
            composer.m_HorizontalDamping = Mathf.Lerp(1.5f, 0.5f, bossAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime + tOffset);
            composer.m_VerticalDamping = Mathf.Lerp(1.5f, 0.5f, bossAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime + tOffset);
            deathVCam.m_Lens.FieldOfView = Mathf.Lerp(60f, 35f, bossAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime + tOffset);

            yield return null;
        }

        deathVCamDolly.m_PathPosition = 1;

        deathVCam.enabled = false;

        yield return new WaitForSeconds(newBlendTime);
        mainBrain.m_DefaultBlend.m_Time = oldBlendTime;
    }
}
