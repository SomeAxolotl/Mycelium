using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sturdy : MonoBehaviour
{
    //This script does not actually do anything lol
    //Player Controller handles this stuff by checking if the player has this component on them

    float currSturdyTime = 2;

    public void StartSturdy(float duration = 2){
        StartCoroutine(SturdyCoroutine(duration));
    }

    private IEnumerator SturdyCoroutine(float duration){
        while(currSturdyTime > 0){
            currSturdyTime -= Time.deltaTime;
            yield return null;
        }
        Destroy(this);
    }
}
