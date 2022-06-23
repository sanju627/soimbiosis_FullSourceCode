using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreManager : MonoBehaviour
{
    public GameObject player;
    public Transform[] wanderPositions;

    [Header("Bandit")]
    public int totalBandit;
    public GameObject[] bandit;
    public Transform[] banditPos;

    [Header("Enemies")]
    public int totalZombies;
    public GameObject[] zombies;
    public GameObject[] zombieBosses;
    public Transform[] zombieSpawnPositions;
    public Transform zombieBossSpawnPos;
    [Space]
    public int currentEnemies;
    public Zombie[] enemyList;

    [Header("Civilians")]
    public int totalCivivls;
    public GameObject[] civilians;
    public Transform[] civilPositions;
    
    [Header("Civilians")]
    public GameObject[] guards;
    public Transform[] guardsPos;

    [Header("Survival")]
    public Transform[] srv_spawnPositions;

    [Header("Vehicles")]
    public GameObject[] Vehicles;
    public int vehicleIndex;
    public Transform vehSpawnPos;
    
    [Header("Dogs")]
    public GameObject[] Dogs;
    public int dogIndex;
    public Transform dog_spawnPositions;

    PlayfabManager database;


    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(player, spawnPos.position, spawnPos.rotation);

        database = GameObject.FindGameObjectWithTag("Database").GetComponent<PlayfabManager>();

        StartCoroutine(LoadData());

        
    }

    IEnumerator LoadData()
    {
        yield return new WaitForSeconds(0.5f);

        GetData();
    }

    // Update is called once per frame
    void Update()
    {
        enemyList = GameObject.FindObjectsOfType<Zombie>();

        currentEnemies = enemyList.Length;

        if (currentEnemies < totalZombies)
        {
            Spawn();
        }

    }

    void GetData()
    {
        for (int i = 0; i < totalZombies; i++)
        {
            Spawn();
        }

        for (int i = 0; i < totalCivivls; i++)
        {
            CivilSpawn();
        }

        for (int i = 0; i < totalBandit; i++)
        {
            BanditSpawn();
        }

        guardSpawn();

        vehicleIndex = database.carVehicleIndex;
        if (vehicleIndex > 0)
        {
            Instantiate(Vehicles[vehicleIndex], vehSpawnPos.position, vehSpawnPos.rotation);
        }
        
        dogIndex = database.dogIndex;
        if (dogIndex > 0)
        {
            Instantiate(Dogs[dogIndex], dog_spawnPositions.position, dog_spawnPositions.rotation);
        }


        int rand = Random.Range(0, zombieBosses.Length);
        Instantiate(zombieBosses[rand], zombieBossSpawnPos.position, zombieBossSpawnPos.rotation);

    }

    void Spawn()
    {
        Transform tSpawn = zombieSpawnPositions[Random.Range(0, zombieSpawnPositions.Length)];
        Instantiate(zombies[Random.Range(0, zombies.Length)], tSpawn.position, tSpawn.rotation);
    }

    void CivilSpawn()
    {
        Transform tSpawn = civilPositions[Random.Range(0, civilPositions.Length)];
        Instantiate(civilians[Random.Range(0, civilians.Length)], tSpawn.position, tSpawn.rotation);
    }

    void BanditSpawn()
    {
        Transform tSpawn = banditPos[Random.Range(0, banditPos.Length)];
        Instantiate(bandit[Random.Range(0, bandit.Length)], tSpawn.position, tSpawn.rotation);
    }
    
    void guardSpawn()
    {
        for (int i = 0; i < guardsPos.Length; i++)
        {
            Instantiate(guards[Random.Range(0, guards.Length)], guardsPos[i].position, guardsPos[i].rotation);
        }
    }
}
