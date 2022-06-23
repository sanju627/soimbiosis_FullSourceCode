using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld_Spawner : MonoBehaviour
{
    public Item item;

    void Start()
    {
    	ItemWorld.SpawnItemWorld(transform.position, item);
    	Destroy(gameObject);
    }
}
