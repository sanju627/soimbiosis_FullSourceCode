using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public Transform[] spawnPosition;
	public GameObject[] spawnObjects;
	public GameObject[] rareObjects;
	public int rand;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
    	for(int i = 0; i < spawnPosition.Length; i++)
    	{
    		rand = Random.Range(0, 10);

    		GameObject spawnsObj = spawnObjects[Random.Range(0, spawnObjects.Length)];
    		Instantiate(spawnsObj, spawnPosition[i].position, spawnPosition[i].rotation);

    		if(rand == 0)
    		{
    			GameObject spawnsObj_R = rareObjects[Random.Range(0, rareObjects.Length)];
    			Instantiate(spawnsObj_R, spawnPosition[i].position, spawnPosition[i].rotation);
    		}
    	}
    }
}
