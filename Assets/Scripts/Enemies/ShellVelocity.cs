using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellVelocity : MonoBehaviour
{
    private Rigidbody rb;
    private Transform player;
    private float launchAngle = 45f;
    private Vector3 gravity = new Vector3(0, -20f, 0);
    [SerializeField] private float damage = 50f;
    [SerializeField] private GameObject particles;
    // Start is called before the first frame update
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("currentPlayer").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        rb.AddForce(gravity, ForceMode.Acceleration);

        transform.Rotate(360f * Time.deltaTime, 0f, 0f);
    }
    public void LaunchShell()
    {
        Vector3 toPlayer = player.position - rb.transform.position;
        float disToPlayer = Vector2.Distance(new Vector2(rb.transform.position.x, rb.transform.position.z), new Vector2(player.position.x, player.position.z));
        toPlayer.y = 0;

        float launchAngleRad = launchAngle * Mathf.Deg2Rad;

        float velocityMagnitude = Mathf.Sqrt(disToPlayer * Mathf.Abs(gravity.y) / Mathf.Sin(2 * launchAngleRad));

        Vector3 velocity = velocityMagnitude * toPlayer.normalized;
        velocity.y = velocityMagnitude * Mathf.Sin(launchAngleRad);

        rb.AddForce(velocity, ForceMode.VelocityChange);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "currentPlayer" && collision.GetComponentInParent<PlayerController>().isInvincible == false)
        {
            collision.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(damage * GlobalData.currentLoop);
            ParticleManager.Instance.SpawnParticles("PlosionParticle", transform.position, Quaternion.Euler(0, 0, 0));
            if (this.gameObject.tag == "Enemy")
            {
                return;
            }
            DestroyShell();
        }
        else if (collision.gameObject.layer == 8 || collision.gameObject.layer == 12)
        {
            ParticleManager.Instance.SpawnParticles("PlosionParticle", transform.position, Quaternion.Euler(0, 0, 0));
            SoundEffectManager.Instance.PlaySound("Explosion", transform);
            if (this.gameObject.tag == "Enemy")
            {
                return;
            }
            DestroyShell();
        }
        
    }

    private void DestroyShell(){
        GetComponent<Collider>().enabled = false;
        //GetComponent<Rigidbody>().useGravity = false;
        Vector3 newVector = GetComponent<Rigidbody>().velocity;
        GetComponent<Rigidbody>().velocity = Vector3.Normalize(newVector) * 0.5f;
        ShrinkToSize shrink = GetComponent<ShrinkToSize>();
        if(shrink != null){
            shrink.enabled = true;
        }
        Destroy(gameObject, 2);
    }
}
