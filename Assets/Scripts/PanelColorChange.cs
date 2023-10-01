using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelColorChange : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag =="Player")
        {
            anim.SetInteger("State", 1);
        }
    }
}
