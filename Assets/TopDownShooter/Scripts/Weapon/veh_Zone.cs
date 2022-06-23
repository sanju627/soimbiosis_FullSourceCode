using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class veh_Zone : MonoBehaviour
{
	public CarController controller;

	public float radius;
	public LayerMask enemyLayer;
	public float damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    	if(controller.speed >= 5f)
        {
            Attack();
        }
        
    }

    public void Attack()
    {
        Collider[] enemyCol = Physics.OverlapSphere(transform.position, radius, enemyLayer);

        foreach(Collider e in enemyCol)
        {
        	Zombie_BP z = e.GetComponent<Zombie_BP>();
            if(z != null)
            {
                z.GetComponent<Zombie_BP>().TakeDamage(damage);
            }

            Bandit_BP b = e.GetComponent<Bandit_BP>();
            if (b != null)
            {
                b.TakeDamage(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
    	Gizmos.DrawWireSphere(transform.position, radius);
    }
}
