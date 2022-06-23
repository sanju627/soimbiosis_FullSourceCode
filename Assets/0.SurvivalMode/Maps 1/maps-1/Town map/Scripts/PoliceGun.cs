using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceGun : MonoBehaviour
{
    public Police bandit;
	public Animator anim;
	public Transform muzzle;
	public Transform gunMuzzle;
	public float Recoil;
	public float fireRate;
	public float Damage;
	public float range;
	public int currentAmmo;
	public int maxAmmo;
	public bool isReloading;
	public float reloadTime;

	[Header("AudioClips")]
	public AudioClip fireSFX;	
  	public AudioClip reloadSFX;
  	public AudioClip[] bloodHitSFX;
  	public AudioClip[] metalHitSFX;
  	public AudioClip[] wallHitSFX;

	[Header("VFX")]
	public ParticleSystem muzzleFlash;
	public GameObject sparkEffect;
  	public GameObject wallEffect;
  	public GameObject bloodEffect;
  	public TrailRenderer BulletTrail;

	
	float nextTimeToFire = 0f;
	AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
    	if(bandit.dead)return;

        //Reloading
       	if(currentAmmo <= 0 && !isReloading)
       	{
       		StartCoroutine(reload());
       	}

        if(bandit.canAttack && Time.time >= nextTimeToFire && !isReloading && currentAmmo > 0 && !isReloading)
        {
          	Fire();
          	anim.SetTrigger("Fire");
        	nextTimeToFire = Time.time + 1f/ fireRate;
        }
    }

    void Fire()
    {
    	currentAmmo--;
    	muzzleFlash.Play();
    	audio.PlayOneShot(fireSFX);

    	Vector3 shootDirection = muzzle.forward;
      	shootDirection.x += Random.Range(Recoil, -Recoil);
      	shootDirection.y += Random.Range(Recoil, -Recoil);

      	RaycastHit hit;
    	if(Physics.Raycast(muzzle.position, shootDirection, out hit, range))
    	{
	        //Trail
    		if(hit.transform.tag == "Metal")
	        {
		        GameObject hitEffect = Instantiate(sparkEffect, hit.point, Quaternion.LookRotation(hit.normal));
		        Destroy(hitEffect, 2f);
		        audio.PlayOneShot(metalHitSFX[Random.Range(0, metalHitSFX.Length)]);
	        }

	        if(hit.transform.tag == "Wall")
	        {
		        GameObject hitEffect = Instantiate(wallEffect, hit.point, Quaternion.LookRotation(hit.normal));
		        Destroy(hitEffect, 2f);
		        audio.PlayOneShot(wallHitSFX[Random.Range(0, wallHitSFX.Length)]);
	        }

	        Zombie_BP zom = hit.transform.GetComponentInChildren<Zombie_BP>();
          if(zom != null)
          {
            zom.TakeDamage(Damage);

            GameObject hitEffect = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(hitEffect, 2f);
            audio.PlayOneShot(bloodHitSFX[Random.Range(0, bloodHitSFX.Length)]);
          }
      	}

    }

    IEnumerator reload()
    {
    	isReloading = true;
    	anim.SetBool("Reload", true);

    	audio.PlayOneShot(reloadSFX);

    	yield return new WaitForSeconds(reloadTime);

    	anim.SetBool("Reload", false);

      	currentAmmo = maxAmmo;

    	isReloading = false;
    }

    IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit hit)
    {
      float time = 0; 
      Vector3 StartPosition = Trail.transform.position;

      while (time < 0.1f)
      {
        Trail.transform.position = Vector3.Lerp(StartPosition, hit.point, time);
        time += Time.deltaTime / Trail.time;

        yield return null;
      }
      Trail.transform.position = hit.point;

        Destroy(Trail.gameObject, Trail.time);
    }
}
