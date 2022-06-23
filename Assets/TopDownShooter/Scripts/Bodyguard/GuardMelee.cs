using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMelee : MonoBehaviour
{
	
    public Bodyguard guard;
	public Animator anim;

	[Header("Stats")]
	public float fireRate;
	public float Damage;

	float nextTimeToFire = 0f;
	AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
    	
    }

    // Update is called once per frame
    void Update()
    {
    	if(guard.dead)return;

        if(guard.canFire && Time.time >= nextTimeToFire)
        {
          	anim.SetTrigger("Slash");
        	nextTimeToFire = Time.time + 1f/ fireRate;
        }
    }
}
