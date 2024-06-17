using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitBase : MonoBehaviour
{
    [HideInInspector] public string traitName;
    [HideInInspector] public string traitDesc;

    [HideInInspector] public GameObject player;
    [HideInInspector] public CharacterStats characterStats;
    [HideInInspector] public HUDStats hudStats;
    [SerializeField] private GameObject playerParent;
    //Automatically updates if the player has a parent when referenced 
    [HideInInspector] public GameObject O_playerParent{
        get{
            UpdateParent();
            return playerParent;
        }
    }
    [SerializeField] private PlayerController controller;
    [HideInInspector] public PlayerController O_controller{
        get{
            if(controller == null){
                UpdateParent();
            }
            return controller;
        }
    }
    [SerializeField] private PlayerHealth health;
    [HideInInspector] public PlayerHealth O_health{
        get{
            if(health == null){
                UpdateParent();
            }
            return health;
        }
    }
    [HideInInspector] public Rigidbody rb;

    private void UpdateParent(){
        if(player.transform.parent != null){
            playerParent = player.transform.parent.gameObject;
            health = playerParent.GetComponent<PlayerHealth>();
            controller = playerParent.GetComponent<PlayerController>();
        }else{
            playerParent = null;
            health = null;
            controller = null;
        }
    }

    private void Awake(){
        player = gameObject;
        characterStats = player.GetComponent<CharacterStats>();
        hudStats = GameObject.Find("HUD").GetComponent<HUDStats>();
        rb = player.GetComponent<Rigidbody>();

        Actions.SwappedCharacter += SwappedCharacter;
        if(gameObject.tag == "currentPlayer"){
            SwappedCharacter(null, gameObject);
        }
    }

    //On the trait being put on the spore
    public virtual void Start(){
    }
    public virtual void OnDisable(){
        Actions.SwappedCharacter -= SwappedCharacter;
        SporeUnselected();
    }

    private void SwappedCharacter(GameObject oldSpore, GameObject newSpore){
        Debug.Log("Old Spore: " + oldSpore + "       New Spore: " + newSpore);
        if(newSpore == gameObject){
            SporeSelected();
        }
        if(oldSpore == gameObject){
            SporeUnselected();
        }
    }

    public virtual void SporeSelected(){

    }
    public virtual void SporeUnselected(){

    }
}
