using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBubble : MonoBehaviour
{
    private Renderer ren;
    private Material baseMaterial;

    public void Awake(){
        ren = this.gameObject.GetComponent<Renderer>();
        baseMaterial = ren.material;
        //Make sure the default is not 0
        maxIntensity = baseMaterial.GetFloat("_Intensity");
        AdjustSize();
    }

    public void AdjustSize(){
        Transform parent = transform.parent;
        if(parent.tag == "Player" || parent.tag == "currentPlayer" || parent.name == "SporeModel" || parent.name.Contains("Mushy")){
            //Adjusts size and position for players
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.3f, transform.localPosition.z - 0.1f);
            transform.localScale = new Vector3(1.35f, 1.35f, 1.6f);
            return;
        }
        if(parent.name.Contains("Beetle")){
            //Adjusts size and position for players
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.5f, transform.localPosition.z + 0.15f);
            transform.localScale = new Vector3(2f, 2.5f, 2f);
            return;
        }
        if(parent.name.Contains("Stickbug")){
            //Adjusts size and position for players
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.5f, transform.localPosition.z + 0.15f);
            transform.localScale = new Vector3(1.75f, 4f, 1.75f);
            return;
        }
        if(parent.name.Contains("Crab")){
            //Adjusts size and position for players
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.5f, transform.localPosition.z);
            transform.localScale = new Vector3(5f, 5f, 5f);
            return;
        }
        if(parent.name.Contains("Leader")){
            //Adjusts size and position for players
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.5f, transform.localPosition.z);
            transform.localScale = new Vector3(5.25f, 5.25f, 15f);
            return;
        }

        //Default size for enemies
        if(parent.tag == "Enemy"){
            //Adjusts size and position for players
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.5f, transform.localPosition.z + 0.15f);
            transform.localScale = new Vector3(2f, 2.5f, 2f);
            return;
        }
    }

    public IEnumerator currentState;
    public void SwitchStatement(int num){
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
            //Don't bother if it is already off
            if(currFlash == 0){break;}
            currentState = SwitchState(false);
            break;
        case 3:
            ren.material.SetFloat("_Intensity", 0);
            break;
        case 4:
            ren.material.SetFloat("_Intensity", maxIntensity);
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
        if(turnOn){currFlash = maxIntensity;}else{currFlash = 0;}
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
}
