using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_BP : MonoBehaviour
{
	public Zombie zombie;
	public float additionalDamage;
	public bool dead;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    	if(dead)return;
        if(zombie.dead)
        {
        	dead = true;
        }
    }

    public void TakeDamage(float amount)
    {
    	if(dead)return;
    	zombie.TakeDamage(amount + additionalDamage);
    }

    public void Burn(float amount)
    {
    	if(dead)return;
    	zombie.Burn(amount);
    }
}
