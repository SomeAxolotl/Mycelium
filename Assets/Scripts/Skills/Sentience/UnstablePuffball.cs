using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstablePuffball : Skill
{
    //Skill specific fields
    [SerializeField] private GameObject puffballPrefab;
    public override void DoSkill()
    {
        //Skill specific stuff
        DoPuffball();
        EndSkill();
    }

    public void DoPuffball()
    {
        GameObject player = GameObject.FindWithTag("currentPlayer");
        Instantiate(puffballPrefab, new Vector3(player.transform.position.x, player.transform.position.y + 0.8f, player.transform.position.z), transform.rotation);
    }
}
