using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerScript : MonoBehaviour
{
    public float healAmount = 1;
    [HideInInspector] public Transform target;
    private bool hasActivated = false;
    private float duration = 0.6f;
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
        Destroy(gameObject);
    }
}
