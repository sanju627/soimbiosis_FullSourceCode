using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory
{
	public event EventHandler OnItemListChanged;

    public List<Item> itemList;
    private Action<Item> useItemAction;

    public int ammoAmount;
    public int medkitAmount;
    public int bandageAmount;
    public int grenadeAmount;
    public int molotovAmount;
    public int smokeAmount;
    public int landmineAmount;
    public int chickenAmount;
    public int woodAmount;
    public int stoneAmount;
    public int wallAmount;
    public int metalWallAmount;
    public int woodDoorAmount;
    public int metalDoorAmount;
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

    public Item[] items;

    public Inventory(Action<Item> useItemAction)
    {
    	this.useItemAction = useItemAction;
    	itemList = new List<Item>();

    	// AddItem(new Item {itemType = Item.ItemType.stone, amount = 1});
    	// AddItem(new Item {itemType = Item.ItemType.wood, amount = 1});
    	// AddItem(new Item {itemType = Item.ItemType.apple, amount = 1});
    	//Debug.Log(itemList.Count);
    }

    public void AddItem(Item item)
    {
    	if(item.IsStackable())
    	{
    		bool itemAlreadtInInventory = false;
    		foreach(Item inventoryItem in itemList)
    		{
    			if(inventoryItem.itemType == item.itemType)
    			{
    				inventoryItem.amount += item.amount;
    				itemAlreadtInInventory = true;
    			}
    		}

    		if(!itemAlreadtInInventory)
    		{
    			itemList.Add(item);
    		}
    	}else
    	{
    		itemList.Add(item);
    	}


        switch (item.itemType)
        {
            case Item.ItemType.ammo:
                ammoAmount += item.amount;
                break;
            case Item.ItemType.medkit:
                medkitAmount += item.amount;
                break;
            case Item.ItemType.bandage:
                bandageAmount += item.amount;
                break;
            case Item.ItemType.grenade:
                grenadeAmount += item.amount;
                break;
            case Item.ItemType.molotov:
                molotovAmount += item.amount;
                break;
            case Item.ItemType.smoke:
                smokeAmount += item.amount;
                break;
            case Item.ItemType.landmine:
                landmineAmount += item.amount;
                break;
            case Item.ItemType.chicken:
                chickenAmount += item.amount;
                break;
            case Item.ItemType.stone:
                stoneAmount += item.amount;
                break;
            case Item.ItemType.wood:
                woodAmount += item.amount;
                break;
            case Item.ItemType.wall:
                wallAmount += item.amount;
                break;
            case Item.ItemType.metalWall:
                metalWallAmount += item.amount;
                break;
            case Item.ItemType.woodDoor:
                woodDoorAmount += item.amount;
                break;
            case Item.ItemType.metalDoor:
                metalDoorAmount += item.amount;
                break;
            case Item.ItemType.kriss:
                krissAmount += item.amount;
                break;
            case Item.ItemType.mp7:
                mp7Amount += item.amount;
                break;
            case Item.ItemType.mp5:
                mp5Amount += item.amount;
                break;
            case Item.ItemType.ump45:
                ump45Amount += item.amount;
                break;
            case Item.ItemType.tec9:
                tec9Amount += item.amount;
                break;
            case Item.ItemType.uzi:
                uziAmount += item.amount;
                break;
            case Item.ItemType.ak12:
                ak12Amount += item.amount;
                break;
            case Item.ItemType.ak74:
                ak74Amount += item.amount;
                break;
            case Item.ItemType.g3a4:
                g3a4Amount += item.amount;
                break;
            case Item.ItemType.g36c:
                g36cAmount += item.amount;
                break;
            case Item.ItemType.flamethrower:
                flamethrowerAmount += item.amount;
                break;
            case Item.ItemType.glock17:
                glock17Amount += item.amount;
                break;
        }

    	OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.ammo:
                ammoAmount -= item.amount;
                break;
            case Item.ItemType.medkit:
                medkitAmount -= item.amount;
                break;
            case Item.ItemType.bandage:
                bandageAmount -= item.amount;
                break;
            case Item.ItemType.grenade:
                grenadeAmount -= item.amount;
                break;
            case Item.ItemType.molotov:
                molotovAmount -= item.amount;
                break;
            case Item.ItemType.smoke:
                smokeAmount -= item.amount;
                break;
            case Item.ItemType.landmine:
                landmineAmount -= item.amount;
                break;
            case Item.ItemType.chicken:
                chickenAmount -= item.amount;
                break;
            case Item.ItemType.stone:
                stoneAmount -= item.amount;
                break;
            case Item.ItemType.wood:
                woodAmount -= item.amount;
                break;
            case Item.ItemType.wall:
                wallAmount -= item.amount;
                break;
            case Item.ItemType.metalWall:
                metalWallAmount -= item.amount;
                break;
            case Item.ItemType.woodDoor:
                woodDoorAmount -= item.amount;
                break;
            case Item.ItemType.metalDoor:
                metalDoorAmount -= item.amount;
                break;
            case Item.ItemType.kriss:
                krissAmount -= item.amount;
                break;
            case Item.ItemType.mp7:
                mp7Amount -= item.amount;
                break;
            case Item.ItemType.mp5:
                mp5Amount -= item.amount;
                break;
            case Item.ItemType.ump45:
                ump45Amount -= item.amount;
                break;
            case Item.ItemType.tec9:
                tec9Amount -= item.amount;
                break;
            case Item.ItemType.uzi:
                uziAmount -= item.amount;
                break;
            case Item.ItemType.ak12:
                ak12Amount -= item.amount;
                break;
            case Item.ItemType.ak74:
                ak74Amount -= item.amount;
                break;
            case Item.ItemType.g3a4:
                g3a4Amount -= item.amount;
                break;
            case Item.ItemType.g36c:
                g36cAmount -= item.amount;
                break;
            case Item.ItemType.flamethrower:
                flamethrowerAmount -= item.amount;
                break;
            case Item.ItemType.glock17:
                glock17Amount -= item.amount;
                break;

        }

        if (item.IsStackable())
    	{
    		Item itemInInventory = null;
    		foreach(Item inventoryItem in itemList)
    		{
    			if(inventoryItem.itemType == item.itemType)
    			{
    				inventoryItem.amount -= item.amount;
    				itemInInventory = inventoryItem;
    			}
    		}

    		if(itemInInventory != null && itemInInventory.amount <= 0)
    		{
    			itemList.Remove(itemInInventory);
    		}
    	}else
    	{
            

            itemList.Remove(item);
    	}


        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UseItem(Item item)
    {
    	useItemAction(item);
    }

    public List<Item> GetItemList()
    {
    	return itemList;
    }
}
