using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
	WeaponManger weaponManager;
    ItemAssets itemAssets;
    HomeBase homeBase;
    PlayfabManager database;
    Inventory inventory;

    public Player player;
	public int money;
    public int prize;
    //public AuthManager database;

    [Header("Item")]
    public Item item;
    public GameObject[] Vehicles;
    public int vehicleIndex;
    public GameObject SRV;
    [Space]
    public GameObject[] Dogs;
    public int dogsIndex;

    [Header("Items")]
    public bool isWeapon;
    public bool isVehicle;
    public bool isSRV;

    [Header("Dogs")]
    public bool isDog;
    public bool isAkira;
    public bool isHusky;
    public bool isAkira_B;
    public bool isHusky_B;

    [Header("Stats")]
	public Image[] weaponSlots;
	ShopStation shopSt;

    [Header("UI")]
    public Sprite defaultSprite;
    public Sprite selectedSprite;
    public TextMeshProUGUI moneyTXT;
	public TextMeshProUGUI moneyTXT_G;

    [Header("Vehicles Type")]
    public bool C_1940;
    public bool C_BubbleCar;
    public bool Hotrod;
    public bool IceCreamTruck;
    public bool MiniVan;
    public bool MonsterTruck;
    public bool MuscleTruck;
    public bool PickupTruck;
    public bool PoopTruck;
    public bool PorkTruck;
    public bool PrisonTruck;
    public bool WaterTruck;
    public bool WienerTruck;
    public bool BlackHawk;

    [Header("Vehicles Type")]
    public bool C_1940_B;
    public bool C_Blackhawk_B;
    public bool C_BubbleCar_B;
    public bool Hotrod_B;
    public bool IceCreamTruck_B;
    public bool MiniVan_B;
    public bool MonsterTruck_B;
    public bool MuscleTruck_B;
    public bool PickupTruck_B;
    public bool PoopTruck_B;
    public bool PorkTruck_B;
    public bool PrisonTruck_B;
    public bool WaterTruck_B;
    public bool WienerTruck_B;
    public bool BlackHawk_B;

    popTXT poptext;

    // Start is called before the first frame update
    void Start()
    {
        shopSt = GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopStation>();
        weaponManager = player.GetComponent<WeaponManger>();
        itemAssets = player.GetComponent<ItemAssets>();

        database = GameObject.FindGameObjectWithTag("Database").GetComponent<PlayfabManager>();

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Game"))
        {
            homeBase = GameObject.FindGameObjectWithTag("HomeBase").GetComponent<HomeBase>();
        }

        inventory = player.inventory;
        poptext = GameObject.FindGameObjectWithTag("MSG").GetComponent<popTXT>();

        CheckPurchasedItem();
    }

    // Update is called once per frame
    void Update()
    {
        moneyTXT.text = ": " + money.ToString("0");
        moneyTXT_G.text = ": " + money.ToString("0");
    }

    public void SelectWeapon(int choice)
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            weaponSlots[i].sprite = defaultSprite;
        }

        weaponSlots[choice].sprite = selectedSprite;
    }

    public void BuyBTN()
    {
        if (isWeapon || isVehicle || isSRV || isDog)
        {
            if (prize <= money)
            {
                if (isWeapon)
                {
                    player.inventory.AddItem(item);
                    CheckItem(item);
                }

                if(isVehicle)
                {
                    Spawn();

                    Initialze();
                }

                if(isSRV && database.srvAmount < 5)
                {
                    Debug.Log("Spawning");

                    Transform tSpawn = weaponManager.shop.srvSpawnPos[Random.Range(0, weaponManager.shop.srvSpawnPos.Length)];
                    Instantiate(SRV, tSpawn.position, tSpawn.rotation);

                    database.srvAmount += 1;
                    database.SendData("SRV Survival", database.srvAmount.ToString());
                }else if(isSRV && database.srvAmount > 5)
                {
                    poptext.PopText("Not Enough Slots");
                }

                if(isDog)
                {
                    SpawnDog();

                    Initialze();
                }

                money -= prize;
                database.SendData("Coins", money.ToString());
            }
        }
    }

    public void Spawn()
    {
        homeBase.DesVeh();

        homeBase.CurrentVeh = Instantiate(Vehicles[vehicleIndex], weaponManager.shop.spawnStations.position, weaponManager.shop.spawnStations.rotation);
        itemAssets.vehiclesIndex = vehicleIndex;

        //StartCoroutine(database.UpdateItem("Vehicle Index", vehicleIndex));
        database.SendData("Car VehicleIndex", vehicleIndex.ToString());
    }

    public void SpawnDog()
    {
        homeBase.DesDog();

        homeBase.CurrentDog = Instantiate(Dogs[dogsIndex], weaponManager.shop.dogSpawnPos.position, weaponManager.shop.dogSpawnPos.rotation);
        itemAssets.dogIndex = dogsIndex;

        //StartCoroutine(database.UpdateItem("Vehicle Index", vehicleIndex));
        database.SendData("Dog Index", dogsIndex.ToString());
    }


    void CheckItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.ammo:
                database.SendData("Item Ammo", item.amount.ToString());
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
                database.SendData("Item Kriss", inventory.krissAmount.ToString());
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

    void Initialze()
    {
        if(C_1940)
        {
            database.SendData("Car 1940", 1.ToString());
            C_1940_B = true;
        }

        if (C_BubbleCar)
        {
            //StartCoroutine(database.UpdateItem("Car Bubble", 1));
            database.SendData("Car Bubble", 1.ToString());
            C_BubbleCar = true;
        }
        
        if (Hotrod)
        {
            //StartCoroutine(database.UpdateItem("Car Hotrod", 1));
            database.SendData("Car Hotrod", 1.ToString());
            Hotrod_B = true;
        }
        
        if (IceCreamTruck)
        {
            //StartCoroutine(database.UpdateItem("Car IceCream", 1));
            database.SendData("Car IceCream", 1.ToString());
            IceCreamTruck_B = true;
        }
        
        if (MiniVan)
        {
            //StartCoroutine(database.UpdateItem("Car MiniVan", 1));
            database.SendData("Car MiniVan", 1.ToString());
            MiniVan_B = true;
        }
        
        if (MonsterTruck)
        {
            //StartCoroutine(database.UpdateItem("Car MonsterTruck", 1));
            database.SendData("Car MonsterTruck", 1.ToString());
            MonsterTruck_B = true;
        }
        
        if (MuscleTruck)
        {
            //StartCoroutine(database.UpdateItem("Car Muscle", 1));
            database.SendData("Car Muscle", 1.ToString());
            MuscleTruck_B = true;
        }
        
        if (PickupTruck)
        {
            //StartCoroutine(database.UpdateItem("Car Pickup", 1));
            database.SendData("Car Pickup", 1.ToString());
            PickupTruck_B = true;
        }
        
        if (PoopTruck)
        {
            //StartCoroutine(database.UpdateItem("Car Poop", 1));
            database.SendData("Car Poop", 1.ToString());
            PoopTruck_B = true;
        }
        
        if (PorkTruck)
        {
            //StartCoroutine(database.UpdateItem("Car Pork", 1));
            database.SendData("Car Pork", 1.ToString());
            PorkTruck_B = true;
        }
        
        if (PrisonTruck)
        {
            //StartCoroutine(database.UpdateItem("Car Prison", 1));
            database.SendData("Car Prison", 1.ToString());
            PrisonTruck_B = true;
        }
        
        if (WaterTruck)
        {
            //StartCoroutine(database.UpdateItem("Car Water", 1));
            database.SendData("Car Water", 1.ToString());
            WaterTruck_B = true;
        }
        
        if (WienerTruck)
        {
            //StartCoroutine(database.UpdateItem("Car Wiener", 1));
            database.SendData("Car Wiener", 1.ToString());
            WienerTruck_B = true;
        }

        if(BlackHawk)
        {
            //StartCoroutine(database.UpdateItem("Car BlackHawk", 1));
            database.SendData("Car BlackHawk", 1.ToString());
            BlackHawk_B = true;
        }

        if(isHusky)
        {
            database.SendData("Dog Husky", 1.ToString());
            isHusky_B = true;
        }
        
        if(isAkira)
        {
            database.SendData("Dog Akira", 1.ToString());
            isAkira_B = true;
        }
    }

    void CheckPurchasedItem()
    {
        if(database.car1940 > 0)
        {
            C_1940_B = true;
        }
        
        if(database.carBlackHawk > 0)
        {
            C_Blackhawk_B = true;
        }

        if (database.carBubble > 0)
        {
            C_BubbleCar_B = true;
        }

        if(database.carHotrod > 0)
        {
            Hotrod_B = true;
        }

        if(database.carIceCream > 0)
        {
            IceCreamTruck_B = true;
        }

        if(database.carMinivan > 0)
        {
            MiniVan_B = true;
        }

        if (database.carMonsterTruck > 0)
        {
            MonsterTruck_B = true;
        }

        if (database.carMuscle > 0)
        {
            MuscleTruck_B = true;
        }

        if (database.carPickup > 0)
        {
            PickupTruck_B = true;
        }

        if (database.carPoop > 0)
        {
            PoopTruck_B = true;
        }

        if (database.carPork > 0)
        {
            PorkTruck_B = true;
        }

        if (database.carPrison > 0)
        {
            PrisonTruck_B = true;
        }

        if (database.carWater > 0)
        {
            WaterTruck_B = true;
        }

        if (database.carWiener > 0)
        {
            WienerTruck_B = true;
        }

        if(database.akira > 0)
        {
            isAkira_B = true;
        }

        if(database.husky > 0)
        {
            isHusky_B = true;
        }
    }
}
