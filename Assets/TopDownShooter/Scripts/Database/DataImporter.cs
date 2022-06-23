
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DataImporter : MonoBehaviour
{
    //public AuthManager authManager;
    EnergySystem energySystem;
    public Shop shop;
    public int woodAmount;
    public bool dataLoaded;
    [Space]
    public GameObject civils;

    [Header("Vehicles")]
    public GameObject[] Vehicles;
    public int vehicleIndex;

    [Header("Survival")]
    public GameObject survival;
    public TMP_Text ammoAmountTXT;

    HomeBase homeBase;
    ExploreManager expManager;
    PlayfabManager database;
    Player player;
    ItemAssets itemAssets;
    Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        energySystem = GetComponent<EnergySystem>();
        itemAssets = GetComponent<ItemAssets>();

        database = GameObject.FindGameObjectWithTag("Database").GetComponent<PlayfabManager>();
        //authManager.LoadData();
        Debug.Log("Loading All Data");
        StartCoroutine(LoadData());

        inventory = player.inventory;

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Game"))
        {
            homeBase = GameObject.FindGameObjectWithTag("HomeBase").GetComponent<HomeBase>();
        }
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Game"))
        {
            expManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ExploreManager>();
        }
    }

    IEnumerator LoadData()
    {
        database.GetData();

        yield return new WaitForSeconds(0.5f);

        GetData();
    }

    // Update is called once per frame
    void GetData()
    {
        energySystem.energy = database.maxEnergy;
        shop.money = database.coins;

        if(database.ammoAmount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.ammo, amount = database.ammoAmount });
        }

        if (database.medkitAmount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.medkit, amount = database.medkitAmount });
        }

        if (database.bandageAmount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.bandage, amount = database.bandageAmount });
        }

        if (database.grenadeAmount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.grenade, amount = database.grenadeAmount });
        }

        if (database.molotovAmount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.molotov, amount = database.molotovAmount });
        }

        if (database.smokeAmount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.smoke, amount = database.smokeAmount });
        }

        if (database.landmineAmount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.landmine, amount = database.landmineAmount });
        }

        if (database.chickenAmount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.chicken, amount = database.chickenAmount });
        }

        if (database.woodAmount > 0)
        {
            woodAmount = database.woodAmount;
            player.inventory.AddItem(new Item { itemType = Item.ItemType.wood, amount = database.woodAmount });
        }

        if (database.stoneAmount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.stone, amount = database.stoneAmount });
        }

        if (database.woodWallAmount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.wall, amount = database.woodWallAmount });
        }

        if (database.metalWallAmount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.metalWall, amount = database.metalWallAmount });
        }

        if (database.woodDoorAmount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.woodDoor, amount = database.woodDoorAmount });
        }

        if (database.metalDoorAmount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.metalDoor, amount = database.metalDoorAmount });
        }

        //-------------------------------------------Weapons------------------------------------//

        if (database.krissAmount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.kriss, amount = database.krissAmount });
        }

        if (database.MP7Amount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.mp7, amount = database.MP7Amount });
        }

        if (database.MP5Amount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.mp5, amount = database.MP5Amount });
        }

        if (database.UMPAmount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.ump45, amount = database.UMPAmount });
        }

        if (database.Tec9Amount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.tec9, amount = database.Tec9Amount });
        }

        if (database.UZIAmount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.uzi, amount = database.UZIAmount });
        }

        if (database.ak12Amount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.ak12, amount = database.ak12Amount });
        }

        if (database.ak74Amount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.ak74, amount = database.ak74Amount });
        }

        if (database.G3A4Amount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.g3a4, amount = database.G3A4Amount });
        }

        if (database.G36CAmount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.g36c, amount = database.G36CAmount });
        }

        if (database.flamethrowerAmount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.flamethrower, amount = database.flamethrowerAmount });
        }

        if (database.Glock17Amount > 0)
        {
            player.inventory.AddItem(new Item { itemType = Item.ItemType.glock17, amount = database.Glock17Amount });
        }

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Game"))
        {
            for (int i = 0; i < database.civilAmount; i++)
            {
                Transform tSpawn = homeBase.campFirePos[Random.Range(0, homeBase.campFirePos.Length)];

                Instantiate(civils, tSpawn.position, tSpawn.rotation);
            }

            for (int i = 0; i < database.srvAmount; i++)
            {
                Transform tSpawn = homeBase.srv_spawnPositions[Random.Range(0, homeBase.srv_spawnPositions.Length)];

                Instantiate(survival, tSpawn.position, tSpawn.rotation);
            }
        }

        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Game"))
        {
            for(int i = 0; i < database.srvAmount; i++)
            {
                Transform tSpawn = expManager.srv_spawnPositions[Random.Range(0, expManager.srv_spawnPositions.Length)];

                Instantiate(survival, tSpawn.position, tSpawn.rotation);
            }
        }

        dataLoaded = true;
    }

    public void SendInventoryData()
    {
        /*int.TryParse(result.Data["Item Ammo"].Value, out ammoAmount);
        int.TryParse(result.Data["Item Bandage"].Value, out bandageAmount);
        int.TryParse(result.Data["Item Medkit"].Value, out medkitAmount);
        int.TryParse(result.Data["Item Grenade"].Value, out grenadeAmount);
        int.TryParse(result.Data["Item Chicken"].Value, out chickenAmount);
        int.TryParse(result.Data["Item Landmine"].Value, out landmineAmount);
        int.TryParse(result.Data["Item MetalWall"].Value, out metalWallAmount);
        int.TryParse(result.Data["Item MetalDoor"].Value, out metalDoorAmount);
        int.TryParse(result.Data["Item WoodDoor"].Value, out woodDoorAmount);
        int.TryParse(result.Data["Item WoodWall"].Value, out woodWallAmount);
        int.TryParse(result.Data["Item Molotov"].Value, out molotovAmount);
        int.TryParse(result.Data["Item Smoke"].Value, out smokeAmount);
        int.TryParse(result.Data["Item Stone"].Value, out stoneAmount);
        int.TryParse(result.Data["Item Wood"].Value, out woodAmount);*/

        /*int.TryParse(result.Data["Weapon AK12"].Value, out ak12Amount);
        int.TryParse(result.Data["Weapon AK74"].Value, out ak74Amount);
        int.TryParse(result.Data["Weapon Kriss"].Value, out krissAmount);
        int.TryParse(result.Data["Weapon Flamethrower"].Value, out flamethrowerAmount);
        int.TryParse(result.Data["Weapon G36C"].Value, out G36CAmount);
        int.TryParse(result.Data["Weapon G3A4"].Value, out G3A4Amount);
        int.TryParse(result.Data["Weapon Glock17"].Value, out Glock17Amount);
        int.TryParse(result.Data["Weapon MP5"].Value, out MP5Amount);
        int.TryParse(result.Data["Weapon MP7"].Value, out MP7Amount);
        int.TryParse(result.Data["Weapon Tec9"].Value, out Tec9Amount);
        int.TryParse(result.Data["Weapon UMP"].Value, out UMPAmount);
        int.TryParse(result.Data["Weapon UZI"].Value, out UZIAmount);*/

        database.SendData("Item Ammo", inventory.ammoAmount.ToString());
        database.SendData("Item Bandage", inventory.bandageAmount.ToString());
        database.SendData("Item Medkit", inventory.medkitAmount.ToString());
        database.SendData("Item Grenade", inventory.grenadeAmount.ToString());
        database.SendData("Item Chicken", inventory.chickenAmount.ToString());
        database.SendData("Item Landmine", inventory.landmineAmount.ToString());
        database.SendData("Item MetalWall", inventory.metalWallAmount.ToString());
        database.SendData("Item MetalDoor", inventory.metalDoorAmount.ToString());
        database.SendData("Item WoodDoor", inventory.woodDoorAmount.ToString());
        database.SendData("Item WoodWall", inventory.wallAmount.ToString());
        database.SendData("Item Molotov", inventory.molotovAmount.ToString());
        database.SendData("Item Smoke", inventory.smokeAmount.ToString());
        database.SendData("Item Stone", inventory.stoneAmount.ToString());
        database.SendData("Item Wood", itemAssets.woodAmount.ToString());
        Debug.Log(inventory.woodAmount);

        database.SendData("Weapon AK12", inventory.ak12Amount.ToString());
        database.SendData("Weapon AK74", inventory.ak74Amount.ToString());
        database.SendData("Weapon Kriss", inventory.krissAmount.ToString());
        database.SendData("Weapon Flamethrower", inventory.flamethrowerAmount.ToString());
        database.SendData("Weapon G36C", inventory.g36cAmount.ToString());
        database.SendData("Weapon G3A4", inventory.g3a4Amount.ToString());
        database.SendData("Weapon Glock17", inventory.glock17Amount.ToString());
        database.SendData("Weapon MP5", inventory.mp5Amount.ToString());
        database.SendData("Weapon MP7", inventory.mp7Amount.ToString());
        database.SendData("Weapon Tec9", inventory.tec9Amount.ToString());
        database.SendData("Weapon UMP", inventory.ump45Amount.ToString());
        database.SendData("Weapon UZI", inventory.uziAmount.ToString());
    }
}
