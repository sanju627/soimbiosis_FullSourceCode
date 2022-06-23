using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ThrowableItem : MonoBehaviour
{
	public Animator anim;

	[Header("Wall")]
	public bool isWall;
	public bool isMetalWall;
	public bool isWoodDoor;
	public bool isMetalDoor;
    public bool isGrenade;
    public bool isSmoke;
    public bool isMolo;
	public Transform buildPos;
	public Transform BlueprintPos;
	public GameObject WallPrefab;
	public float range = 4f;
	RaycastHit hit;

	ItemAssets itemAssets;
	WeaponManger weaponManager;
	Player player;

    // Start is called before the first frame update
    void Start()
    {
    	itemAssets = anim.GetComponent<ItemAssets>();
    	weaponManager = anim.GetComponent<WeaponManger>();
    	player = anim.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
    	if(isWall || isMetalWall || isWoodDoor || isMetalDoor)
        	BlueprintPos.gameObject.SetActive(true);
        else
        	BlueprintPos.gameObject.SetActive(false);

        if(isWall || isMetalWall || isWoodDoor || isMetalDoor)
        {
        	if(isWall)
        	{
        		player.woodWall = true;
        		player.metalWall = false;
        		player.woodDoor = false;
        		player.metalDoor = false;
        	}

        	if(isMetalWall)
        	{
        		player.woodWall = false;
        		player.metalWall = true;
        		player.woodDoor = false;
        		player.metalDoor = false;
        	}

        	if(isWoodDoor)
        	{
        		player.woodWall = false;
        		player.metalWall = false;
        		player.woodDoor = true;
        		player.metalDoor = false;
        	}

        	if(isMetalDoor)
        	{
        		player.woodWall = false;
        		player.metalWall = false;
        		player.woodDoor = false;
        		player.metalDoor = true;
        	}

        	player.Smoke = false;
		    player.Grenade = false;
		    player.LandMine = false;
		    player.Medkit = false;
		    player.Molo = false;
        }

        if(isGrenade)
        {
            player.Smoke = false;
            player.Grenade = true;
            player.LandMine = false;
            player.Medkit = false;
            player.Molo = false;
            player.woodWall = false;
            player.metalWall = false;
            player.woodDoor = false;
            player.metalDoor = false;

            if (CrossPlatformInputManager.GetButton("Fire1"))
            {
                anim.SetTrigger("Fire");
            }
        }

        if(isSmoke)
        {
            player.Smoke = true;
            player.Grenade = false;
            player.LandMine = false;
            player.Medkit = false;
            player.Molo = false;
            player.woodWall = false;
            player.metalWall = false;
            player.woodDoor = false;
            player.metalDoor = false;

            if (CrossPlatformInputManager.GetButton("Fire1"))
            {
                anim.SetTrigger("Fire");
            }
        }

        if (isMolo)
        {
            player.Smoke = false;
            player.Grenade = false;
            player.LandMine = false;
            player.Medkit = false;
            player.Molo = true;
            player.woodWall = false;
            player.metalWall = false;
            player.woodDoor = false;
            player.metalDoor = false;

            if (CrossPlatformInputManager.GetButton("Fire1"))
            {
                anim.SetTrigger("Fire");
            }
        }

        anim.SetLayerWeight(anim.GetLayerIndex("Rifle"), 0);
        anim.SetLayerWeight(anim.GetLayerIndex("Item"), 1);
        anim.SetLayerWeight(anim.GetLayerIndex("Pistol"), 0);

        if(isWall && itemAssets.wallAmount <= 0)
        {
        	weaponManager.SelectMelee(weaponManager.meleeWeaponIndex);
        }

        if(isMetalWall && itemAssets.metalWallAmount <= 0)
        {
            weaponManager.SelectMelee(weaponManager.meleeWeaponIndex);
        }

        if(isWoodDoor && itemAssets.woodDoorAmount <= 0)
        {
            weaponManager.SelectMelee(weaponManager.meleeWeaponIndex);
        }

        if(isMetalDoor && itemAssets.metalDoorAmount <= 0)
        {
            weaponManager.SelectMelee(weaponManager.meleeWeaponIndex);
        }

        if(isGrenade && itemAssets.grenadeAmount <= 0)
        {
            weaponManager.SelectMelee(weaponManager.meleeWeaponIndex);
        }

        if(isSmoke && itemAssets.smokeAmount <= 0)
        {
            weaponManager.SelectMelee(weaponManager.meleeWeaponIndex);
        }

        if(isMolo && itemAssets.molotovAmount <= 0)
        {
            weaponManager.SelectMelee(weaponManager.meleeWeaponIndex);
        }

        if (isWall || isMetalWall || isWoodDoor || isMetalDoor)
        {
        	Build();
        }
    }

    public void Build()
    {
    	if(Physics.Raycast(buildPos.position, buildPos.forward, out hit, range))
        {
        	BlueprintPos.position = new Vector3(Mathf.RoundToInt(hit.point.x) != 0 ? Mathf.RoundToInt(hit.point.x/1) * 1 : 1
        	, (Mathf.RoundToInt(hit.point.y) != 0 ? Mathf.RoundToInt(hit.point.y/1) * 1 : 0)
        	, Mathf.RoundToInt(hit.point.z) != 0 ? Mathf.RoundToInt(hit.point.z/1) * 1 : 1);

        	BlueprintPos.eulerAngles = new Vector3(0, Mathf.RoundToInt(transform.eulerAngles.y) != 0 ? Mathf.RoundToInt(transform.eulerAngles.y / 90f) * 90f : 0, 0);

        	if(CrossPlatformInputManager.GetButton("Fire1"))
        	{
        		anim.SetTrigger("Fire");
        	}
        }
    }

    public void Throw()
    {
        if(isWall || isMetalWall || isWoodDoor || isMetalDoor)
    	Instantiate(WallPrefab, BlueprintPos.position, BlueprintPos.rotation);

    	if(isWall)
        player.inventory.RemoveItem(new Item {itemType = Item.ItemType.wall, amount = 1});

        if(isMetalWall)
        player.inventory.RemoveItem(new Item {itemType = Item.ItemType.metalWall, amount = 1});

        if(isWoodDoor)
        player.inventory.RemoveItem(new Item {itemType = Item.ItemType.woodDoor, amount = 1});

        if(isMetalDoor)
        player.inventory.RemoveItem(new Item {itemType = Item.ItemType.metalDoor, amount = 1});
    }
}
