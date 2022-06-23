using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    Animator anim;
    NavMeshAgent agent;
    Zombie zombie;
    Transform tSpawn;
    HomeBase _homeBase;
    Rigidbody[] ragdollBodies;
    Collider[] colliders;
    float nextAttackTime = 0f;
    AudioSource audio;
    Player player;
    float distance;
    bool setPos;
    popTXT poptext;
    ExploreManager ep_Manager;
    HomeBase home;

    public bool withinEnemy;
    public bool canFire;
    public bool dead;
    [Space]
    public GameObject GunOBJ;
    [Space]
    public float withinMeleeRadius;
    public float weaponCheckRadius;
    public float fleeDist;
    public int rand;
    [Space]
    public LayerMask TargetLayer;

    [Header("Stats")]
    public float currentHealth;
    public float maxHealth;

    [Header("SFX")]
    public AudioClip[] hurtSFX;

    [Header("VFX")]
    public ParticleSystem bloodVFX;
    public GameObject[] weapons;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        currentHealth = maxHealth;

        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        ToggleRagdoll(false);

        rand = Random.Range(0, weapons.Length);
        SelectWeapon(rand);
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) return;

        withinEnemy = Physics.CheckSphere(transform.position, weaponCheckRadius, TargetLayer);

        if (currentHealth <= 0)
        {
            Dead();
        }


        if (withinEnemy)
        {
            FindClosesteEnemy();

            anim.SetFloat("speed", 0f);

            agent.SetDestination(transform.position);

            distance = Vector3.Distance(transform.position, zombie.transform.position);


            canFire = true;
        }else
        {
            canFire = false;
        }
    }

    void FindClosesteEnemy()
    {
        float distanceToClosesteEnemy = Mathf.Infinity;
        zombie = null;

        Zombie[] allZombies = GameObject.FindObjectsOfType<Zombie>();

        foreach (Zombie currentZombie in allZombies)
        {
            float distToEnemy = (currentZombie.transform.position - this.transform.position).sqrMagnitude;
            if (distToEnemy < distanceToClosesteEnemy)
            {
                distanceToClosesteEnemy = distToEnemy;
                zombie = currentZombie;
            }
        }

        var targetT = zombie.transform.position;
        targetT.y = transform.position.y;
        transform.LookAt(targetT);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, weaponCheckRadius);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;


        audio.PlayOneShot(hurtSFX[Random.Range(0, hurtSFX.Length)]);

        bloodVFX.Play();

        anim.SetTrigger("hit");

        if (currentHealth <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        Collider[] col = GetComponentsInChildren<Collider>();

        ToggleRagdoll(true);
        agent.enabled = false;
        Destroy(this.GetComponent<ZombiesTarget>());
        Destroy(this.GetComponent<NPC_Target>());

        foreach (Collider c in col)
        {
            c.isTrigger = false;
        }

        gameObject.layer = 0;

        dead = true;

    }

    void ToggleRagdoll(bool state)
    {
        anim.enabled = !state;

        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = !state;
        }
    }

    void SelectWeapon(int choice)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }

        weapons[choice].SetActive(true);
    }
}
