using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;
using TMPro;
using UnityStandardAssets.CrossPlatformInput;

public class AR_Rifle : MonoBehaviour
{
    [Header("Player")]
    public Player player;
    public WeaponManger weapon;
    public Animator anim;
    public bool Pistol;
    public bool Rifle;
    public bool flamethrower;
    public float x;
    public float z;

    [Header("Stats")]
    public Transform muzzle;
    public Transform gunMuzzle;
    public float fireRate;
    public float Damage;
    public float range;
    public int magazine;
    public int magSize;
    public int totalAmmo;
    public float reloadTime;
    public bool isReloading;
    public float Recoil;

    [Header("VFX")]
    public ParticleSystem muzzleFlash;
    public ParticleSystem FlameVFX;
    public GameObject sparkEffect;
    public GameObject wallEffect;
    public GameObject bloodEffect;
    public GameObject vehicleEffect;
    public GameObject OBJEffect;
    public TrailRenderer BulletTrail;

    [Header("AudioClips")]
    public AudioClip fireSFX;
    public AudioClip reloadSFX;
    public AudioClip[] bloodHitSFX;
    public AudioClip[] metalHitSFX;
    public AudioClip[] wallHitSFX;

    [Header("UI")]
    public TextMeshProUGUI ammoTXT;
    public Image weaponSlot;
    public Sprite weaponImage;

    float nextTimeToFire = 0f;
    float lastAmount;
    float minusAmmoCount;
    DataImporter dataImporter;

    int magazineTamp;

    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Player>();
        weapon = player.GetComponent<WeaponManger>();
        anim = player.GetComponent<Animator>();
        dataImporter = player.GetComponent<DataImporter>();
        audio = GetComponent<AudioSource>();

