using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyKnockback : MonoBehaviour
{
    public bool damaged = false;
    [SerializeField] private bool onGround = true;
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
        RaycastHit test;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z), transform.up, out test, .35f, groundLayer))
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
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
        if(this.gameObject.name == "Giga Beetle")
        {
            onGround = true;
        }
        else
        {
            yield return new WaitUntil(() => onGround);
        }
        animator.SetBool("IsMoving", true);
        damaged = false;
        reworkedEnemyNav.enabled = true;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0 && collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.contacts.Length > 0 && collision.gameObject.layer == LayerMask.NameToLayer("Enviroment"))
        {
            ContactPoint contact = collision.GetContact(0);
            if(contact.point.y <= transform.position.y + .5f)
            {
                onGround = true;
            }
        }
    }
}
