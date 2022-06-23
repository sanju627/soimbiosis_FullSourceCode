using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class Grenade : MonoBehaviour
{
	public bool smoke;
    public bool grenade;
    public bool landMine;
    public bool Molotov;
    public GameObject ExplosionVFX;
    public GameObject landMineVFX;
    public GameObject FireField;
    public GameObject Trail;
	public float delay;
    public float force;
    public float damage;
    public float vehicleDamage;
    public float radius;
    public float landMineRadius;
    public LayerMask enemyLayer;

	[Header("AudioClips")]
    public AudioClip explosionSFX;
	AudioSource audio;
    public bool destroyed = false;

    [Header("GFX")]
    public MeshRenderer[] mesh;
    public MeshCollider[] meshC;
    public GameObject[] trail;
    public SphereCollider sphereC;

    // Start is called before the first frame update
    void Start()
    {
    	audio = GetComponent<AudioSource>();

    	if(smoke)
        StartCoroutine(SmokeBlast());

        if(grenade)
        StartCoroutine(Explosion());
    }

    // Update is called once per frame
    void Update()
    {
        if(landMine)
        {
            bool inRadius = Physics.CheckSphere(transform.position, radius, enemyLayer);

            if(inRadius)
            {
                if(destroyed)return;

                audio.PlayOneShot(explosionSFX);

                Instantiate(landMineVFX, transform.position, transform.rotation);

                for (int i = 0; i < mesh.Length; i++)
                {
                    mesh[i].enabled = false;
                }

                for (int i = 0; i < meshC.Length; i++)
                {
                    meshC[i].enabled = false;
                }

                sphereC.enabled = false;

                Collider[] col = Physics.OverlapSphere(transform.position, landMineRadius, enemyLayer);

                foreach(Collider enemies in col)
                {
                    enemies.GetComponent<Zombie_BP>().TakeDamage(damage);
                }


                destroyed = true;

                Destroy(gameObject, 0.5f);
            }
            
        }
    }

    IEnumerator SmokeBlast()
    {
    	yield return new WaitForSeconds(delay);

    	Instantiate(ExplosionVFX, transform.position, transform.rotation);

    	audio.PlayOneShot(explosionSFX);

        for (int i = 0; i < mesh.Length; i++)
        {
            mesh[i].enabled = false;
        }


        for (int i = 0; i < meshC.Length; i++)
        {
            meshC[i].enabled = false;
        }


    	Destroy(gameObject, 5f);
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(delay);

        Instantiate(ExplosionVFX, transform.position, transform.rotation);

        audio.PlayOneShot(explosionSFX);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

            if(rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }

            Zombie_BP target = nearbyObject.GetComponentInChildren<Zombie_BP>();
            if(target != null)
            {
                target.TakeDamage(damage);
            }

            CarController veh = nearbyObject.GetComponent<CarController>();
            if(veh != null)
            {
                veh.TakeDamage(damage);
            }

            BodyParts targetBoss = nearbyObject.GetComponentInChildren<BodyParts>();
            if(targetBoss != null)
            {
                targetBoss.TakeExplosionDamage();
            }

            HelicopterController hel = nearbyObject.GetComponent<HelicopterController>();
            if(hel != null)
            {
                hel.TakeDamage(damage);
            }

        }

        for (int i = 0; i < mesh.Length; i++)
        {
            mesh[i].enabled = false;
        }

        for (int i = 0; i < meshC.Length; i++)
        {
            meshC[i].enabled = false;
        }

        for (int i = 0; i < meshC.Length; i++)
        {
            trail[i].SetActive(false);
        }

        Destroy(gameObject, 2f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void OnCollisionEnter()
    {
        if(Molotov)
        {
            Instantiate(FireField, transform.position, Quaternion.identity);

            audio.PlayOneShot(explosionSFX);

            Destroy(Trail);

            for (int i = 0; i < mesh.Length; i++)
            {
                mesh[i].enabled = false;
            }

            for (int i = 0; i < meshC.Length; i++)
            {
                meshC[i].enabled = false;
            }

            for (int i = 0; i < meshC.Length; i++)
            {
                trail[i].SetActive(false);
            }

            Destroy(gameObject, 2f);
        }
    }
}
