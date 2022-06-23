using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class shopItem : MonoBehaviour
{
    public bool purchased;
    [Space]
	public Shop shop;
	public int Prize;
    [Space]
    public bool isVehicle;
    public bool isWeapon;
    public bool isSRV;
    public bool isDog;
    public bool isAkira;
    public bool isHusky;

    [Header("Item")]
    public Item item;
    public int vehicleIndex;
    public int dogsIndex;

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

    [Header("UI")]
    public TMP_Text PrizeText;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        Initialize();
    }

    public void itemClick()
    {
        if(!purchased)
        {
            shop.prize = Prize;

            if(isWeapon)
            {
                CancelBools();
                shop.isWeapon = true;
                shop.item = item;
            }

            if(isVehicle)
            {
                CancelBools();
                shop.isVehicle = true;

                //-------------------------Vehicle Types--------------------------//

                shop.vehicleIndex = vehicleIndex;

                if(C_1940)
                {
                    shop.C_1940 = true;
                }
                if(C_BubbleCar)
                {
                    shop.C_BubbleCar = true;
                }
                if (Hotrod)
                {
                    shop.Hotrod = true;
                }
                if (IceCreamTruck)
                {
                    shop.IceCreamTruck = true;
                }
                if (MiniVan)
                {
                    shop.MiniVan = true;
                }
                if (MonsterTruck)
                {
                    shop.MonsterTruck = true;
                }
                if (MuscleTruck)
                {
                    shop.MuscleTruck = true;
                }
                if (PickupTruck)
                {
                    shop.PickupTruck = true;
                }
                if (PoopTruck)
                {
                    shop.PoopTruck = true;
                }
                if (PorkTruck)
                {
                    shop.PorkTruck = true;
                }
                if (PrisonTruck)
                {
                    shop.PrisonTruck = true;
                }
                if (WaterTruck)
                {
                    shop.WaterTruck = true;
                }
                if (WienerTruck)
                {
                    shop.WienerTruck = true;
                }
                if(BlackHawk)
                {
                    shop.BlackHawk = true;
                }

            }

            if(isDog)
            {
                CancelBools();
                shop.isDog = true;
                shop.dogsIndex = dogsIndex;

                if (isHusky)
                {
                    shop.isHusky = true;
                }

                if (isAkira)
                {
                    shop.isAkira = true;
                }
            }
            

            if (isSRV)
            {
                CancelBools();
                shop.isSRV = true;
            }

        }
        else if (purchased)
        {
            if(isDog)
            {
                shop.dogsIndex = dogsIndex;
                shop.SpawnDog();
            }
            
            if(isVehicle)
            {
                shop.vehicleIndex = vehicleIndex;
                shop.Spawn();
            }
            
        }

    }

    void CancelBools()
    {
        shop.C_1940 = false;
        shop.C_BubbleCar = false;
        shop.Hotrod = false;
        shop.IceCreamTruck = false;
        shop.MiniVan = false;
        shop.MonsterTruck = false;
        shop.MuscleTruck = false;
        shop.PickupTruck = false;
        shop.PoopTruck = false;
        shop.PorkTruck = false;
        shop.PrisonTruck = false;
        shop.WaterTruck = false;
        shop.WienerTruck = false;
        shop.BlackHawk = false;
        shop.isWeapon = false;
        shop.isVehicle = false;
        shop.isSRV = false;
        shop.isDog = false;
        shop.isHusky = false;
        shop.isAkira = false;
    }

    void PurchasedItem()
    {
        PrizeText.text = "Click To Park";
        purchased = true;
    }

    void Initialize()
    {
        if(C_1940 && shop.C_1940_B)
        {
            PurchasedItem();
        }

        if (C_BubbleCar && shop.C_BubbleCar_B)
        {
            PurchasedItem();
        }

        if (Hotrod && shop.Hotrod_B)
        {
            PurchasedItem();
        }

        if (IceCreamTruck && shop.IceCreamTruck_B)
        {
            PurchasedItem();
        }

        if (MiniVan && shop.MiniVan_B)
        {
            PurchasedItem();
        }

        if(MonsterTruck && shop.MonsterTruck_B)
        {
            PurchasedItem();
        }

        if (MuscleTruck && shop.MuscleTruck_B)
        {
            PurchasedItem();
        }

        if (PickupTruck && shop.PickupTruck_B)
        {
            PurchasedItem();
        }

        if (PoopTruck && shop.PoopTruck_B)
        {
            PurchasedItem();
        }

        if (PorkTruck && shop.PorkTruck_B)
        {
            PurchasedItem();
        }

        if (PrisonTruck && shop.PrisonTruck_B)
        {
            PurchasedItem();
        }

        if (WaterTruck && shop.WaterTruck_B)
        {
            PurchasedItem();
        }

        if (WienerTruck && shop.WienerTruck_B)
        {
            PurchasedItem();
        }

        if (BlackHawk && shop.BlackHawk_B)
        {
            PurchasedItem();
        }

        if(isAkira && shop.isAkira_B)
        {
            purchasedDog();
        }
        
        if(isHusky && shop.isHusky_B)
        {
            purchasedDog();
        }
    }
    public void purchasedDog()
    {
        PrizeText.text = "Click To Equip";
        purchased = true;
    }

}
