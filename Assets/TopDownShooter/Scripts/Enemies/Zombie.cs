using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Vehicles.Car;

public class Zombie : MonoBehaviour
{
    private string V = "Default";
    Animator anim;
    public ZombiesTarget target;
    public bool dead;
    public bool isBoss;
	public float currentHealth;
	public float MaxHealth;
    public GameObject checkObj;
    [Space]
    public float distance;
    public float distGaurd;

    [Header("AudioClips")]
	public AudioClip[] footStepSFX;
	public AudioClip[] BodyFallSFX;
	public AudioClip[] hurtSFX;

    [Header("Attack")]
    public Transform attackPoint;
    public float radius;
    public float checkRadius;
    public LayerMask playerLayer;
    public LayerMask guardLayer;
    public LayerMask vehLayer;
    public LayerMask helLayer;
    public LayerMask TargetLayer;
    public LayerMask npcLayer;
    public LayerMask banditLayer;
    public LayerMask objLayer;
    public float damage;
    public float flameDamage;
    public bool burning;
    public float burnDuration;
    [Space]
    public bool withinTarget;
    [Space]
    public Transform g;

    [Header("WalkPoints")]
    public Vector3 walkPoint;
    public Vector3 ditanceToWalkPoint;
    public float distanceToWalkMagnitude;
    public bool walkPointSet;

    [Header("VFX")]
    public ParticleSystem flameVFX;
    public ParticleSystem bloodVFX;
    public GameObject[] Bodies;
    public GameObject[] Heads;
    public GameObject[] Legs;

    Rigidbody[] ragdollBodies;
    Collider[] colliders;
    NavMeshAgent agent;
	Player player;
	AudioSource audio;
    ExploreManager exp_Manager;

    // Start is called before the first frame update
    void Start()
    {
        if(isBoss)return;
        anim = GetComponent<Animator>();
        currentHealth = MaxHealth;
        audio = GetComponent<AudioSource>();
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        StartCoroutine(GetPlayer());
        exp_Manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ExploreManager>();

        agent = GetComponent<NavMeshAgent>();

        ToggleRagdoll(false);

        foreach(Collider col in colliders)
    	{
    		col.isTrigger = true;
    	}

        Customize();
    }

    IEnumerator GetPlayer()
    {
        yield return new WaitForSeconds(0.5f);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(isBoss)return;

        if(burning)
        {
            currentHealth -= Time.time * flameDamage;
        }

        if(currentHealth <= 0)
        {
            Dead();
        }

        withinTarget = Physics.CheckSphere(transform.position, checkRadius, TargetLayer);

        if (withinTarget)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            distance = Vector3.Distance(transform.position, player.transform.position);
        }

