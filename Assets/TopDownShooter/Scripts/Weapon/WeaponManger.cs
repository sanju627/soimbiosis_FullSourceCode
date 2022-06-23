using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using TMPro;
using UnityEngine.SceneManagement;
public class WeaponManger : MonoBehaviour
{
    //public AuthManager authManager;

    [Header("Stats")]
    public GameObject[] Melee;
    public GameObject[] Weapons;
    public GameObject[] Items;
    public ShopStation shop;
    [Space]
    public GameObject MeleeOBJ;
    public GameObject WeaponsOBJ;
    public GameObject ItemsOBJ;
    public GameObject BlueprintOBJ;
    [Space]
    public GameObject[] Bags;
    public AR_Rifle[] rifles;
    public bool isSwitching;
    [Space]
    public int weaponSelected = 1;
    public int meleeWeaponIndex;
    public int PrimaryWeaponIndex;
    public int SecondryWeaponIndex;
    public int itemIndex;
    [Space]
    public float totalAmmo = 300f;
    public float InventorySpace = 300f;
    [Space]
    public bool shopOpen;

    [Header("UI")]
    public UI_Inventory ui_Inventory;
    public Image[] slotImage;
    public Sprite selectedSprite;
    public Sprite defaultSprite;
    public Image primaryWeaponIcon;
    public Image secondryWeaponIcon;
    public Sprite defaultWeaponSprite;
    public TextMeshProUGUI primaryText;
    public TextMeshProUGUI secondryText;

    [Header("Shop")]
    public bool withinShop;
    public GameObject ShopPanel;
    public GameObject ShopBTN;
    public GameObject GameplayPanel;
    public float radius;
    public LayerMask shopLayer;
	
    [Header("AudioClips")]
	public AudioClip switchSFX;

	GameObject currentGun;
    GameObject currentBag;
	Animator anim;
	AudioSource audio;
    Image curentImage;
    Player player;

    // Start is called before the first frame update
    void Start()
    {	
    	anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        player = GetComponent<Player>();

        for(int i = 0; i < Weapons.Length; i++)
        {
            Weapons[i].SetActive(false);
        }

        SwitchThree();

        if(PrimaryWeaponIndex <= 0)
        {
            primaryWeaponIcon.sprite = defaultWeaponSprite;
            primaryText.text = "";
        }

        if (SecondryWeaponIndex <= 0)
        {
            secondryWeaponIcon.sprite = defaultWeaponSprite;
            secondryText.text = "";
        }

        //authManager = GameObject.FindGameObjectWithTag("Database").GetComponent<AuthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!player.inCar && !player.inHelicopter)
        {
            //Shop
            withinShop = Physics.CheckSphere(transform.position, radius, shopLayer);

            if(withinShop)
            {
                ShopBTN.SetActive(true);
                shop = FindObjectOfType<ShopStation>();
            }else
            {
                ShopBTN.SetActive(false);
            }
        }

        if(totalAmmo > InventorySpace)
        {
            totalAmmo = InventorySpace;
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            //authManager.LoadData();
            SceneManager.LoadScene("Junkyard");
            
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            //authManager.LoadData();
            SceneManager.LoadScene("Game");
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            //authManager.LoadData();
            SceneManager.LoadScene("MainCity");
        }
    }

    public void SwitchOne()
    {   
        if(!player.inCar && !player.inHelicopter)
        {

            if(weaponSelected != 1 && PrimaryWeaponIndex > 0)
            {
                SelectGun(PrimaryWeaponIndex);
                SelectIcon(0);
                anim.SetTrigger("switch");

                weaponSelected = 1;
            }
        }
    }

    public void SwitchTwo()
    {
        if(!player.inCar && !player.inHelicopter)
        {
            if(weaponSelected != 2 && SecondryWeaponIndex > 0)
            {
                SelectGun(SecondryWeaponIndex);
                SelectIcon(1);
                anim.SetTrigger("switch");

                weaponSelected = 2;
            }
        }
    }

    public void SwitchThree()
    {
        if (!player.inCar && !player.inHelicopter)
        {
            if (weaponSelected != 3)
            {
                SelectMelee(meleeWeaponIndex);
                SelectIcon(2);
                anim.SetTrigger("switch");

                weaponSelected = 3;
            }
        }
    }

    public void SwitchItem()
    {
        if(!player.inCar && !player.inHelicopter)
        {
            if(weaponSelected != 3)
            {
                SelectItem(itemIndex);
                SelectIcon(1);
                anim.SetTrigger("switch");

                weaponSelected = 3;
            }
        }
    }

    public void SelectMelee(int choice)
    {
        anim.SetBool("Reload", false);
        WeaponsOBJ.SetActive(false);
        MeleeOBJ.SetActive(true);
        ItemsOBJ.SetActive(false);
        BlueprintOBJ.SetActive(false);

        for (int i = 0; i < Melee.Length; i++)
        {
            Melee[i].SetActive(false);
        }

        Melee[choice].SetActive(true);
    }

    public void SelectGun(int choice)
    {
        anim.SetBool("Reload", false);
        WeaponsOBJ.SetActive(true);
        MeleeOBJ.SetActive(false);
        ItemsOBJ.SetActive(false);
        BlueprintOBJ.SetActive(false);

        for(int i = 0; i < Weapons.Length; i++)
    	{
    		Weapons[i].SetActive(false);
    	}

        for(int i = 0; i < rifles.Length; i++)
        {
            rifles[i].isReloading = false;
        }

    	Weapons[choice].SetActive(true);
    }

    public void SelectItem(int choice)
    {
        for(int i = 0; i < Items.Length; i++)
        {
            Items[i].SetActive(false);
        }

        if(weaponSelected != 3)
        {
            weaponSelected = 3;
        }

        isSwitching = false;

        anim.SetBool("Reload", false);
        WeaponsOBJ.SetActive(false);
        MeleeOBJ.SetActive(false);
        ItemsOBJ.SetActive(true);

        Items[choice].SetActive(true);
    }

    public void SelectBag(int choice)
    {
        if(currentBag != null)
        currentBag.SetActive(false);

        currentBag = Bags[choice];
        currentBag.SetActive(true);
    }

    void SelectIcon(int choice)
    {
        for (int i = 0; i < slotImage.Length; i++)
        {
            slotImage[i].sprite = defaultSprite;
        }

        slotImage[choice].sprite = selectedSprite;
    }

    public void Switching()
    {
        isSwitching = true;
    }

    public void Switched()
    {
        isSwitching = false;
    }

    public void EquipSFX()
    {
        audio.PlayOneShot(switchSFX);
    }

}
