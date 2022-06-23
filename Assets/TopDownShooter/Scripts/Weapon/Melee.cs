using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class Melee : MonoBehaviour
{
    public Player player;
    public float attackRate;
    public float damage;
    public float atkRange;
    public Sprite weaponSprite;
    public Image weaponImage;

    [Header("SFX")]
    public AudioClip[] SwingSFX;

    float nextTimeToAttack = 0f;
    WeaponManger weapon;
    Animator anim;
    Combat combat;
    popTXT poptext;
    EnergySystem energySystem;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        anim = player.GetComponent<Animator>();
        weapon = player.GetComponent<WeaponManger>();
        combat = player.GetComponent<Combat>();
        energySystem = player.GetComponent<EnergySystem>();

        poptext = GameObject.FindGameObjectWithTag("MSG").GetComponent<popTXT>();
    }

    // Update is called once per frame
    void Update()
    {
        combat.atkRange = atkRange;
        combat.damage = damage;
        combat.SwingSFX = SwingSFX;

        weaponImage.sprite = weaponSprite;


        anim.SetLayerWeight(anim.GetLayerIndex("Rifle"), 0);
        anim.SetLayerWeight(anim.GetLayerIndex("Pistol"), 0);
        anim.SetLayerWeight(anim.GetLayerIndex("Melee"), 1);
        anim.SetLayerWeight(anim.GetLayerIndex("Item"), 0);

        if (CrossPlatformInputManager.GetButton("Fire1") && Time.time >= nextTimeToAttack && !weapon.isSwitching && !player.isRunning && energySystem.energy >= 3f)
        {
            int rand = Random.Range(0, 6);

            if (rand == 0 || rand == 1 || rand == 2 || rand == 3)
            {
                anim.SetTrigger("attack1");
            }else
            {
                anim.SetTrigger("attack2");
            }

            

            nextTimeToAttack = Time.time + 1f / attackRate;

        }

        if(CrossPlatformInputManager.GetButton("Fire1") && Time.time >= nextTimeToAttack && !weapon.isSwitching && !player.isRunning && energySystem.energy <= 3f)
        {
            poptext.PopText("Not Enough Energy");
        }
    }
}
