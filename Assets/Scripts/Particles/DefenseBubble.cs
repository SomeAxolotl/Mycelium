using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBubble : MonoBehaviour
{
    private Renderer ren;
    private Material baseMaterial;

    private bool acceptNewStates = true;
    public bool isBubble = true;
    //I'm sorry this is a crime
    public bool work = true;

    public void Awake(){
        ren = this.gameObject.GetComponent<Renderer>();
        ren.enabled = false;
        baseMaterial = ren.material;
        //Make sure the default is not 0
        maxIntensity = 1;
        ren.material.SetFloat("_Intensity", 0);
        currFlash = 0;
        AdjustSize();
    }

    public IEnumerator currentState;
    public void SwitchStatement(int num){
        if(!work){return;}
        if(failedAdjust){
            AdjustSize();
        }
        if(!acceptNewStates){return;}
        if(currentState != null){
            StopCoroutine(currentState);
        }
        switch(num){
        case 0:
            currentState = SwitchState(true);
            break;
        case 1:
            currentState = Active();
            break;
        case 2:
            if(currFlash == 0){break;}
            currentState = SwitchState(false);
            break;
        case 3:
            ren.material.SetFloat("_Intensity", 0);
            currFlash = 0;
            ren.enabled = false;
            break;
        case 4:
            ren.material.SetFloat("_Intensity", maxIntensity);
            currFlash = maxIntensity;
            ren.enabled = true;
            break;
        }
        if(currentState != null){
            StartCoroutine(currentState);
        }
    }

    float maxIntensity;
    public float timeLeft = 1;
    float timePassed = 0f;
    float currFlash = 0f;
    IEnumerator SwitchState(bool turnOn, float switchTime = 0.2f){
        //Comfortably change
        while(timePassed < switchTime){
            timePassed += Time.deltaTime;
            timeLeft -= Time.deltaTime;
            if(turnOn){
                currFlash = Mathf.Lerp(0, maxIntensity, (timePassed / switchTime));
                ren.material.SetFloat("_Intensity", currFlash);
            }else{
                currFlash = Mathf.Lerp(maxIntensity, 0, (timePassed / switchTime));
                ren.material.SetFloat("_Intensity", currFlash);
            }
            yield return null;
        }
        if(turnOn){currFlash = maxIntensity; ren.enabled = true;}else{currFlash = 0; ren.enabled = false;}
        ren.material.SetFloat("_Intensity", currFlash);
        timePassed = 0f;
        currentState = null;
        if(turnOn){
            SwitchStatement(1);
        }
    }
    IEnumerator Active(float switchTime = 0.2f){
        //Could add some overflow code here to make it so extra time decreases switch time to make it slightly more consistant 
        //Might already be in the else code of the switch state
        while(timeLeft > switchTime){
            timePassed += Time.deltaTime;
            timeLeft -= Time.deltaTime;
            yield return null;
        }  
        timePassed = 0f;
        currentState = null;
        //Turn off the bubble
        SwitchStatement(2);
    }

    public void DisableBubble(){
        if(!work){return;}
        SwitchStatement(2);
        acceptNewStates = false;
    }

    private bool failedAdjust = false;
    public void AdjustSize(){
        Transform parent = transform.parent;
        if(parent == null){Debug.Log("No parent"); failedAdjust = true; return;}
        if(isBubble){
            if(parent.tag == "Player" || parent.tag == "currentPlayer" || parent.name == "SporeModel" || parent.name.Contains("Mushy")){
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.3f, transform.localPosition.z - 0.1f);
                transform.localScale = new Vector3(1.35f, 1.35f, 1.6f);
                return;
            }
            if(parent.name.Contains("Beetle")){
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.5f, transform.localPosition.z + 0.15f);
                transform.localScale = new Vector3(2f, 2.5f, 2f);
                return;
            }
            if(parent.name.Contains("Stickbug")){
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.5f, transform.localPosition.z + 0.15f);
                transform.localScale = new Vector3(1.75f, 4f, 1.75f);
                return;
            }
            if(parent.name.Contains("Crab")){
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.5f, transform.localPosition.z);
                transform.localScale = new Vector3(5f, 5f, 5f);
                return;
            }
            if(parent.name.Contains("Sporemother")){
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.5f, transform.localPosition.z);
                transform.localScale = new Vector3(3.5f, 3.5f, 4.25f);
                return;
            }
            if(parent.name.Contains("Isopod")){
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.5f, transform.localPosition.z + 0.3f);
                transform.localScale = new Vector3(2.8f, 3.6f, 2.8f);
                return;
            }

            //Default size for enemies
            if(parent.tag == "Enemy"){
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.5f, transform.localPosition.z + 0.15f);
                transform.localScale = new Vector3(2f, 2.5f, 2f);
                return;
            }
        }else{
            if(parent.tag == "Player" || parent.tag == "currentPlayer" || parent.name == "SporeModel" || parent.name.Contains("Mushy")){
                //transform.GetChild(0).localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.3f, transform.localPosition.z - 0.1f);
                //transform.GetChild(0).localScale = Vector3.one;
                return;
            }
            if(parent.name.Contains("Beetle")){
                //transform.GetChild(0).localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.5f, transform.localPosition.z + 0.6f);
                //transform.GetChild(0).localScale = Vector3.one * 0.6f;
                return;
            }
            if(parent.name.Contains("Stickbug")){
                //transform.GetChild(0).localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 1f, transform.localPosition.z + 1.5f);
                //transform.GetChild(0).localScale = Vector3.one * 0.6f;
                return;
            }
            if(parent.name.Contains("Crab")){
                transform.GetChild(0).localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 2f, transform.localPosition.z);
                transform.GetChild(0).localScale = new Vector3(3f, 3f, 3f);
                return;
            }
        }
    }
}
