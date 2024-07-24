using Cinemachine;
using UnityEngine;

public class BossAnimationEvents : MonoBehaviour
{
    private enum BossAttack { Left, Right, Smash, Spin}

    [SerializeField] private CinemachineImpulseSource impulseSource;

    [SerializeField] private AttackTargetSolver leftATS;
    [SerializeField] private AttackTargetSolver rightATS;

    [SerializeField] private Transform leftShoulder;
    [SerializeField] private Transform rightShoulder;

    [HideInInspector] public bool isInDangerZone = false;

    private Transform player;
    private Transform camtracker;

    //==================================================================================
    //                             -=-=-IMPORTANT-=-=-
    //PRETTY MUCH EVERY FUNCTION IN THIS SCRIPT IS BEING CALLED BY VARIOUS ANIMATIONS
    //                          VIA ANIMATION EVENTS! -ryan
    //==================================================================================

    private void Start()
    {
        player = GameObject.FindWithTag("currentPlayer").GetComponent<Transform>();
        camtracker = GameObject.FindWithTag("Camtracker").GetComponent<Transform>();
    }

    void Finish() 
    {
        GameObject.Find("CreditsPlayer").GetComponent<CreditsPlayer>().StartPlayCredits();
        GameObject boss = GameObject.Find("Rival Colony Leader");
        ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", GameObject.Find("Rival Colony Leader").GetComponent<BossHealth>().nutrientDrop, 0.1f, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
        //boss.SetActive(false);
    }

    void PositionAttackTarget(BossAttack attack)
    {
        //Debug.Log($"Danger Zone: {isInDangerZone}");
        //Debug.Log($"Left Dist: {Vector3.Distance(leftShoulder.position, player.position)} | Right Dist: {Vector3.Distance(rightShoulder.position, player.position)}");

        switch (attack)
        {
            case BossAttack.Left:
                if(isInDangerZone == true && Vector3.Distance(leftShoulder.position, player.position) < 8.8f)
                {
                    leftATS.GoToPlayer();
                }
                else
                {
                    leftATS.StartFollowArm();
                }
                break;

            case BossAttack.Right:
                if (isInDangerZone == true && Vector3.Distance(rightShoulder.position, player.position) < 10.4f)
                {
                    rightATS.GoToPlayer();
                }
                else
                {
                    rightATS.StartFollowArm();
                }
                break;

            case BossAttack.Smash:
                if (isInDangerZone == true && Vector3.Distance(leftShoulder.position, player.position) < 10.9f && Vector3.Distance(rightShoulder.position, player.position) < 11.1f)
                {
                    leftATS.GoToPlayerSmash(true);
                    rightATS.GoToPlayerSmash(false);
                }
                else
                {
                    leftATS.StartFollowArm();
                    rightATS.StartFollowArm();
                }
                break;

            default:
                break;
        }
    }

    void ShakeCamera() 
    {
        CameraShakeManager.instance.ShakeCamera(impulseSource);
    }

    void PlayBoomSound() 
    {
        float distance = Vector3.Distance(camtracker.position, gameObject.transform.position);
        Vector2 from = new Vector2(5f, 35f);
        Vector2 to = new Vector2(-0.1f, -0.5f);

        //remap
        float newVolume = to.x + (distance - from.x) * (to.y - to.x) / (from.y - from.x);
        newVolume = Mathf.Clamp(newVolume, Mathf.Min(to.x, to.y), Mathf.Max(to.x, to.y));

        //Debug.Log("Distance: " + distance + "\tVolume: " + newVolume);

        SoundEffectManager.Instance.PlaySound("Boss Boom", camtracker, newVolume);
    }

    void PlayBossMusic()
    {
        GameObject.Find("BackgroundMusicPlayer").GetComponent<BarrensMuffling>().StartBossMusic();
    }
}