using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spineshot : Skill
{
    //Skill specific fields
    [SerializeField] private GameObject spineshotPrefab;
    public override void DoSkill()
    {
        //Skill specific stuff
        DoSpineshot();
        EndSkill();
    }

    public void DoSpineshot()
    {
        GameObject player = GameObject.FindWithTag("currentPlayer");
        Instantiate(spineshotPrefab, new Vector3(player.transform.position.x, player.transform.position.y + 0.7f, player.transform.position.z), transform.rotation);
    }
}
