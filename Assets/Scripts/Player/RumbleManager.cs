using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleManager : MonoBehaviour
{
    public static RumbleManager Instance;
    private Gamepad pad;
    private IEnumerator currentRumble;
    private List<RumbleInfo> rumbles = new List<RumbleInfo>();
    private RumbleInfo highestRumble;
    //Changes how intense controller shakes are
    public float shakeMult = 1;

    private void Awake(){
        if(Instance != null && Instance != this){
            Destroy(this);
        }else{
            Instance = this;
        }
    }

    public void RumblePulse(float duration = 1, float lowFrequency = 0.2f, float highFrequency = 0.2f){
        InputDevice inputDevice = Gamepad.current;
        if(inputDevice is Gamepad gamepad){
            pad = gamepad;
            //Creates new instance of a rumble
            RumbleInfo newRumble = new RumbleInfo();
            newRumble.rumbleLow = lowFrequency * shakeMult;
            newRumble.rumbleHigh = highFrequency * shakeMult;
            newRumble.currTime = duration;
            rumbles.Add(newRumble);
            PickHighestRumble();
            StartCoroutine(StopRumble(newRumble));
        }
    }

    private void PickHighestRumble(){
        if(rumbles.Count == 1){
            pad.SetMotorSpeeds(rumbles[0].rumbleLow, rumbles[0].rumbleHigh);
        }else if(rumbles.Count > 1){
            rumbles.Sort(delegate(RumbleInfo a, RumbleInfo b)
            {return b.rumbleLow.CompareTo(a.rumbleLow);});
            pad.SetMotorSpeeds(rumbles[0].rumbleLow, rumbles[0].rumbleHigh);
        }else{
            pad.SetMotorSpeeds(0, 0);
        }
    }

    private IEnumerator StopRumble(RumbleInfo rumble){
        while(rumble.currTime > 0){
            rumble.currTime -= Time.unscaledDeltaTime;
            yield return null;
        }
        rumbles.Remove(rumble);
        PickHighestRumble();
    }
}

public class RumbleInfo{
    public float rumbleLow = 0;
    public float rumbleHigh = 0;
    public float currTime = 1;
}
