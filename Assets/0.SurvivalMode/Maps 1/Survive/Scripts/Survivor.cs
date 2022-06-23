using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor : MonoBehaviour
{
	public bool canFire, nearEnemy;
	public float enemyCheckRadius;
	public LayerMask enemyLayer;
	public Transform muzzle;
	public bool dead;

	[Header("GFX")]
	public GameObject[] Bodies;
    public GameObject[] Heads;
    public GameObject[] Legs;

    // Start is called before the first frame update
    void Start()
    {
        Customize();
    }

    // Update is called once per frame
    void Update()
    {
    	if(dead)return;

    	nearEnemy = Physics.CheckSphere(transform.position, enemyCheckRadius, enemyLayer);

        if(nearEnemy)
        {
            canFire = true;
            FindClosesteEnemy();
        }else
        {
            canFire = false;
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
	    muzzle.LookAt(target);
    	
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
        int H_R = Random.Range(1, Heads.Length);
        int L_R = Random.Range(0, Legs.Length);

        Bodies[B_R].SetActive(true);
        Heads[H_R].SetActive(true);
        Legs[L_R].SetActive(true);
    }
}
