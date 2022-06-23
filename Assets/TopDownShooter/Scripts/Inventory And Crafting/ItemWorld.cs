using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemWorld : MonoBehaviour
{
	public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
	{
		Transform transform = Instantiate(ItemAssets.Instance.itemWorldPrefab, position, Quaternion.identity);

		ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
		itemWorld.SetItem(item);

		return itemWorld;
	}

	public static ItemWorld DropItem(Vector3 dropPos, Item item)
	{
		ItemWorld itemWorld = SpawnItemWorld(dropPos, item);
		//itemWorld.GetComponent<Rigidbody2D>().AddForce(dropPos, ForceMode2D.Impulse);
		return itemWorld;
	}

    [HideInInspector]
    public Item item;

    public GameObject[] items;
    
    GameObject currentItem;

    private SpriteRenderer spriteRenderer;
    //private TextMeshPro textMeshPro;

    void Awake()
    {
    	spriteRenderer = GetComponent<SpriteRenderer>();
    	//textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();

        for(int i = 0; i < items.Length; i++)
        {
            items[i].SetActive(false);
        }
    }

    public void SetItem(Item item)
    {
    	this.item = item;
    	
        switch (item.itemType)
        {
            case Item.ItemType.ammo:
                SwitchItem(0);
                break;
            case Item.ItemType.medkit:
                SwitchItem(1);
                break;
            case Item.ItemType.bandage:
                SwitchItem(2);
                break;
            case Item.ItemType.grenade:
                SwitchItem(3);
                break;
            case Item.ItemType.molotov:
                SwitchItem(4);
                break;
            case Item.ItemType.smoke:
                SwitchItem(5);
                break;
            case Item.ItemType.landmine:
                SwitchItem(6);
                break;
            case Item.ItemType.chicken:
                SwitchItem(7);
                break;
            case Item.ItemType.wood:
                SwitchItem(8);
                break;
            case Item.ItemType.stone:
                SwitchItem(9);
                break;
            //-------------------------------------------------------Weapon----------------------------------------//
            case Item.ItemType.wall:
                SwitchItem(10);
                break;
            case Item.ItemType.metalWall:
                SwitchItem(11);
                break;
            case Item.ItemType.woodDoor:
                SwitchItem(12);
                break;
            case Item.ItemType.metalDoor:
                SwitchItem(13);
                break;
            case Item.ItemType.kriss:
                SwitchItem(14);
                break;
            case Item.ItemType.mp7:
                SwitchItem(15);
                break;
            case Item.ItemType.mp5:
                SwitchItem(16);
                break;
            case Item.ItemType.ump45:
                SwitchItem(17);
                break;
            case Item.ItemType.tec9:
                SwitchItem(18);
                break;
            case Item.ItemType.uzi:
                SwitchItem(19);
                break;
            case Item.ItemType.ak12:
                SwitchItem(20);
                break;
            case Item.ItemType.ak74:
                SwitchItem(21);
                break;
            case Item.ItemType.g3a4:
                SwitchItem(22);
                break;
            case Item.ItemType.g36c:
                SwitchItem(23);
                break;
            case Item.ItemType.flamethrower:
                SwitchItem(24);
                break;
            case Item.ItemType.glock17:
                SwitchItem(25);
                break;
        }

    	if(item.amount > 1)
    	{
    		//textMeshPro.SetText(item.amount.ToString());
    	}else
    	{
    		//textMeshPro.SetText("");
    	}
    }

    public Item GetItem()
    {
    	return item;
    }

    public void DestroySelf()
    {
    	Destroy(gameObject);
    }

    public void SwitchItem(int Choice)
    {
        if(currentItem != null)
        {
            currentItem.SetActive(false);
        }

        for(int i = 0; i < items.Length; i++)
        {
            items[i].SetActive(false);
        }

        currentItem = items[Choice];
        currentItem.SetActive(true);
    }
}
