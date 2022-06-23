using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    Animator anim;
	NavMeshAgent agent;
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

    public PlayfabManager database;
    [Header("Stats")]
    public Transform[] movePoints;
    public GameObject[] Weapons;
    public NPC_Gun[] guns;
	public Zombie zombie;
	public bool walkPointSet;
    public Vector3 walkPoint;
    public Vector3 ditanceToWalkPoint;
    public float distanceToWalkMagnitude;
    public float checkRadius;
    public float weaponCheckRadius;
    public float radius;
    public float playerCheckRadius;
    public float withinMeleeRadius;
    public float fleeDist = 1f;
    [Space]
    public float attackRate;
    public float damage;
    public LayerMask TargetLayer;
    public LayerMask playerLayer;
    public Transform attackPoint;
    [Space]
    public float currentHealth;
    public float maxHealth;
    public float healHealth;
    public float healTime;
    public bool isHealing;
    [Space]
    public GameObject GunOBJ;
    public GameObject MeleeOBJ;

    [Header("Stats")]
    public bool stateSet;
    [Space]
    public bool homeBase;
    public bool civil;
    public bool patrol;
    public bool follow;
    public bool melee;
    public bool gun;
    public bool withinEnemy;
    public bool withinPlayer;
    public bool withinMelee;
    public bool canFire;
    public bool dead;
    public int weaponNum;
    

    [Header("SFX")]
    public AudioClip[] hurtSFX;
    public AudioClip[] swingSFX;
    public AudioClip[] footsSFX;

    [Header("UI")]
    public Slider healthSlider;
    public Slider energySlider;
    public GameObject Canvas;
    public GameObject statsBTN;
    public GameObject statsInfo;
    public TextMeshProUGUI statTXT;
    public GameObject InteractBTN;

    [Header("VFX")]
    public ParticleSystem bloodVFX;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        if (homeBase)
            _homeBase = GameObject.FindGameObjectWithTag("HomeBase").GetComponent<HomeBase>();

        database = GameObject.FindGameObjectWithTag("Database").GetComponent<PlayfabManager>();

        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        audio = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        poptext = GameObject.FindGameObjectWithTag("MSG").GetComponent<popTXT>();

        statTXT.text = "Equip";

        ToggleRagdoll(false);

        if (gun)
            Switch(weaponNum);

        currentHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        if (civil)
        {

            statsInfo.SetActive(false);

            statsBTN.SetActive(true);

            if (!homeBase)
            {
                ep_Manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ExploreManager>();
                movePoints = ep_Manager.civilPositions;
            }
            else
            {
                home = GameObject.FindGameObjectWithTag("HomeBase").GetComponent<HomeBase>();
                movePoints = home.campFirePos;

                statsInfo.SetActive(false);
                statsBTN.SetActive(false);
            }
        }
        else
        {
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Game"))
            {
                ep_Manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ExploreManager>();

                patrol = false;
                follow = true;

                ClickPick();

                movePoints = ep_Manager.civilPositions;
            }

            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Game"))
            {
                home = GameObject.FindGameObjectWithTag("HomeBase").GetComponent<HomeBase>();

                movePoints = home.campFirePos;
            }
        }

        Collider[] col = GetComponentsInChildren<Collider>();
        foreach (Collider c in col)
        {
            c.isTrigger = true;
        }

        if (!civil)
        SendStats();

        poptext = GameObject.FindGameObjectWithTag("MSG").GetComponent<popTXT>();
        //poptext.PopText("Not Enough Slots");
    }

    // Update is called once per frame
    void Update()
    {
        if(dead)return;

        if(!withinEnemy)
        anim.SetFloat("speed", agent.velocity.sqrMagnitude);

        
        withinEnemy = Physics.CheckSphere(transform.position, weaponCheckRadius, TargetLayer);

        withinPlayer = Physics.CheckSphere(transform.position, playerCheckRadius, playerLayer);

        withinMelee = Physics.CheckSphere(transform.position, withinMeleeRadius, TargetLayer);

        if (!follow)
        {
            StartCoroutine(EnableSkin(0));
        }

        //--------------------------------------------------Patrolling---------------------------------------//
        if (patrol && !withinEnemy && !homeBase && !civil)
        	Patroling();

        if(follow)
        {
            if (player.inCar)
            {
                transform.position = player.carController.seat.position;
                agent.enabled = false;
                DisableSkin();
                canFire = false;
            }
            else
            {

                agent.enabled = true;
                agent.SetDestination(player.transform.position);

                StartCoroutine(EnableSkin(0.1f));

                //Debug.Log("Chasing");

                canFire = true;
            }
        }

        //--------------------------------------------------Follow Player---------------------------------------//
        if(follow && !withinEnemy)
        {
            if(!withinPlayer)
            {
                if (player.inCar)
                {
                    transform.position = player.carController.seat.position;
                    agent.enabled = false;
                    canFire = false;
                }
                else
                {

                    agent.enabled = true;
                    agent.SetDestination(player.transform.position);

                    StartCoroutine(EnableSkin(0.1f));

                    //Debug.Log("Chasing");

                    canFire = true;
                }
            }
        }else if(!follow)
        {
            Patroling();
            //StartCoroutine(EnableSkin(0));
        }

        //-------------------------------Civilians-------------------------------//

        
        if(civil && !homeBase && !player.inCar)
        {
            if (withinPlayer)
            {
                if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("CityBase"))
                {
                    Canvas.SetActive(true);
                }
                
            }else if(!withinPlayer && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("CityBase"))
            {
                Canvas.SetActive(false);
            }
        }else if(player.inCar)
        {
            Canvas.SetActive(false);
        }

        if(homeBase)
        {
            Canvas.SetActive(false);
        }

        if(homeBase && !withinEnemy && civil && !setPos)
        {
            Transform tSpawn = _homeBase.campFirePos[Random.Range(0, _homeBase.campFirePos.Length)];
            float dist = Vector3.Distance(tSpawn.position, transform.position);

            agent.SetDestination(tSpawn.position);

            setPos = true;
        }

        if (withinEnemy && !isHealing)
        {
            FindClosesteEnemy();

            anim.SetFloat("speed", 0f);

            agent.SetDestination(transform.position);

            distance = Vector3.Distance(transform.position, zombie.transform.position);

            
            if (withinMelee)
            {
                GunOBJ.SetActive(false);
                MeleeOBJ.SetActive(true);

                canFire = false;

                if (melee && Time.time >= nextAttackTime)
                {
                    int rand = Random.Range(0, 2);

                    if (rand == 0)
                    {
                        anim.SetTrigger("attack1");
                    }
                    else
                    {
                        anim.SetTrigger("attack2");
                    }

                    nextAttackTime = Time.time + 1f / attackRate;
                }

                if (distance < fleeDist)
                {
                    Vector3 DistToEnemy = transform.position - zombie.transform.position;

                    Vector3 newPos = transform.position + DistToEnemy;

                    anim.SetFloat("speed", -1f);

                    agent.SetDestination(newPos);
                }
            }else
            {
                GunOBJ.SetActive(true);
                MeleeOBJ.SetActive(false);

                if(!player.inCar)
                canFire = true;
            }

            setPos = false;
        }
        else
        {
            GunOBJ.SetActive(true);
            MeleeOBJ.SetActive(false);

            canFire = false;
        }

        //------------------------------------------Healing---------------------------------//
        if(currentHealth <= healHealth && !isHealing)
        {
            StartCoroutine(Heal());
        }

        healthSlider.value = currentHealth;
    }

    public void DisableSkin()
    {
        SkinnedMeshRenderer[] skin = GetComponentsInChildren<SkinnedMeshRenderer>();
        MeshRenderer[] mesh = GetComponentsInChildren<MeshRenderer>();
        

        foreach (SkinnedMeshRenderer c in skin)
        {
            c.enabled = false;
        }

        foreach (MeshRenderer m in mesh)
        {
            m.enabled = false;
        }
    }

    public IEnumerator EnableSkin(float delay)
    {
        yield return new WaitForSeconds(delay);

        SkinnedMeshRenderer[] skin = GetComponentsInChildren<SkinnedMeshRenderer>();
        MeshRenderer[] mesh = GetComponentsInChildren<MeshRenderer>();
        Collider[] col = GetComponentsInChildren<Collider>();

        foreach (SkinnedMeshRenderer c in skin)
        {
            c.enabled = true;
        }

        foreach (MeshRenderer m in mesh)
        {
            m.enabled = true;
        }
    }

    void Patroling()
    {
        if(!walkPointSet)SearchWalkPoint();

        agent.SetDestination(walkPoint);

        ditanceToWalkPoint = transform.position - walkPoint;
        distanceToWalkMagnitude = ditanceToWalkPoint.magnitude;

        if(ditanceToWalkPoint.magnitude < 12f)
        walkPointSet = false;
    }


    void SearchWalkPoint()
    {	
    	if(!withinEnemy)
    		tSpawn = movePoints[Random.Range(0, movePoints.Length)];
    	else
    		tSpawn = anim.transform;

        walkPoint = new Vector3(tSpawn.position.x, tSpawn.position.y, tSpawn.position.z);

       // if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        walkPointSet = true;
    }

    IEnumerator Heal()
    {
        isHealing = true;

        anim.SetBool("heal", true);

        yield return new WaitForSeconds(healTime);

        anim.SetBool("heal", false);

        currentHealth = maxHealth;

        isHealing = false;
    }

    void OnDrawGizmosSelected()
    {
    	Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, radius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, weaponCheckRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerCheckRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }

    public void TakeDamage(float amount)
    {
    	currentHealth -= amount;

        healthSlider.value = currentHealth;

    	audio.PlayOneShot(hurtSFX[Random.Range(0, hurtSFX.Length)]);

    	bloodVFX.Play();

    	anim.SetTrigger("hit");

    	if(currentHealth <= 0)
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

        if (!civil)
        {
            //StartCoroutine(database.UpdateItem("Survivals", database.survivals - 1));
            database.srvAmount -= 1;
            database.SendData("SRV Survival", database.srvAmount.ToString());
        }else if(civil && follow)
        {
            database.civilAmount -= 1;
            database.SendData("SRV Civils", database.civilAmount.ToString());
        }

        foreach (Collider c in col)
        {
            c.isTrigger = false;
        }

        gameObject.layer = 0;

        dead = true;

        Canvas.SetActive(false);
    }

    public void ClickPick()
    {
        

            if (civil)
        {
            if(database.civilAmount < 10)
            {
                statsInfo.SetActive(true);
                statsBTN.SetActive(false);

                //StartCoroutine(database.UpdateItem("Civils", database.civils + 1));

                database.civilAmount += 1;
                database.SendData("SRV Civils", database.civilAmount.ToString());

                follow = true;
                patrol = false;
            }else if(database.civilAmount >= 10)
            {
                poptext.PopText("Not Enough Slots");
            }
            

        }
        else if(!civil)
        {
            if (!stateSet && player.currentSurvivars < player.maxSurvivars)
            {
                follow = true;
                patrol = false;

                statTXT.text = "Unequip";

                player.currentSurvivars += 1;


                equippedStats();

                stateSet = true;
            }
            else if (stateSet)
            {
                follow = false;
                patrol = true;

                statTXT.text = "Equip";

                player.currentSurvivars -= 1;

                SendStats();

                stateSet = false;
            }

            if (player.currentSurvivars >= player.maxSurvivars)
            {
                poptext.PopText("Not Enough Slots");
            }
        }
    }

    public void SendStats()
    {
        //statsBTN.SetActive(true);
        //statsBTN.transform.GetComponent<RectTransform>().Rotate(0,0,0);
        //statsBTN.transform.SetParent(player.ui_Inventory.survivalSlot);
        //statsBTN.transform.position = new Vector3(0,0,0);
        //statsBTN.transform.localScale = new Vector3(1,1,1);
        //statsBTN.transform.GetComponent<RectTransform>().rotation = Quaternion.Euler(0,0,0);

        //InteractBTN.SetActive(false);

        equippedStats();
    }

    public void equippedStats()
    {
        statsBTN.SetActive(true);
        statsBTN.transform.GetComponent<RectTransform>().Rotate(0, 0, 0);
        statsBTN.transform.SetParent(player.ui_Inventory.equippedSlot);
        statsBTN.transform.position = new Vector3(0, 0, 0);
        statsBTN.transform.localScale = new Vector3(1, 1, 1);
        statsBTN.transform.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);

        InteractBTN.SetActive(true);
    }

    public void FootSFX()
    {
    	audio.PlayOneShot(footsSFX[Random.Range(0, footsSFX.Length)]);
    }

    void ToggleRagdoll(bool state)
    {
    	anim.enabled = !state;


    	foreach(Rigidbody rb in ragdollBodies)
    	{
    		rb.isKinematic = !state;
    	}
    }

    void FindClosesteEnemy()
    {
    	float distanceToClosesteEnemy = Mathf.Infinity;
    	zombie = null;

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

    	var targetT = zombie.transform.position;
        targetT.y = transform.position.y;
        transform.LookAt(targetT);
    }

    public void Attack()
    {
        Collider[] enemyCol = Physics.OverlapSphere(attackPoint.position, radius, TargetLayer);

        resetWeapon();

        audio.PlayOneShot(swingSFX[Random.Range(0, swingSFX.Length)]);

        foreach(Collider t in enemyCol)
        {
            Zombie_BP z = t.GetComponent<Zombie_BP>();
            if(z != null)
            {
                z.TakeDamage(damage);
            }

            Bandit_BP b = t.GetComponent<Bandit_BP>();
            if(b != null)
            {
                b.TakeDamage(damage);
            }
            
        }
    }

    public void Switch(int choice)
    {
        for(int i = 0; i < Weapons.Length; i++)
        {
            Weapons[i].SetActive(false);
        }

        for(int i = 0; i < guns.Length; i++)
        {
            guns[i].isReloading = false;
        }

        if(!civil)
        {
            Weapons[choice].SetActive(true);
        }
        else if(civil)
        {
            int rand = Random.Range(0, 3);
            if(rand == 0 || rand == 1)
            {
                Weapons[0].SetActive(true);
                melee = true;
                gun = false;
            }else
            {
                Weapons[1].SetActive(true);
                melee = false;
                gun = true;
            }
        }

        
    }

    public void resetWeapon()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].isReloading = false;
        }
    }

    public void WeaponClick()
    {
        player.ui_Inventory.Click_SurvivalWeaponPanel();
        player.ui_Inventory.SetSurvivalShop(this.GetComponent<NPC>());
    }
}
