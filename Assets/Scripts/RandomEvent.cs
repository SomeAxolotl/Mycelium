using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomEvent : MonoBehaviour
{
    [SerializeField][Tooltip("A list of events to trigger, either on start or ")] private List<REvent> Events;
    void Start()
    {
        foreach (REvent _event in Events)
        {
            if (UnityEngine.Random.Range(0f,1f) > _event.PercentChance/100f)
            {
                _event.goingToActivate = false;
            }

            if(_event.goingToActivate && !_event.TriggerOnEnter)
            {
                _event.RemoveObjects();
                _event.ActivateObjects();
            }     
        }   
    }

    void OnTriggerEnter(Collider collider){
        foreach (REvent _event in Events)
        {
            if(_event.TriggerOnEnter && _event.goingToActivate && collider.tag == "currentPlayer")
            {
                _event.RemoveObjects();
                _event.ActivateObjects();
            }
        }
    }
}

[System.Serializable]
public class REvent
{
    [Range(1,100)][Tooltip("The odds of the event triggering")] public float PercentChance;
    public List<GameObject> ObjectsToActivate;
    public List<GameObject> ObjectsToRemove;
    [Tooltip("Triggers on entering a collider, rather than on start.")] public bool TriggerOnEnter = false;
    [Tooltip("Affects whether or not you want objects scale to be random")] public bool changeScale = false;
    [Tooltip("Minimum Scale Modifier")] public float localScaleMin = 0.75f;
    [Tooltip("Maximum Scale Modifier")] public float localScaleMax = 1.25f;
    [NonSerialized] public bool goingToActivate = true;
    public void ActivateObjects(){
        //Debug.Log("Activating!");
        foreach(GameObject obj in ObjectsToActivate)
        {
            obj.SetActive(true);
            if(changeScale)
            {
                float scaleModifier = UnityEngine.Random.Range(localScaleMin, localScaleMax);
                obj.transform.localScale *= scaleModifier;
            }
        }
    }
    public void RemoveObjects(){
        //Debug.Log("Removing!");
        foreach (GameObject obj in ObjectsToRemove)
        {
            obj.SetActive(false);
        }
    }
}

