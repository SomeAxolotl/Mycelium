using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyKnockback : MonoBehaviour
{
    public bool damaged = false;
    private bool onGround = true;
    Rigidbody rb;
    Transform player;
    Animator animator;
    public LayerMask groundLayer;
    ReworkedEnemyNavigation reworkedEnemyNav;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("currentPlayer").transform;
        animator = GetComponent<Animator>();
        reworkedEnemyNav = GetComponent<ReworkedEnemyNavigation>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z), transform.up * .35f, Color.green);

        RaycastHit test;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z), transform.up, out test, .35f, groundLayer))
        {
            Debug.Log("hitting!: " + gameObject.name);
            rb.AddForce(Vector3.up * 2f, ForceMode.Impulse);
        }
        else
        {
            Debug.Log("not hitting");
        }
    }
    public void Knockback(float knockbackForce)
    {
        animator.SetBool("IsMoving", false);
        damaged = true;
        reworkedEnemyNav.enabled = false;
        Vector3 dirFromPlayer = (new Vector3(transform.position.x, 0f, transform.position.z) - new Vector3(player.position.x, 0f, player.position.z)).normalized;
        StartCoroutine(StartKnockback(dirFromPlayer, knockbackForce));
    }
    IEnumerator StartKnockback(Vector3 direction, float force)
    {
        Vector3 knockbackForce = direction * force;
        knockbackForce += Vector3.up * 3f;
        rb.AddForce(knockbackForce, ForceMode.Impulse);
        onGround = false;
        yield return new WaitForFixedUpdate();
        yield return new WaitUntil(() => onGround);
        animator.SetBool("IsMoving", true);
        damaged = false;
        reworkedEnemyNav.enabled = true;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0 && collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            ContactPoint contact = collision.GetContact(0);
            if(contact.point.y <= transform.position.y + .25f)
            {
                onGround = true;
            }
        }
    }
}
