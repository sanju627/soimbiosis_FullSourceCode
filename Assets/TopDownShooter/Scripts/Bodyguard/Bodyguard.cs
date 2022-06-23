using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bodyguard : MonoBehaviour
{
  NavMeshAgent agent;
	Animator anim;
	Player player;

  [Header("Stats")]
	public float radius;
  public float coverCheckRadius;
  public float meleeCheckRadius;
  public Transform meleeT;
	public bool dead;
  public float currentHealth;
  public float maxHealth;
	public float enemyCheckRadius;
  public bool canFire;
  public Cover cover;
  public float playerExitCheckRadius;
  public GameObject Gun;
  [Space]
  public Transform attackPoint;
  public float attackRadius;
  public float Damage;
  
  [Header("States")]
  public bool defend;
  public bool follow;

  [Header("UI")]
  public GameObject Canvas;

  [Header("Checking")]
	public LayerMask playerLayer;
	public LayerMask enemyLayer;
  public LayerMask coverLayer;
	public bool withinPlayer;
	public bool nearEnemy;
  public bool nearMeleeEnemy;
  public bool withinCover;
  public float dist;
  public float distCover;

	[Header("VFX")]
	public ParticleSystem bloodVFX;
  public GameObject gun;
  public GuardGun rifle;
  public GameObject melee;

	[Header("AudioClips")]
  public AudioClip[] footStepSFX;
  public AudioClip[] slashesSFX;

    AudioSource audio;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        Follow_Click();

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
    	if(dead)return;

    	withinPlayer = Physics.CheckSphere(transform.position, radius, playerLayer);
    	nearEnemy = Physics.CheckSphere(transform.position, enemyCheckRadius, enemyLayer);
      nearMeleeEnemy = Physics.CheckSphere(meleeT.position, meleeCheckRadius, enemyLayer);
      withinCover = Physics.CheckSphere(transform.position, coverCheckRadius, coverLayer);
      
      dist = Vector3.Distance(transform.position, player.transform.position);

      //Switch Weapons
      if(nearMeleeEnemy)
      {
        gun.SetActive(false);
        melee.SetActive(true);
        rifle.isReloading = false;
        anim.SetBool("Reload", false);

        anim.SetLayerWeight(anim.GetLayerIndex("Rifle"), 0);
        anim.SetLayerWeight(anim.GetLayerIndex("Melee"), 1);

      }else
      {
        melee.SetActive(false);
        gun.SetActive(true);

        anim.SetLayerWeight(anim.GetLayerIndex("Rifle"), 1);
        anim.SetLayerWeight(anim.GetLayerIndex("Melee"), 0);
      }

      if(cover != null)
      {
        distCover = Vector3.Distance(transform.position, cover.transform.position);
      }

      if(follow)
      {
          if(dist > agent.stoppingDistance && !withinPlayer)
          {
            agent.SetDestination(player.transform.position);
            anim.SetBool("walk", true);
            anim.SetBool("Cover", false);
            canFire = false;
          }else
          {
            agent.SetDestination(transform.position);
            anim.SetBool("walk", false);

            anim.SetBool("Cover", false);

            if(nearEnemy)
            {
              canFire = true;
              FindClosesteEnemy();
            }else
            {
              canFire = false;
            }
          }
      }

      if(defend)
      {
          if(withinCover)
          {
            FindClosestCover();
            agent.SetDestination(cover.transform.position);

            if(distCover <= agent.stoppingDistance)
            {
              anim.SetBool("walk", false);
              Debug.Log("In Cover");
              anim.SetBool("Cover", true);

              if(nearEnemy)
              {
                canFire = true;
                FindClosesteEnemy();
              }else
              {
                canFire = false;
              }
            }
          }else
          {
            agent.SetDestination(transform.position);
            anim.SetBool("walk", false);

            anim.SetBool("Cover", false);

            if(nearEnemy)
            {
              canFire = true;
              FindClosesteEnemy();
            }else
            {
              canFire = false;
            }
          }
      }

      if(player.GetComponent<WeaponManger>().shopOpen)
      {
        Canvas.SetActive(false);
      }else
      {
        Canvas.SetActive(true);
      }
    }

    public void Defend_Click()
    {
      defend = true;
      follow = false;
    }

    public void Follow_Click()
    {
      defend = false;
      follow = true;
    }

    void OnDrawGizmosSelected()
    {
    	Gizmos.DrawWireSphere(transform.position, radius);
      Gizmos.DrawWireSphere(meleeT.position, meleeCheckRadius);
    	Gizmos.DrawWireSphere(transform.position, enemyCheckRadius);
    }

    public void Foots()
    {
        audio.PlayOneShot(footStepSFX[Random.Range(0, footStepSFX.Length)]);
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

    void FindClosestCover()
    {
      float distanceToClosesteCover = Mathf.Infinity;
      cover = null;

      Cover[] allCovers = GameObject.FindObjectsOfType<Cover>();

      foreach(Cover currentCover in allCovers)
      {
        float distToCover = (currentCover.transform.position - this.transform.position).sqrMagnitude;
        if(distToCover < distanceToClosesteCover)
        {
          distanceToClosesteCover = distToCover;
          cover = currentCover;
        }
      }
    }

    public void TakeDamage(float amount)
    {
    	if(dead)return;

    	bloodVFX.Play();

    	currentHealth -= amount;

    	if(currentHealth <= 0)
    	{
    		anim.SetTrigger("Dead");
        Canvas.SetActive(false);
        anim.SetLayerWeight(anim.GetLayerIndex("Rifle"), 0);
        anim.SetLayerWeight(anim.GetLayerIndex("Melee"), 0);

    		GetComponent<CapsuleCollider>().enabled = false;
    		GetComponent<NavMeshAgent>().enabled = false;
        gameObject.layer = 0;
        dead = true;
    	}
    }

    public void Attack()
    {
        Collider[] enemyCol = Physics.OverlapSphere(attackPoint.position, attackRadius, enemyLayer);

        audio.PlayOneShot(slashesSFX[Random.Range(0, slashesSFX.Length)]);

        foreach(Collider z in enemyCol)
        {
            z.GetComponent<Zombie_BP>().TakeDamage(Damage);
        }

    }
}
