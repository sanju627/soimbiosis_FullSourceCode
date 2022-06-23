using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ItemAssets : MonoBehaviour
{
	public static ItemAssets Instance {get; private set;}

	// ammo,
	// medkit,
	// bandage,
	// grenade,
	// molotov,
	// smoke,
	// landmine,

	void Awake()
	{
		Instance = this;
        //database = GameObject.FindGameObjectWithTag("Database").GetComponent<AuthManager>();
        //database.LoadData();
    }

    public List<Item> itemList;
    [Space]
    public Transform itemWorldPrefab;
	[Space]

    public Sprite ammoSprite; 
    public Sprite medkitSprite;
    public Sprite bandageSprite;
    public Sprite grenadeSprite;
    public Sprite molotovSprite;
    public Sprite smokeSprite;
    public Sprite landmineSprite;
    public Sprite chickenSprite;
    public Sprite woodSprite;
    public Sprite stoneSprite;
    [Space]
    //----------------------------------------------------------Weapon---------------------------------------------//
    public Sprite krissSprite;
    public Sprite mp7Sprite;
    public Sprite mp5Sprite;
    public Sprite tec9Sprite;
    public Sprite umpSprite;
    public Sprite uziSprite;
    public Sprite ak12Sprite;
    public Sprite ak74Sprite;
    public Sprite g3a4Sprite;
    public Sprite g36cSprite;
    public Sprite flamethrowerSprite;
    public Sprite glock17Sprite;
	public Sprite wallSprite;
    public Sprite metalSprite;
    public Sprite woodDoorSprite;
    public Sprite metalDoorSprite;

	[Header("Amounts")]
	public int ammoAmount;
    public int medkitAmount;
    public int bandageAmount;
    [Space]
    public int grenadeAmount;
    public int molotovAmount;
    public int smokeAmount;
    public int landmineAmount;
    public int chickenAmount;
    [Space]
    public int woodAmount;
    public int stoneAmount;
    public int wallAmount;
    public int metalWallAmount;
    public int woodDoorAmount;
    public int metalDoorAmount;
    [Space]
    public int krissAmount;
    public int mp7Amount;
    public int mp5Amount;
    public int ump45Amount;
    public int tec9Amount;
    public int uziAmount;
    public int ak12Amount;
    public int ak74Amount;
    public int g3a4Amount;
    public int g36cAmount;
    public int flamethrowerAmount;
    public int glock17Amount;
    [Space]
    public int civils;
    public int survivals;
    public int vehiclesIndex;
    public int dogIndex;

    Inventory inventory;
    PlayfabManager database;
    DataImporter data;
    Player player;

    void Start()
    {
        //civils = database.civils;
        //vehiclesIndex = database.vehiclesIndex;
        database = GameObject.FindGameObjectWithTag("Database").GetComponent<PlayfabManager>();
        data = GetComponent<DataImporter>();
        //database.GetData();
    }

    void Update()
    {
        inventory = GetComponent<Player>().inventory;
        player = GetComponent<Player>();
        
        if(data.dataLoaded)
        CheckItem();
    }

	void CheckItem()
    {
        itemList = GetComponent<Player>().inventory.itemList;

        ammoAmount = inventory.ammoAmount;
        medkitAmount = inventory.medkitAmount;
        bandageAmount = inventory.bandageAmount;
        grenadeAmount = inventory.grenadeAmount;
        molotovAmount = inventory.molotovAmount;
        smokeAmount = inventory.smokeAmount;
        landmineAmount = inventory.landmineAmount;
        chickenAmount = inventory.chickenAmount;
        woodAmount = inventory.woodAmount;
        stoneAmount = inventory.stoneAmount;
        wallAmount = inventory.wallAmount;
        metalWallAmount = inventory.metalWallAmount;
        woodDoorAmount = inventory.woodDoorAmount;
        metalDoorAmount = inventory.metalDoorAmount;
        krissAmount = inventory.krissAmount;
        mp7Amount = inventory.mp7Amount;
        mp5Amount = inventory.mp5Amount;
        ump45Amount = inventory.ump45Amount;
        tec9Amount = inventory.tec9Amount;
        uziAmount = inventory.uziAmount;
        ak12Amount = inventory.ak12Amount;
        ak74Amount = inventory.ak74Amount;
        g3a4Amount = inventory.g3a4Amount;
        g36cAmount = inventory.g36cAmount;
        flamethrowerAmount = inventory.flamethrowerAmount;
        glock17Amount = inventory.glock17Amount;
    }

    public void RemoveItems()
    {
        foreach (Item inventoryItem in itemList)
        {
            inventory.RemoveItem(inventoryItem);

            switch (inventoryItem.itemType)
            {
                case Item.ItemType.ammo:
                    database.SendData("Item Ammo", inventory.ammoAmount.ToString());
                    break;
                case Item.ItemType.medkit:
                    database.SendData("Item Medkit", inventory.medkitAmount.ToString());
                    break;
                case Item.ItemType.bandage:
                    database.SendData("Item Bandage", inventory.bandageAmount.ToString());
                    break;
                case Item.ItemType.grenade:
                    database.SendData("Item Grenade", inventory.grenadeAmount.ToString());
                    break;
                case Item.ItemType.molotov:
                    database.SendData("Item Molotov", inventory.molotovAmount.ToString());
                    break;
                case Item.ItemType.smoke:
                    database.SendData("Item Smoke", inventory.smokeAmount.ToString());
                    break;
                case Item.ItemType.landmine:
                    database.SendData("Item Landmine", inventory.landmineAmount.ToString());
                    break;
                case Item.ItemType.chicken:
                    database.SendData("Item Chicken", inventory.chickenAmount.ToString());
                    break;
                case Item.ItemType.stone:
                    database.SendData("Item Stone", inventory.stoneAmount.ToString());
                    break;
                case Item.ItemType.wood:
                    database.SendData("Item Wood", inventory.woodAmount.ToString());
                    break;
                case Item.ItemType.wall:
                    database.SendData("Item WoodWall", inventory.wallAmount.ToString());
                    break;
                case Item.ItemType.metalWall:
                    database.SendData("Item MetalWall", inventory.metalWallAmount.ToString());
                    break;
                case Item.ItemType.woodDoor:
                    database.SendData("Item WoodDoor", inventory.woodDoorAmount.ToString());
                    break;
                case Item.ItemType.metalDoor:
                    database.SendData("Item MetalDoor", inventory.metalDoorAmount.ToString());
                    break;
                case Item.ItemType.kriss:
                    database.SendData("Weapon Kriss", inventory.krissAmount.ToString());
                    break;
                case Item.ItemType.mp7:
                    database.SendData("Weapon MP7", inventory.mp7Amount.ToString());
                    break;
                case Item.ItemType.mp5:
                    database.SendData("Weapon MP5", inventory.mp5Amount.ToString());
                    break;
                case Item.ItemType.ump45:
                    database.SendData("Weapon UMP", inventory.ump45Amount.ToString());
                    break;
                case Item.ItemType.tec9:
                    database.SendData("Weapon Tec9", inventory.tec9Amount.ToString());
                    break;
                case Item.ItemType.uzi:
                    database.SendData("Weapon UZI", inventory.uziAmount.ToString());
                    break;
                case Item.ItemType.ak12:
                    database.SendData("Weapon AK12", inventory.ak12Amount.ToString());
                    break;
                case Item.ItemType.ak74:
                    database.SendData("Weapon AK74", inventory.ak74Amount.ToString());
                    break;
                case Item.ItemType.g3a4:
                    database.SendData("Weapon G3A4", inventory.g3a4Amount.ToString());
                    break;
                case Item.ItemType.g36c:
                    database.SendData("Weapon G36C", inventory.g36cAmount.ToString());
                    break;
                case Item.ItemType.flamethrower:
                    database.SendData("Weapon Flamethrower", inventory.flamethrowerAmount.ToString());
                    break;
                case Item.ItemType.glock17:
                    database.SendData("Weapon Glock17", inventory.glock17Amount.ToString());
                    break;
            }
        }
    }
}
