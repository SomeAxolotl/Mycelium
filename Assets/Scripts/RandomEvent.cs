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
        foreach (REvent e in Events)
        {
            if (UnityEngine.Random.Range(0f,1f) > e.PercentChance/100f)
            {
                e.goingToActivate = false;
            }

            if(e.goingToActivate && !e.TriggerOnEnter)
            {
                e.RemoveObjects();
                e.ActivateObjects();
            }     
        }   
    }

    void OnTriggerEnter(Collider collider){
        foreach (REvent e in Events)
        {
            if(e.TriggerOnEnter && e.goingToActivate && collider.tag == "currentPlayer")
            {
                e.RemoveObjects();
                e.ActivateObjects();
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
    [NonSerialized] public bool goingToActivate = true;
    public void ActivateObjects(){
        //Debug.Log("Activating!");
        foreach(GameObject obj in ObjectsToActivate)
            obj.SetActive(true);
    }
    public void RemoveObjects(){
        //Debug.Log("Removing!");
        foreach (GameObject obj in ObjectsToRemove)
            obj.SetActive(false);
    }
}

