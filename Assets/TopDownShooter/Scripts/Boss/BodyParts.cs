using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyParts : MonoBehaviour
{
    public bool head;
	public Brute brute;
	public float additionalDamage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float amount)
    {
    	brute.TakeDamage(amount + additionalDamage);
    }

    public void TakeExplosionDamage()
    {
        brute.ExplosionDamage();
    }
}
