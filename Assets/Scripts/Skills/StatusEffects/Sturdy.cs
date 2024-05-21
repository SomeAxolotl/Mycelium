using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sturdy : MonoBehaviour
{
    //This script does not actually do anything lol
    //Player Controller handles this stuff by checking if the player has this component on them

    private float currSturdyTime = 2;
    public Animator animator;

    public void InitializeSturdy(float duration = 2){
        animator = GetComponentInChildren<Animator>();
        NoHurt();
        StartCoroutine(SturdyCoroutine(duration));
    }

    private IEnumerator SturdyCoroutine(float duration){
        while(currSturdyTime > 0){
            currSturdyTime -= Time.deltaTime;
            NoHurt();
            yield return null;
        }
        Destroy(this);
    }

    private void NoHurt(){
        if(animator != null && animator.GetBool("Hurt") == true){
            animator.SetBool("Hurt", false);
        }
    }
}
