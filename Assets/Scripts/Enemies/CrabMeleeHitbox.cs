using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabMeleeHitbox : MonoBehaviour
{
    [HideInInspector] public float damage;
    [HideInInspector] public float knockbackForce;
    [HideInInspector] public List<GameObject> playerHit = new List<GameObject>();
    [SerializeField] private GameObject particles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator ActivateHitbox()
    {
        yield return new WaitForSeconds(0.35f);
        gameObject.GetComponent<Collider>().enabled = true;
        //gameObject.GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        Instantiate(particles, transform.position + new Vector3(0f, -0.5f, 0f), Quaternion.identity);
        yield return new WaitForSeconds(0.15f);
        gameObject.GetComponent<Collider>().enabled = false;
        //gameObject.GetComponent<Renderer>().enabled = false;
        playerHit.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "currentPlayer" && !other.gameObject.GetComponentInParent<PlayerController>().isInvincible && !playerHit.Contains(other.gameObject))
        {
            other.gameObject.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(damage);
            other.gameObject.GetComponentInParent<PlayerController>().Knockback(this.gameObject, knockbackForce);
            playerHit.Add(other.gameObject);
        }
    }
}
