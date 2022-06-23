using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadbody : MonoBehaviour
{
	public GameObject[] Bodies;
    public GameObject[] Heads;
    public GameObject[] Legs;

    Rigidbody[] ragdollBodies;
    Collider[] colliders;

    // Start is called before the first frame update
    void Start()
    {
    	ragdollBodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();

        Customize();

        ToggleRagdoll(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ToggleRagdoll(bool state)
    {
    	foreach(Rigidbody rb in ragdollBodies)
    	{
    		rb.isKinematic = !state;
    	}
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
        int H_R = Random.Range(0, Heads.Length);
        int L_R = Random.Range(0, Legs.Length);

        Bodies[B_R].SetActive(true);
        Heads[H_R].SetActive(true);
        Legs[L_R].SetActive(true);
    }
}
