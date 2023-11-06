using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCharacter : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;
    SwapCharacter swapCharacter;
    SkillManager skillManager;
    // Start is called before the first frame update
    void Start()
    {
        swapCharacter = GetComponent<SwapCharacter>();
        skillManager = GetComponent<SkillManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //TEMPORARY KEYCODE ~ WILL BE TURNED INTO UI MENU BUTTON IN THE FUTURE

        if (Input.GetKeyDown(KeyCode.K))
        {
            SpawnNewCharacter();
        }
    }
    void SpawnNewCharacter()
    {
        GameObject newCharacter = Instantiate(characterPrefab); //WE HAVE TO EVENTUALLY BE ABLE TO SPAWN 4 DIFF TYPES OF SHROOMS, RIGHT NOW THIS IS JUST A SINGLE TYPE
        swapCharacter.characters.Add(newCharacter);
        newCharacter.transform.position = new Vector3(0, 1.2f, 0); //WE CAN SET A SPAWNPOINT IN THE HUB SOMEWHERE
        
        //Temporary im too tired for this
        skillManager.SetSkill(name, 0, newCharacter);
        skillManager.SetSkill(name, 1, newCharacter);
        skillManager.SetSkill(name, 2, newCharacter);

    }
}
