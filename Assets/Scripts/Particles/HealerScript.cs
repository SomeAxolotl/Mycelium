using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerScript : MonoBehaviour
{
    [SerializeField] private float healAmount = 1;
    [HideInInspector] public float O_healAmount{
        get{
            return healAmount;
        }
        set{
            healAmount = value;
            float clampedScaleFactor = Mathf.Clamp((healAmount / 100) + 0.05f, minScale, maxScale);
            Vector3 newScale = new Vector3(clampedScaleFactor, clampedScaleFactor, clampedScaleFactor);
            transform.localScale = newScale;
            trail.localScale = newScale;
            trail.GetComponent<TrailRenderer>().widthMultiplier = clampedScaleFactor;
        }
    }
    public float minScale = 0.15f;
    public float maxScale = 0.5f;
    [HideInInspector] public Transform target;
    [SerializeField] private Transform trail;
    private bool hasActivated = false;
    private float duration = 0.5f;
    private float arcHeight = 2f;

    public bool targetPlayer = true;
    private Transform player;

    void Awake(){
        if(targetPlayer){
            target = GameObject.FindWithTag("currentPlayer").transform;
            player = target.parent;
        }
        if(target != null){
            StartCoroutine(MoveToTarget());
        }
    }

    IEnumerator MoveToTarget(){
        Vector3 startPosition = transform.position;
        float currTime = 0f;
        while(currTime < duration){
            Vector3 currentPosition = transform.position = Vector3.Lerp(startPosition, target.position, currTime / duration);
            float t = currTime / duration;
            float arc = Mathf.Sin(t * Mathf.PI) * arcHeight;
            currentPosition.y += arc;
            transform.position = currentPosition;
            currTime += Time.deltaTime;
            yield return null;
        }
        if(!hasActivated){
            HealTarget();
            hasActivated = true;
        }
    }

    void HealTarget(){
        if(player != null){
            player.GetComponent<PlayerHealth>().PlayerHeal(healAmount);
        }
        trail.parent = null;
        Destroy(gameObject);
    }
}
