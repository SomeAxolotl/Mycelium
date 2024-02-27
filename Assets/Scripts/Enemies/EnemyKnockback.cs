using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    EnemyAttack enemyAttack;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("currentPlayer").transform;
        animator = GetComponent<Animator>();
        reworkedEnemyNav = GetComponent<ReworkedEnemyNavigation>();
        enemyAttack = GetComponent<EnemyAttack>();
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
        enemyAttack.CancelAttack();
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
        float timer = 0f;
        if (this.gameObject.name == "Giga Beetle")
        {
            onGround = true;
        }
        while(timer < 1f && !onGround)
        {
            timer += Time.fixedDeltaTime;
                if(timer > 1f)
                {
                    onGround = true;
                }
            yield return new WaitForFixedUpdate();
        }
        timer = 0f;
        animator.SetBool("IsMoving", true);
        enemyAttack.CancelAttack();
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
