using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_BTN : MonoBehaviour
{
    public Item item;
    public GameObject FunctionsOBJ;

    public bool functionOpen;
    public bool isWeapon;
    
    Player player;
    DataImporter dataImporter;
    WeaponManger weapon;
    bool switched;
    ItemAssets itemAssets;
    UI_Inventory uI_Inventory;
    SurvivalShop sShop;
    WeaponManger weaponManger;
    PlayfabManager database;
    Inventory inventory;

    void Start()
    {
    	player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        itemAssets = player.GetComponent<ItemAssets>();
        weapon = player.GetComponent<WeaponManger>();
        dataImporter = player.GetComponent<DataImporter>();
        inventory = player.inventory;

        database = GameObject.FindGameObjectWithTag("Database").GetComponent<PlayfabManager>();

        uI_Inventory = player.ui_Inventory;
        sShop = uI_Inventory.survivalShop;

    	functionOpen = false;
        FunctionsOBJ.SetActive(false);
        weaponManger = player.GetComponent<WeaponManger>();
    }


    public void SetItem(Item item)
    {
    	this.item = item;
    }

    public void ClickFunction()
    {
        switch (item.itemType)
        {
            case Item.ItemType.wall:
                weapon.SelectItem(0);
                break;
            case Item.ItemType.metalWall:
                weapon.SelectItem(1);
                break;
            case Item.ItemType.woodDoor:
                weapon.SelectItem(2);
                break;
            case Item.ItemType.metalDoor:
                weapon.SelectItem(3);
                break;
            case Item.ItemType.grenade:
                weapon.SelectItem(4);
                break;
            case Item.ItemType.smoke:
                weapon.SelectItem(5);
                break;
            case Item.ItemType.molotov:
                weapon.SelectItem(6);
                break;
            case Item.ItemType.medkit:
                if(player.currentHealth < player.maxHealth && !player.healing)
                {
                    player.HealOrder(50f, false);
                }
                break;
            case Item.ItemType.bandage:
                if (player.currentHealth < player.maxHealth && !player.healing)
                {
                    player.HealOrder(5f, true);
                }
                break;
            case Item.ItemType.glock17:
                weapon.SecondryWeaponIndex = 12;
                weapon.SwitchTwo();
                weapon.SelectGun(12);
                sShop.EnableWeapon(0);
                break;
            case Item.ItemType.kriss:
                weapon.PrimaryWeaponIndex = 1;
                weapon.SwitchOne();
                weapon.SelectGun(1);
                sShop.EnableWeapon(1);
                break;
            case Item.ItemType.mp7:
                weapon.PrimaryWeaponIndex = 2;
                weapon.SwitchOne();
                weapon.SelectGun(2);
                sShop.EnableWeapon(2);
                break;
            case Item.ItemType.mp5:
                weapon.PrimaryWeaponIndex = 3;
                weapon.SwitchOne();
                weapon.SelectGun(3);
                sShop.EnableWeapon(3);
                break;
            case Item.ItemType.tec9:
                weapon.PrimaryWeaponIndex = 4;
                weapon.SwitchOne();
                weapon.SelectGun(4);
                sShop.EnableWeapon(4);
                break;
            case Item.ItemType.ump45:
                weapon.PrimaryWeaponIndex = 5;
                weapon.SwitchOne();
                weapon.SelectGun(5);
                sShop.EnableWeapon(5);
                break;
            case Item.ItemType.uzi:
                weapon.PrimaryWeaponIndex = 6;
                weapon.SwitchOne();
                weapon.SelectGun(6);
                sShop.EnableWeapon(6);
                break;
            case Item.ItemType.ak12:
                weapon.PrimaryWeaponIndex = 7;
                weapon.SwitchOne();
                weapon.SelectGun(7);
                sShop.EnableWeapon(7);
                break;
            case Item.ItemType.ak74:
                weapon.PrimaryWeaponIndex = 8;
                weapon.SwitchOne();
                weapon.SelectGun(8);
                sShop.EnableWeapon(8);
                break;
            case Item.ItemType.g3a4:
                weapon.PrimaryWeaponIndex = 9;
                weapon.SwitchOne();
                weapon.SelectGun(9);
                sShop.EnableWeapon(9);
                break;
            case Item.ItemType.g36c:
                weapon.PrimaryWeaponIndex = 10;
                weapon.SwitchOne();
                weapon.SelectGun(10);
                sShop.EnableWeapon(10);
                break;
        }
    }

    public void ItemClick()
    {
        if(!functionOpen)
        {
            FunctionsOBJ.SetActive(true);
            functionOpen = true;
        }else
        {
            FunctionsOBJ.SetActive(false);
            functionOpen = false;
        }
    }

    public void Drop()
    {
        // if(isWeapon && player.weaponAmount <= 1)return;

        if (item.IsWeapon())
        {
            weaponManger.SwitchThree();
        }

        Item duplicateItem = new Item { itemType = item.itemType, amount = item.amount };
        inventory.RemoveItem(item);
        ItemWorld.DropItem(player.dropPosition, duplicateItem);

        switch (item.itemType)
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
