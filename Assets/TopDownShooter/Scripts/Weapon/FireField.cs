using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireField : MonoBehaviour
{
	public float damage;
	public float radius;
	public float burnDuration;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Burn());
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider nearbyObject in colliders)
        {
            Zombie_BP zombie = nearbyObject.GetComponent<Zombie_BP>();
            if(zombie != null)
            {
                zombie.Burn(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    IEnumerator Burn()
    {
        yield return new WaitForSeconds(burnDuration);

        Destroy(gameObject);
    }
}
