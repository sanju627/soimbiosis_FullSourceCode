using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item
{
	public enum ItemType
	{
		ammo,
		medkit,
		bandage,
		grenade,
		molotov,
		smoke,
		landmine,
		chicken,
		wood,
		stone,
		wall,
		metalWall,
		woodDoor,
		metalDoor,
		//-----------------------------------------------------Weapon--------------------------------------------------------///
		kriss,
		mp7,
		mp5,
		ump45,
		tec9,
		uzi,
		ak12,
		ak74,
		g3a4,
		g36c,
		flamethrower,
		glock17,
	}

	public ItemType itemType;
	public int amount;

	public Sprite GetSprite()
	{
		switch(itemType)
		{
			default:

			case ItemType.ammo: 		return ItemAssets.Instance.ammoSprite;
			case ItemType.bandage: 		return ItemAssets.Instance.bandageSprite;
			case ItemType.grenade: 		return ItemAssets.Instance.grenadeSprite;
			case ItemType.molotov: 		return ItemAssets.Instance.molotovSprite;
			case ItemType.smoke: 		return ItemAssets.Instance.smokeSprite;
			case ItemType.landmine: 	return ItemAssets.Instance.landmineSprite;
			case ItemType.medkit: 		return ItemAssets.Instance.medkitSprite;
			case ItemType.chicken: 		return ItemAssets.Instance.chickenSprite;
			case ItemType.wood: 		return ItemAssets.Instance.woodSprite;
			case ItemType.stone: 		return ItemAssets.Instance.stoneSprite;
			//-----------------------------------------------------------Weapon-----------------------------------------//
			case ItemType.kriss:		return ItemAssets.Instance.krissSprite;
			case ItemType.mp7:			return ItemAssets.Instance.mp7Sprite;
			case ItemType.mp5:			return ItemAssets.Instance.mp5Sprite;
			case ItemType.ump45:		return ItemAssets.Instance.umpSprite;
			case ItemType.tec9:			return ItemAssets.Instance.tec9Sprite;
			case ItemType.uzi:			return ItemAssets.Instance.uziSprite;
			case ItemType.ak12:			return ItemAssets.Instance.ak12Sprite;
			case ItemType.ak74:			return ItemAssets.Instance.ak74Sprite;
			case ItemType.g3a4:			return ItemAssets.Instance.g3a4Sprite;
			case ItemType.g36c:			return ItemAssets.Instance.g36cSprite;
			case ItemType.flamethrower: return ItemAssets.Instance.flamethrowerSprite;
			case ItemType.glock17:		return ItemAssets.Instance.glock17Sprite;
		}
	}

	public string GetName()
	{
		switch(itemType)
		{
			default:

			case ItemType.ammo: 		return "Ammo";
			case ItemType.bandage: 		return "Bandage";
			case ItemType.grenade: 		return "Grenade";
			case ItemType.molotov: 		return "Molotov";
			case ItemType.smoke: 		return "Smoke";
			case ItemType.landmine: 	return "Landmine";
			case ItemType.medkit: 		return "Medkit";
			case ItemType.chicken: 		return "Chicken";
			case ItemType.wood: 		return "Wood";
			case ItemType.stone: 		return "Stone";
			//-----------------------------------------------------------Weapon-----------------------------------------//
			case ItemType.kriss:		return "Kriss";
			case ItemType.mp7:			return "MP7";
			case ItemType.mp5:			return "MP5";
			case ItemType.ump45:		return "UMP";
			case ItemType.tec9:			return "Tec-9";
			case ItemType.uzi:			return "UZI";
			case ItemType.ak12:			return "AK12";
			case ItemType.ak74:			return "AK74";
			case ItemType.g3a4:			return "G3A4";
			case ItemType.g36c:			return "G36C";
			case ItemType.flamethrower: return "Flamethrower";
			case ItemType.glock17:		return "Glock 17";
		}

	}

	public bool IsStackable()
	{
		switch(itemType)
		{
			default:
			case ItemType.bandage:
			case ItemType.grenade:
			case ItemType.molotov:
			case ItemType.smoke:
			case ItemType.landmine:
			case ItemType.ammo:
			case ItemType.medkit:
			case ItemType.chicken:
			case ItemType.wood:
			case ItemType.stone:
			case ItemType.wall:
			case ItemType.metalWall:
			case ItemType.woodDoor:
			case ItemType.metalDoor:
				//------------------------------------------Weapon-------------------------------------------//
				return true;
			case ItemType.kriss: 
			case ItemType.mp7: 
			case ItemType.mp5: 
			case ItemType.ump45: 
			case ItemType.tec9: 
			case ItemType.uzi: 
			case ItemType.ak12:
			case ItemType.ak74:
			case ItemType.g3a4:
			case ItemType.g36c:
			case ItemType.flamethrower:
			case ItemType.glock17:
				return false;

		}
	}

	public bool IsWeapon()
	{
		switch(itemType)
		{
			default:
			case ItemType.bandage:
			case ItemType.grenade:
			case ItemType.molotov:
			case ItemType.smoke:
			case ItemType.landmine:
			case ItemType.ammo:
			case ItemType.medkit:
			case ItemType.chicken:
			case ItemType.wood:
			case ItemType.stone:
			case ItemType.wall:
			case ItemType.metalWall:
			case ItemType.woodDoor:
			case ItemType.metalDoor:
			return false;
			//------------------------------------------Weapon-------------------------------------------//
			case ItemType.kriss:
			case ItemType.mp7:
			case ItemType.mp5:
			case ItemType.ump45:
			case ItemType.tec9:
			case ItemType.uzi:
			case ItemType.ak12:
			case ItemType.ak74:
			case ItemType.g3a4:
			case ItemType.g36c:
			case ItemType.flamethrower:
			case ItemType.glock17:
				return true;

		}
	}

}