        //ammo = magazine * mags;
        magazineTamp = magazine;
    }

    // Update is called once per frame
    void Update()
    {
        totalAmmo = player.GetComponent<ItemAssets>().ammoAmount;

        if (Pistol)
        {
            anim.SetLayerWeight(anim.GetLayerIndex("Rifle"), 0);
            anim.SetLayerWeight(anim.GetLayerIndex("Item"), 0);
            anim.SetLayerWeight(anim.GetLayerIndex("Melee"), 0);
            anim.SetLayerWeight(anim.GetLayerIndex("Pistol"), 1);
        }

        if (Rifle || flamethrower)
        {
            anim.SetLayerWeight(anim.GetLayerIndex("Rifle"), 1);
            anim.SetLayerWeight(anim.GetLayerIndex("Pistol"), 0);
            anim.SetLayerWeight(anim.GetLayerIndex("Melee"), 0);
            anim.SetLayerWeight(anim.GetLayerIndex("Item"), 0);
        }

        //Reloading
        if (magazine <= 0 && !isReloading && !weapon.isSwitching && totalAmmo > 0)
        {
            StartCoroutine(reload());
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && !weapon.isSwitching && magazine < magSize && totalAmmo > 0)
        {
            StartCoroutine(reload());
        }

        // x = player.rotJoystick.Horizontal;
        // z = player.rotJoystick.Vertical;

        if (CrossPlatformInputManager.GetButton("Fire1") && Time.time >= nextTimeToFire && !isReloading && !weapon.isSwitching && magazine > 0 && !player.isRunning)
        {
            if (!flamethrower)
            {
                Fire();
            }
            else if (flamethrower)
            {
                Flame();
                FlameVFX.Play();
            }

            nextTimeToFire = Time.time + 1f / fireRate;
        }
        else if (flamethrower)
        {
            FlameVFX.Stop();
        }



        //UI
        ammoTXT.text = magazine.ToString("0") + " / " + totalAmmo.ToString("0");
        weaponSlot.sprite = weaponImage;
    }

    void Fire()
    {
        Vector3 shootDirection = muzzle.forward;
        shootDirection.x += Random.Range(Recoil, -Recoil);
        shootDirection.y += Random.Range(Recoil, -Recoil);

        magazine--;
        muzzleFlash.Play();
        audio.PlayOneShot(fireSFX);
        anim.SetTrigger("Fire");

        RaycastHit hit;
        if (Physics.Raycast(muzzle.position, shootDirection, out hit, range))
        {
            //Trail
            if (hit.transform.tag == "Metal")
            {
                GameObject hitEffect = Instantiate(sparkEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(hitEffect, 2f);
                audio.PlayOneShot(metalHitSFX[Random.Range(0, metalHitSFX.Length)]);
            }

            if (hit.transform.tag == "Shop")
            {
                GameObject hitEffect = Instantiate(sparkEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(hitEffect, 2f);
                audio.PlayOneShot(metalHitSFX[Random.Range(0, metalHitSFX.Length)]);
            }

            if (hit.transform.tag == "Wall")
            {
                GameObject hitEffect = Instantiate(wallEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(hitEffect, 2f);
                audio.PlayOneShot(wallHitSFX[Random.Range(0, wallHitSFX.Length)]);
            }

            if (hit.transform.tag == "Vehicle")
            {
                GameObject hitEffect = Instantiate(vehicleEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(hitEffect, 2f);
                audio.PlayOneShot(metalHitSFX[Random.Range(0, metalHitSFX.Length)]);
            }

            Zombie_BP zom = hit.transform.GetComponentInChildren<Zombie_BP>();
            if (zom != null)
            {
                zom.TakeDamage(Damage);

                GameObject hitEffect = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(hitEffect, 2f);
                audio.PlayOneShot(bloodHitSFX[Random.Range(0, bloodHitSFX.Length)]);
            }

            CarController veh = hit.transform.GetComponent<CarController>();
            if (veh != null)
            {
                veh.TakeDamage(Damage);

                GameObject hitEffect = Instantiate(vehicleEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(hitEffect, 2f);
                audio.PlayOneShot(metalHitSFX[Random.Range(0, metalHitSFX.Length)]);
            }

            HelicopterController hel = hit.transform.GetComponent<HelicopterController>();
            if (hel != null)
            {
                hel.TakeDamage(Damage);

                GameObject hitEffect = Instantiate(vehicleEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(hitEffect, 2f);
                audio.PlayOneShot(metalHitSFX[Random.Range(0, metalHitSFX.Length)]);
            }

            BodyParts targetBoss = hit.transform.GetComponentInChildren<BodyParts>();
            if (targetBoss != null)
            {
                targetBoss.TakeDamage(Damage);

                GameObject hitEffect = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(hitEffect, 2f);
                audio.PlayOneShot(bloodHitSFX[Random.Range(0, bloodHitSFX.Length)]);
            }

            Bandit_BP bp = hit.transform.GetComponentInChildren<Bandit_BP>();
            if (bp != null)
            {
                bp.TakeDamage(Damage);

                GameObject hitEffect = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(hitEffect, 2f);
                audio.PlayOneShot(bloodHitSFX[Random.Range(0, bloodHitSFX.Length)]);
            }

            TrailRenderer trail = Instantiate(BulletTrail, gunMuzzle.position, gunMuzzle.rotation);

            StartCoroutine(SpawnTrail(trail, hit));
        }
    }

    void Flame()
    {
        magazine--;
        //muzzleFlash.Play();
        audio.PlayOneShot(fireSFX);
        anim.SetTrigger("Fire");

        RaycastHit hit;
        if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, range))
        {
            //Trail
            // if(hit.transform.tag == "Metal")
            //   {
            //     GameObject hitEffect = Instantiate(sparkEffect, hit.point, Quaternion.LookRotation(hit.normal));
            //     Destroy(hitEffect, 2f);
            //     audio.PlayOneShot(metalHitSFX[Random.Range(0, metalHitSFX.Length)]);
            //   }

            //   if(hit.transform.tag == "Wall")
            //   {
            //     GameObject hitEffect = Instantiate(wallEffect, hit.point, Quaternion.LookRotation(hit.normal));
            //     Destroy(hitEffect, 2f);
            //     audio.PlayOneShot(wallHitSFX[Random.Range(0, wallHitSFX.Length)]);
            //   }

            Zombie_BP target = hit.transform.GetComponentInChildren<Zombie_BP>();
            if (target != null)
            {
                Debug.Log("Hit");
                target.Burn(Damage);
            }

            CarController veh = hit.transform.GetComponent<CarController>();
            if (veh != null)
            {
                veh.TakeDamage(Damage);
            }

            BodyParts targetBoss = hit.transform.GetComponentInChildren<BodyParts>();
            if (targetBoss != null)
            {
                targetBoss.TakeDamage(Damage);

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

        totalAmmo = totalAmmo - magSize + magazine;
        magazine = magazineTamp;

        player.inventory.RemoveItem(new Item {itemType = Item.ItemType.ammo, amount = magazine});

        dataImporter.SendInventoryData();

        if (totalAmmo < 0)
        {
            magazine += totalAmmo;
            totalAmmo = 0;
        }

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
