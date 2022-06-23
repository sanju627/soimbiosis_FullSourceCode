using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopStation : MonoBehaviour
{
    public Transform spawnStations;
    public Transform dogSpawnPos;
    public Transform[] srvSpawnPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
    	if(col.transform.tag == "Player")
    	{
    		//col.transform.GetComponent<WeaponManger>().OpenShop();
    	}
    }
}
