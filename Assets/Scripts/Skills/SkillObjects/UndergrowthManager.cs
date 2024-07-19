using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergrowthManager : MonoBehaviour
{
    public List<GameObject> roots;
    [HideInInspector] public List<Collider> hitTargets;
    Undergrowth undergrowth;

    [SerializeField] private float rootDuration = 2;

    void Awake(){
        StartCoroutine(Timer());
    }
    void Start(){
        undergrowth = GameObject.FindWithTag("currentPlayer").GetComponentInChildren<Undergrowth>();
    }

    IEnumerator Timer(){
        while(roots.Count > 0){
            yield return null;
        }
        Destroy(gameObject);
    }

    public void ProcessTarget(Collider other){
        if(!hitTargets.Contains(other)){
            //Debug.Log("Hitting: " + other.gameObject.name);
            hitTargets.Add(other);
            if(other.GetComponent<EnemyHealth>() != null){
                other.GetComponent<EnemyHealth>().EnemyTakeDamage(undergrowth.finalSkillValue / 5);
                Root rootEffect = other.gameObject.AddComponent<Root>();
                rootEffect.duration = rootDuration;
            }
        }
    }
}
