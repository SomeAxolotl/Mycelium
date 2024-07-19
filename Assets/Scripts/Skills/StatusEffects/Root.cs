using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    private string particlePath = "Effects/RootParticles";
    [SerializeField] private GameObject rootParticles;
    private GameObject rootInstance;
    private IEnumerator timer;

    public GameObject undergrowthCaughtParticles;
    public float duration;

    void Awake(){
        rootParticles = Resources.Load<GameObject>(particlePath);
    }

    void Start(){
        SpeedChange speedChangeEffect = gameObject.AddComponent<SpeedChange>();
        speedChangeEffect.InitializeSpeedChange(duration, -100);
        if(rootParticles != null){
            rootInstance = Instantiate(rootParticles) as GameObject;
            rootInstance.transform.position = this.transform.position;
            rootInstance.transform.parent = this.transform;
            GrowToSize grow = rootInstance.GetComponent<GrowToSize>();
            if(grow != null){
                float sizeDifference = transform.localScale.x / 6.5f;
                float sizeBase = 0.75f;
                float targetSize = sizeBase + sizeDifference;
                if(name.Contains("Crab")){
                    targetSize *= 2;
                }
                grow.targetScale = new Vector3(targetSize, targetSize, targetSize);
            }else{
                Debug.Log("What happened dude...");
            }
        }else{
            Debug.Log("Particles not set for root effect");
        }
        if(timer == null){
            timer = RootCoroutine();
            StartCoroutine(timer);
        }
    }

    private IEnumerator RootCoroutine(){
        yield return new WaitForSeconds(duration);
        Destroy(this);
    }

    void OnDestroy(){
        rootInstance.transform.parent = null;
        ShrinkToSize shrink = rootInstance.GetComponent<ShrinkToSize>();
        if(shrink != null){
            shrink.enabled = true;
        }else{
            Debug.Log("What happened dude...");
        }
        Destroy(rootInstance, 1);
    }
}
