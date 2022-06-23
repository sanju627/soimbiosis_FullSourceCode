using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
	public Transform[] spawnPoints;
	public GameObject[] zombies;
	public GameObject Boss;
	public float spawnRate;
	public float spawnRate_B;
	float nextSpawn = 0f;
	float nextSpawn_B = 0f;
    public Player[] players;
    public Player currentPlayer;
    public WeaponManger weaponManager;


    // Start is called before the first frame update
    void Start()
    {
        Spawn_B();
    }

    // Update is called once per frame
    void Update()
    {

        if(Time.time >= nextSpawn)
        {
        	Spawn();
        	nextSpawn = Time.time + 1f/ spawnRate;
        }

    }

    void Spawn()
    {
    	int randomZ = Random.Range(0, zombies.Length);

    	Transform tSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

    	Instantiate(zombies[randomZ], tSpawn.position, tSpawn.rotation);
    }

    void Spawn_B()
    {
    	Transform tSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

    	Instantiate(Boss, tSpawn.position, tSpawn.rotation);
    }
}