        if(withinTarget)
        {
            //Find Closest Target
            FindClosesteEnemy();
        }else
        {
            //Debug.Log("Patrolling");
            Patroling();
        }
        
    }

    void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        agent.SetDestination(walkPoint);

        ditanceToWalkPoint = transform.position - walkPoint;
        distanceToWalkMagnitude = ditanceToWalkPoint.magnitude;

        if (ditanceToWalkPoint.magnitude < 5f)
            walkPointSet = false;
    }

    void SearchWalkPoint()
    {
        Transform tSpawn = exp_Manager.wanderPositions[Random.Range(0, exp_Manager.wanderPositions.Length)];
        walkPoint = new Vector3(tSpawn.position.x, transform.position.y, tSpawn.position.z);

        // if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        walkPointSet = true;
    }

    void FindClosesteEnemy()
    {
        float distanceToClosesteTarget = Mathf.Infinity;
        target = null;

        ZombiesTarget[] allTarget = GameObject.FindObjectsOfType<ZombiesTarget>();

        foreach(ZombiesTarget currentTarget in allTarget)
        {
            float distToTarget = (currentTarget.transform.position - this.transform.position).sqrMagnitude;
            if(distToTarget < distanceToClosesteTarget)
            {
                distanceToClosesteTarget = distToTarget;
                target = currentTarget;

                if(distance >= 2f && !target.isAlive)
                {
                    target = player.GetComponent<ZombiesTarget>();
                }else
                {
                    target = currentTarget;
                }
            }
        }

        var targetT = target.transform.position;
        targetT.y = transform.position.y;
        transform.LookAt(targetT);
    }

    public void TakeDamage(float amount)
    {
    	if(dead)return;

    	currentHealth -= amount;
        Hurt();

        if(amount >= 10)
    	anim.SetTrigger("Hit");

        bloodVFX.Play();

    	if(currentHealth < 0)
    	{
    		anim.SetTrigger("Dead");
    		//GetComponent<CapsuleCollider>().enabled = false;
    		//ToggleRagdoll(false);
    		GetComponent<NavMeshAgent>().enabled = false;
            checkObj.layer = 0;

            foreach(Collider col in colliders)
	    	{
	    		col.isTrigger = false;
	    	}

            ToggleRagdoll(true);
            //dead = true;
            Destroy(this);
    	}
    }

    public void Burn(float amount)
    {
        if(dead)return;

        flameDamage = amount;
        StartCoroutine(Burn());

        if(currentHealth < 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        if(dead)return;
        
        anim.SetTrigger("Dead");
        ToggleRagdoll(true);
        //GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        Hurt();

        anim.SetLayerWeight(anim.GetLayerIndex("Rifle"), 0);

        checkObj.layer = 0;
        gameObject.tag = V;

        foreach(Collider col in colliders)
    	{
    		col.isTrigger = false;
    	}

        foreach(Rigidbody rb in ragdollBodies)
        {
        	rb.AddExplosionForce(107f, new Vector3(-1f, 0.5f, -1f), 5f, 0f, ForceMode.Impulse);
        }

        ToggleRagdoll(true);

        

        dead = true;
        Destroy(this);
    }

    void ToggleRagdoll(bool state)
    {
    	anim.enabled = !state;

    	foreach(Rigidbody rb in ragdollBodies)
    	{
    		rb.isKinematic = !state;
    	}
    }

    IEnumerator Burn()
    {
        burning = true;   

        flameVFX.Play();     

        yield return new WaitForSeconds(burnDuration);

        flameVFX.Stop();

        burning = false;
    }

    public void Foots()
    {
        audio.PlayOneShot(footStepSFX[Random.Range(0, footStepSFX.Length)]);
    }

    public void Hurt()
    {
        audio.PlayOneShot(hurtSFX[Random.Range(0, hurtSFX.Length)]);
    }

    public void Attack()
    {
        Collider[] playerCol = Physics.OverlapSphere(attackPoint.position, radius, playerLayer);
        Collider[] guardCol = Physics.OverlapSphere(attackPoint.position, radius, guardLayer);
        Collider[] vehCol = Physics.OverlapSphere(attackPoint.position, radius, vehLayer);
        Collider[] helCol = Physics.OverlapSphere(attackPoint.position, radius, helLayer);
        Collider[] npcCol = Physics.OverlapSphere(attackPoint.position, radius, npcLayer);
        Collider[] banditCol = Physics.OverlapSphere(attackPoint.position, radius, banditLayer);
        Collider[] objCol = Physics.OverlapSphere(attackPoint.position, radius, objLayer);

        foreach (Collider player in playerCol)
        {
            player.GetComponent<Player>().TakeDamage(damage);
        }

        foreach(Collider veh in vehCol)
        {
            veh.GetComponent<CarController>().TakeDamage(damage);
        }

        foreach(Collider hel in helCol)
        {
            hel.GetComponent<HelicopterController>().TakeDamage(damage);
        }

        foreach(Collider t in npcCol)
        {
            t.GetComponent<NPC>().TakeDamage(damage);
        }

        foreach (Collider b in banditCol)
        {
            b.GetComponent<Bandit_BP>().TakeDamage(damage);
        }
        
        foreach (Collider g in guardCol)
        {
            g.GetComponent<Guard>().TakeDamage(damage);
        }
        
        foreach (Collider o in objCol)
        {
            o.GetComponent<OBJ>().TakeDamage(damage);
        }
    }

    void Customize()
    {
        foreach(GameObject G in Bodies)
        {
            G.SetActive(false);
        }

        foreach(GameObject H in Heads)
        {
            H.SetActive(false);
        }

        foreach(GameObject L in Legs)
        {
            L.SetActive(false);
        }

        int B_R = Random.Range(0, Bodies.Length);
        int H_R = Random.Range(0, Heads.Length);
        int L_R = Random.Range(0, Legs.Length);

        Bodies[B_R].SetActive(true);
        Heads[H_R].SetActive(true);
        Legs[L_R].SetActive(true);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
