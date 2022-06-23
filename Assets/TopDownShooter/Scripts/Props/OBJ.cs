using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class OBJ : MonoBehaviour
{
	public GameObject parentTransform;
	public float currentHealth;
	public float maxHealth;
	public bool isWall;
	public bool isDoor;
	public bool doorOpen;

    [Header("Spawner Stats")]
    public bool isSpawner;
    public bool isStatic;
    public GameObject SpawnObj;
    public Transform[] spawnPos;
    public float spawnDelay;

    [Header("GFX")]
    public NavMeshObstacle[] navs;
    public MeshRenderer[] meshes;
    public Collider[] colls;

	[Header("SFX")]
	public AudioClip[] placeSFX;
    public AudioClip[] hitSFX;
    public AudioClip[] destroySFX;

	[Header("VFX")]
	public ParticleSystem DustVFX;

	[Header("UI")]
	public GameObject interactOBJ;
	public TextMeshProUGUI interactTXT;


	AudioSource audio;
	Transform player;
	Animator anim;
    Rigidbody rb;
    public bool destroyed;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if(isWall)
        {
        	DustVFX.Play();
        	audio.PlayOneShot(placeSFX[Random.Range(0, placeSFX.Length)]);
        }

        if(isSpawner && !isStatic)
        {
            rb = GetComponent<Rigidbody>();
        }

        if(isDoor)
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
    	if(isDoor)
    	{
	        float Distance = Vector3.Distance(transform.position, player.position);

	        if(Distance <= 1f)
	        {
	        	interactOBJ.SetActive(true);
	        }else
	        {
	        	interactOBJ.SetActive(false);
	        }
	    }
    }

    public void TakeDamage(float amount)
    {
        if (destroyed) return;

    	currentHealth -= amount;

        audio.PlayOneShot(hitSFX[Random.Range(0, hitSFX.Length)]);

        DustVFX.Play();

        if (!isSpawner)
        {
           if(currentHealth <= 0)
            {
                foreach(MeshRenderer m in meshes)
                {
                    m.enabled = false;
                }

                foreach(Collider c in colls)
                {
                    c.enabled = false;
                }

                foreach(NavMeshObstacle n in navs)
                {
                    n.enabled = false;
                }

                audio.PlayOneShot(destroySFX[Random.Range(0, destroySFX.Length)]);

                DustVFX.Play();

                Destroy(parentTransform, 5f);

                destroyed = true;
            } 
        }

    	if(isSpawner && currentHealth <= 0)
        {
            DustVFX.Play();
            
            if(!isStatic)
            {
                rb.isKinematic = false;

                float rotX = Random.Range(0f, 5f);
                float rotY = Random.Range(0f, 5f);
                float rotZ = Random.Range(0f, 5f);

                transform.Rotate(rotX, rotY, rotZ);
            }

            audio.PlayOneShot(destroySFX[Random.Range(0, destroySFX.Length)]);

            
            StartCoroutine(Spawn());

            destroyed = true;
        }
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        player.GetComponent<AudioSource>().PlayOneShot(destroySFX[Random.Range(0, destroySFX.Length)]);

        for (int i = 0; i < spawnPos.Length; i++)
        {
            Instantiate(SpawnObj, spawnPos[i].position, spawnPos[i].rotation);
            
        }

        Destroy(gameObject);
    }

    public void Interact()
    {
    	if(!doorOpen)
    	{
    		anim.SetBool("open", true);
    		interactTXT.text = "close";
    		doorOpen = true;
    	}else if(doorOpen)
    	{
    		anim.SetBool("open", false);
    		interactTXT.text = "open";
    		doorOpen = false;
    	}
    }
}
