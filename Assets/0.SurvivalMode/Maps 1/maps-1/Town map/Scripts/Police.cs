using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Police : MonoBehaviour
{
    public Animator anim;
	public NavMeshAgent agent;
	public LayerMask whatIsGround, whatIsPlayer;
	public Vector3 walkPoint;
	public float walkPointRange;
    public float runSpeed;
    public float maxPointRange;
	[Space]
	public float currentHealth;
	public float maxHealth;
	[Space]
	public float sightRange, attackRange;
	public bool playerInSightRange, playerInAttackRange, canAttack, sighted;

	[Header("AudioClips")]
	public AudioClip[] footSFX;
    public AudioClip[] screamSFX;

	bool walkPointSet;
	AudioSource audio;
	public bool dead;
	Transform player;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
    	if(dead)return;

    	playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
    	playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

    	if(!playerInSightRange && !playerInAttackRange && !sighted)
    	{
    		Patroling();
    	}
        

        if(playerInSightRange && !playerInAttackRange)
        {
        	ChasePlayer();
        }
        

        if(playerInSightRange)
        {
        	canAttack = true;
        	agent.SetDestination(transform.position);

        	FindClosesteEnemy();
        	
        }
        else
        {
        	canAttack = false;
        }
        
        if(currentHealth <= 0)
        {
            Dead();
        }

        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    void Patroling()
    {
    	if(!walkPointSet)SearchWalkPoint();

    	if(walkPointSet)
    	agent.SetDestination(walkPoint);

    	Vector3 ditanceToWalkPoint = transform.position - walkPoint;

    	if(ditanceToWalkPoint.magnitude < 1f)
    	walkPointSet = false;
    }

    void ChasePlayer()
    {
    	agent.SetDestination(transform.position);
    }

    void SearchWalkPoint()
    {
    	float randomZ = Random.Range(-walkPointRange, walkPointRange);
    	float randomX = Random.Range(-walkPointRange, walkPointRange);

    	walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

    	walkPointSet = true;
    }

    void OnCollisionEnter()
    {
    	walkPointSet = false;
    }

    public void Foots()
    {
    	audio.PlayOneShot(footSFX[Random.Range(0, footSFX.Length)]);
    }

    public void TakeDamage(float amount)
    {
        if(dead)return;

    	currentHealth -= amount;

        audio.PlayOneShot(screamSFX[Random.Range(0, screamSFX.Length)]);

    	if(currentHealth <= 0)
    	{
    		Dead();
    	}
    }

    void FindClosesteEnemy()
    {
    	float distanceToClosesteEnemy = Mathf.Infinity;
    	Zombie zombie = null;

    	Zombie[] allZombies = GameObject.FindObjectsOfType<Zombie>();

    	foreach(Zombie currentZombie in allZombies)
    	{
    		float distToEnemy = (currentZombie.transform.position - this.transform.position).sqrMagnitude;
    		if(distToEnemy < distanceToClosesteEnemy)
    		{
    			distanceToClosesteEnemy = distToEnemy;
    			zombie = currentZombie;
    		}
    	}

    	var target = zombie.transform.position;
	    target.y = transform.position.y;
	    transform.LookAt(target);
    	
    }

    public void Dead()
    {
        anim.SetBool("Run", false);
        anim.SetTrigger("dead");

            agent.SetDestination(transform.position);
            dead = true;
    }
}
