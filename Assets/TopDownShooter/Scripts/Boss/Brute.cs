using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Vehicles.Car;

public class Brute : MonoBehaviour
{
	public bool inRaged;
	public float currentHealth;
	public float maxHealth;
	public float ragedHealth;
    public GameObject checkObj;
	public bool dead;

    [Header("Check")]
    public bool withinTarget;
    public float checkRadius;
    public LayerMask TargetLayer;

    [Header("Attack")]
    public Transform attackPoint;
    public float radius;
    public float ragedRadius;
    public LayerMask playerLayer;
    public LayerMask vehLayer;
    public LayerMask helLayer;
    public LayerMask guardLayer;
    public LayerMask npcLayer;
    public float damage;
    public float ragedDamage;
    public float force;
    public float explosionDamage;

    [Header("VFX")]
    public ParticleSystem dustExplosion;

	[Header("Sound Effects")]
	public AudioClip roarSFX;
	public AudioClip[] footsSFX;
	public AudioClip[] explosionSFX;
	public AudioClip[] BodyFallSFX;

	AudioSource audio;
	Animator anim;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        audio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        withinTarget = Physics.CheckSphere(transform.position, checkRadius, TargetLayer);

        if (withinTarget)
        {
            anim.SetBool("walk", true);
        }
    }

    public void ExplosionDamage()
    {
        if(inRaged)return;
        if(dead)return;

        currentHealth -= explosionDamage;
        //anim.SetTrigger("Hit");

        if(currentHealth <= ragedHealth)
        {
            anim.SetBool("inRage", true);
        }

        if(currentHealth < 0)
        {
            anim.SetTrigger("Dead");
            checkObj.layer = 0;
            //GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            dead = true;
            this.enabled = false;
        }
    }

    public void TakeDamage(float amount)
    {
        if(inRaged)return;
		if(dead)return;

    	currentHealth -= amount;
    	//anim.SetTrigger("Hit");

    	if(currentHealth <= ragedHealth)
    	{
    		anim.SetBool("inRage", true);
    	}

    	if(currentHealth < 0)
    	{
    		anim.SetTrigger("Dead");
            checkObj.layer = 0;
    		//GetComponent<CapsuleCollider>().enabled = false;
    		GetComponent<NavMeshAgent>().enabled = false;
    		dead = true;
            this.enabled = false;
    	}
    }

    public void Shout()
    {
    	audio.PlayOneShot(roarSFX);
    }

    public void Foots()
    {
    	audio.PlayOneShot(footsSFX[Random.Range(0, footsSFX.Length)]);
    }

    public void Attack()
    {
        Collider[] playerCol = Physics.OverlapSphere(attackPoint.position, radius, playerLayer);
        Collider[] guardCol = Physics.OverlapSphere(attackPoint.position, radius, guardLayer);
        Collider[] vehCol = Physics.OverlapSphere(attackPoint.position, radius, vehLayer);
        Collider[] helCol = Physics.OverlapSphere(attackPoint.position, radius, helLayer);
        Collider[] npcCol = Physics.OverlapSphere(attackPoint.position, radius, npcLayer);

        foreach (Collider player in playerCol)
        {
            player.GetComponent<Player>().TakeDamage(damage);
        }

        foreach(Collider g in guardCol)
        {
            g.GetComponent<Bodyguard>().TakeDamage(damage);
        }

        foreach(Collider veh in vehCol)
        {
            veh.GetComponent<CarController>().TakeDamage(damage);
        }

        foreach(Collider hel in helCol)
        {
            hel.GetComponent<HelicopterController>().TakeDamage(damage);
        }

        foreach (Collider t in npcCol)
        {
            t.GetComponent<NPC>().TakeDamage(damage);
        }
    }

    public void RagedAttack()
    {
        dustExplosion.Play();

        Collider[] colliders = Physics.OverlapSphere(transform.position, ragedRadius);

        audio.PlayOneShot(explosionSFX[Random.Range(0, explosionSFX.Length)]);

        foreach(Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

            if(rb != null)
            {
                rb.AddExplosionForce(force, transform.position, ragedRadius);
            }

            Player player = nearbyObject.GetComponent<Player>();
            if(player != null)
            {
                player.TakeDamage(damage);
            }

            CarController c = nearbyObject.GetComponent<CarController>();
            if(c != null)
            {
                c.TakeDamage(damage);
            }

            HelicopterController h = nearbyObject.GetComponent<HelicopterController>();
            if(h != null)
            {
                h.TakeDamage(damage);
            }
        }
    }

    public void BodyFall()
    {
        audio.PlayOneShot(BodyFallSFX[Random.Range(0, BodyFallSFX.Length)]);
    }

    void OnDrawGizmosSelected()
    {
    	Gizmos.DrawWireSphere(transform.position, ragedRadius);
    }
}
