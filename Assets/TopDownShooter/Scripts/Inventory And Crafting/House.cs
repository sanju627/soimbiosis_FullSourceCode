using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
	public GameObject Roof;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter()
    {
    	Roof.SetActive(false);
    }

    void OnTriggerExit()
    {
    	Roof.SetActive(true);
    }
}
