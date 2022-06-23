using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityStandardAssets.Vehicles.Car;
using UnityStandardAssets.CrossPlatformInput;
using Michsky.UI.ModernUIPack;

public class Player : MonoBehaviour
{
    [Header("Reference")]
    //public AuthManager authManager;
    [Space]
	public float speed;
	public float Gravity;
	public bool isMoving;
	public bool isGrounded;
	public float groundRadius;
	public float turnSpeed;
    public float vehicleCheckRadius;
    public CarController carController;
    public HelicopterController heliController;
    public bool withinCar;
    public bool withinHelicopter;
    public LayerMask carLayer;
    public LayerMask heliLayer;
    public LayerMask groundLayer;
    public GameObject camera;
    public bool inCar;
    public bool inHelicopter;
    public bool Dead;
    public UI_Manager ui_Manager;
    [Space]
    public float sprintSpeed;
    public bool isRunning;
    public bool disableS;
    public float sprintValue;
    public float sprintFillSpeed;
    public float sprintUseSpeed;
    public float sprintMaxValue;
    [Space]
    public float energyValue;
    public float energyFillSpeed;
    public float energyMaxValue;
    public Vector3 dropPosition;
    public Vector3 offset;
    [Space]
    public int currentSurvivars;
    public int maxSurvivars;

    public Inventory inventory;

    [Header("Health")]
    public float currentHealth;
    public float maxHealth;
    public ParticleSystem bloodVFX;
    public float healSpeed;
    public float healDuration;
    public ParticleSystem bleedVFX;

    [Header("Skill")]
    public float skillValue;
    public float filledSpeed;
    public Slider skillSlider;
    public bool pilot;
    public bool soldier;
    public bool mechanic;
    public bool medic;

    [Header("Skill Attributes")]
    public Transform[] spawnPosition;
    public GameObject ammoBox;
    public float radius;
    [Space]
    public float runSpeed;
    public float runDuration;
    [Space]
    public float healRadius;
    public float healAmount;
    public bool healing;
    public bool bleed;
    public LayerMask playerLayer;
    public ParticleSystem healVFX;
    [Space]
    public float vehicleHealAmount;

    [Header("Items")]
    public Item[] items;
    public bool Smoke;
    public bool Grenade;
    public bool LandMine;
    public bool Medkit;
    public bool Molo;
    public bool woodWall;
    public bool metalWall;
    public bool woodDoor;
    public bool metalDoor;
    public ThrowableItem woodWallItem;
    public ThrowableItem metalWallItem;
    public ThrowableItem woodDoorItem;
    public ThrowableItem metalDoorItem;
    public Transform throwPosition;
    public Transform BlueprintPos;
    public float force;
    public GameObject SmokeGrenadeOBJ;
    public GameObject FragGrenadeOBJ;
    public GameObject landMineOBJ;
    public GameObject medkitOBJ;
    public GameObject MolotovOBJ;
    public GameObject woodWallOBJ;
    public int nadeCount;
    public bool haveDog;

    [Header("UI")]
    public UI_Inventory ui_Inventory;
    public FixedJoystick joystick;
    public Slider healthSlider;
    public ProgressBar sprintSlider;
    public ProgressBar healingSlider;
    public TextMeshProUGUI nadeCountTXT;
    public Image nadeImage;
    public Sprite[] grenadesSprite;
    public GameObject ArmoryPanel;
    public GameObject VehiclePanel;
    public TextMeshProUGUI speedTXT;
    public Image v_Icon;
    public Slider vehSlider;
    public GameObject CarEnterBTN;
    public GameObject HeliEnterBTN;
    public GameObject healingPanel;
    public FixedTouchBTN Car_BTN;
    public FixedTouchBTN Heli_BTN;

    [Header("AudioClips")]
    public AudioClip[] footStepSFX;
    public AudioClip[] bloodSFX;
    public AudioClip[] pickSFX;

	CharacterController controller;
	Vector3 velocity;
	Vector3 shellPos;
	AudioSource audio;
	float defaultSpeed;
	Animator anim;
	float angle;
	Camera cam;
	WeaponManger weaponManager;
	float dr;
    DataImporter dataImporter;

    PlayfabManager database;

    private void Awake()
    {
        inventory = new Inventory(UseItem);
    }


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        audio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        weaponManager = GetComponent<WeaponManger>();
        dataImporter = GetComponent<DataImporter>();

        //authManager = GameObject.FindGameObjectWithTag("Database").GetComponent<AuthManager>();
        //authManager.LoadData();

        database = GameObject.FindGameObjectWithTag("Database").GetComponent<PlayfabManager>();

        //---------------------------------------------------------Inventory & Crafting------------------------------------------------//
        ui_Inventory.SetPlayer(this);
        ui_Inventory.SetInventory(inventory);


        for(int i = 0; i < items.Length; i++)
        {
            inventory.AddItem(items[i]);
        }

        //UI
        skillValue = 0;
        skillSlider.value = skillValue;

        currentHealth = maxHealth;

        //currentHealth = authManager.health;
        healthSlider.value = currentHealth;

        //Instantiate(CameraObj, transform.position, Quaternion.identity);
        cam = Camera.main;
        defaultSpeed = speed;

        sprintValue = sprintMaxValue;
        sprintSlider.currentPercent = sprintValue;

        //StartCoroutine(authManager.UpdateItem("Vehicle Index", 0));
    }

    // Update is called once per frame
    void Update()
    {
        if(Dead)return;

        dropPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z) + offset;

        if(currentHealth > maxHealth)
    	{
    		currentHealth = maxHealth;
    	}

    	isGrounded = Physics.CheckSphere(transform.position, groundRadius, groundLayer);

        if(carController == null || heliController == null)
        {
            if(isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            if(isGrounded)
            {
                anim.SetBool("Fall", false);
            }else
            {
                anim.SetBool("Fall", true);
            }

            float x = joystick.Horizontal;
            float z = joystick.Vertical;

            Vector3 moveDir = new Vector3(x, 0, z);
            velocity.y += Gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
            

            if(isRunning)
            {
            	controller.Move(moveDir * sprintSpeed * Time.deltaTime);
            }else
            {
            	controller.Move(moveDir * speed * Time.deltaTime);
            }

            if(joystick.Horizontal != 0f || joystick.Vertical != 0f)
            {
            	Vector3 lookDir = new Vector3 (x, 0f, z);
        		transform.rotation = Quaternion.LookRotation(lookDir);

            	if(isRunning)
            	{
            		anim.SetFloat("InputY", 1f);
            	}else
            	{
            		anim.SetFloat("InputY", 0.5f);
            	}
            	isMoving = true;
            	
            }else
            {
            	anim.SetFloat("InputY", 0f);
            	isMoving = false;
            }

            if(CrossPlatformInputManager.GetButton("Sprint") && isMoving && sprintValue > 0f && !disableS)
            {
            	isRunning = true;
            	sprintValue -= sprintUseSpeed * Time.deltaTime;
            }else
            {
            	isRunning = false;
            	sprintValue += sprintFillSpeed * Time.deltaTime;
            }

            if(sprintValue <= 5f)
            {
            	StartCoroutine(DisableSprint());
            }

            if(sprintValue > sprintMaxValue)
            {
            	sprintValue = sprintMaxValue;
            }

            if(sprintValue <= 0)
            {
            	sprintValue = 0f;
            }

            sprintSlider.currentPercent = sprintValue;
            
            if(inCar)
            {
            	if(carController.destroyed)
	            {	
	                camera.SetActive(true);
	            }
            }

            //Skill
            if(skillValue <= 100)
            {
                skillValue += Time.deltaTime * filledSpeed;
                skillSlider.value = skillValue;
            }

            if(skillValue >= 100)
            {
                Skill();
                skillValue = 0;
            }

            //Active Skill
            if (medic)
            {
                if(healing)
                {
                    Collider[] col = Physics.OverlapSphere(transform.position, healRadius, playerLayer);

                    foreach(Collider player in col)
                    {
                        //player.GetComponent<Player>().heal(healAmount);
                    }

                    healVFX.Play();
                }else
                {
                    healVFX.Stop();
                }
            }

            //Rotate();
            

            //Grenade
            //Smoke

            //GrenadeIcons();
        }
        
        CarUpdate();

        withinCar = Physics.CheckSphere(transform.position, vehicleCheckRadius, carLayer);

        if(withinCar)
        {
        	CarEnterBTN.SetActive(true);
        }else
        {
        	CarEnterBTN.SetActive(false);
        }
        

        if(pilot)
        {
            withinHelicopter = Physics.CheckSphere(transform.position, vehicleCheckRadius, heliLayer);

            if(withinHelicopter)
            {
                HeliUpdate();
            }

            if(withinHelicopter)
	        {
	        	HeliEnterBTN.SetActive(true);
	        }else
	        {
	        	HeliEnterBTN.SetActive(false);
	        }
        }

        if(inCar || inHelicopter)
        {
            anim.SetBool("Drive", true);
            anim.SetLayerWeight(anim.GetLayerIndex("Rifle"), 0);
            anim.SetLayerWeight(anim.GetLayerIndex("Pistol"), 0);
            weaponManager.SelectGun(0);
            ArmoryPanel.SetActive(false);
            VehiclePanel.SetActive(true);

            //UI
            if(inCar)
            {
                v_Icon.sprite = carController.Icon;
                speedTXT.text = carController.speed.ToString("0") + " mph";
                vehSlider.maxValue = carController.maxHealth;
                vehSlider.value = carController.currentHealth;

                if(carController.destroyed)
                {	
                	camera.SetActive(true);
                    carController.followCamera.SetActive(false);
                    weaponManager.SelectGun(weaponManager.PrimaryWeaponIndex);
                    EjectCar();
                }
            }

            if(inHelicopter)
            {
                v_Icon.sprite = heliController.Icon;
                speedTXT.text = "H : " + heliController.hMove.x.ToString("0") + " " + "V :" + heliController.hMove.y.ToString("0");

                vehSlider.maxValue = heliController.maxHealth;
                vehSlider.value = heliController.currentHealth;

                if(heliController.destroyed)
                {
                    EjectHel();
                }
            }
        }else
        {
            anim.SetBool("Drive", false);
            ArmoryPanel.SetActive(true);
            VehiclePanel.SetActive(false);
        }


    }

    void OnTriggerEnter(Collider col)
    {
        ItemWorld itemWorld = col.GetComponent<ItemWorld>();
        Crate crate = col.GetComponent<Crate>();

        if(crate != null)
        {
            crate.Open();
        }

        if(itemWorld != null)
        {
            audio.PlayOneShot(pickSFX[Random.Range(0, pickSFX.Length)]);

            inventory.AddItem(itemWorld.GetItem());

            switch (itemWorld.GetItem().itemType)
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
            itemWorld.DestroySelf();
        }
    }

    void UseItem(Item item)
    {
        
    }

    IEnumerator DisableSprint()
    {
    	disableS = true;

    	yield return new WaitForSeconds(5f);

    	disableS = false;
    }

    void CarUpdate()
    {
        if(carController == null)
        {
            //Check if Player Won't Car
            if(Car_BTN.Pressed)
            {
            	Car_BTN.Pressed = false;
                Collider[] carCol = Physics.OverlapSphere(transform.position, vehicleCheckRadius, carLayer);

                DisableSkin();

                foreach (Collider collider in carCol)
                {
                    carController = collider.GetComponent<CarController>();
                    carController.Drive();


                    inCar = true;
                }
            }

        }else
        {
            //Drive Car
            carController.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetAxis("Vertical"), 0);
            transform.position = carController.seat.position;
            transform.rotation = carController.seat.rotation;
            carController.Canvas.SetActive(true);
            carController.followCamera.SetActive(true);
            camera.SetActive(false);

            if(Car_BTN.Pressed)
            {	
            	Car_BTN.Pressed = false;
                carController.Eject();
                EjectCar();
                EnableSkin();
            }
        }
    }

    public void DisableSkin()
    {
        SkinnedMeshRenderer[] skin = GetComponentsInChildren<SkinnedMeshRenderer>();
        MeshRenderer[] mesh = GetComponentsInChildren<MeshRenderer>();

        foreach (SkinnedMeshRenderer c in skin)
        {
            c.enabled = false;
        }

        foreach (MeshRenderer m in mesh)
        {
            m.enabled = false;
        }
    }

    public void EnableSkin()
    {
        SkinnedMeshRenderer[] skin = GetComponentsInChildren<SkinnedMeshRenderer>();
        MeshRenderer[] mesh = GetComponentsInChildren<MeshRenderer>();

        foreach (SkinnedMeshRenderer c in skin)
        {
            c.enabled = true;
        }

        foreach (MeshRenderer m in mesh)
        {
            m.enabled = true;
        }
    }

    void EjectCar()
    {
        transform.position = carController.outPos.position;
        transform.rotation = carController.outPos.rotation;
        carController.followCamera.SetActive(false);
        weaponManager.SelectMelee(weaponManager.meleeWeaponIndex);
        carController.Move(0, 0, 0, 0);



        camera.SetActive(true);
        carController.Canvas.SetActive(false);
        inCar = false;
        carController = null;
    }

    void HeliUpdate()
    {
        //Check if Player Won't Car
        if(heliController == null)
        {
            if(Heli_BTN.Pressed)
            {
            	Heli_BTN.Pressed = false;
                Collider[] carCol = Physics.OverlapSphere(transform.position, vehicleCheckRadius, heliLayer);

                foreach(Collider collider in carCol)
                {
                    collider.GetComponent<ControlPanel>().enabled = true;
                    heliController = collider.GetComponent<HelicopterController>();
                    inHelicopter = true;
                }
            }
        }
        else
        {
            //Drive Car
            transform.position = heliController.seat.position;
            transform.rotation = heliController.seat.rotation;
            heliController.followCamera.SetActive(true);
            heliController.Canvas.SetActive(true);
            camera.SetActive(false);

            if(Heli_BTN.Pressed)
            {
            	Heli_BTN.Pressed = false;
                EjectHel();
            }
        }
    }

    void EjectHel()
    {
        transform.position = heliController.outPos.position;
        transform.rotation = heliController.outPos.rotation;
        weaponManager.SelectGun(weaponManager.PrimaryWeaponIndex);
        inHelicopter = false;

        heliController.followCamera.SetActive(false);
        heliController.Canvas.SetActive(false);
        camera.SetActive(true);
        heliController.GetComponent<ControlPanel>().enabled = false;
        heliController = null;
    }

    void Rotate()
    {
    	if(!inCar || !inHelicopter)
    	{
    		float hoz = joystick.Horizontal;
	    	float ver = joystick.Vertical;

	    	Vector2 convertedXY = ConvertWithCamera(Camera.main.transform.position, hoz, ver);
	    	Vector3 direction = new Vector3(convertedXY.x, 0, convertedXY.y).normalized;
	    	Vector3 lookPos = transform.position + direction;

	    	lookPos.y = transform.position.y;
	    	transform.LookAt(lookPos);
    	}    	
    }

    Vector2 ConvertWithCamera(Vector3 cameraPos, float hor, float ver)
    {
    	Vector2 joyDir = new Vector2(hor, ver).normalized;
    	Vector2 camPos = new Vector2(cameraPos.x, cameraPos.z);
    	Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);
    	Vector2 cameraToPlayerDir = (Vector2.zero - camPos).normalized;
    	float angle = Vector2.SignedAngle(cameraToPlayerDir, new Vector2(0, 1));
    	Vector2 finalDir = RotateVector(joyDir, -angle);
    	return finalDir;
    }

    Vector2 RotateVector(Vector2 v, float angle)
    {
    	float radian = angle * Mathf.Deg2Rad;
    	float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
    	float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
    	return new Vector2(_x, _y);
    }

    public void Foots()
    {
        audio.PlayOneShot(footStepSFX[Random.Range(0, footStepSFX.Length)]);
    }

    public void Skill()
    {
    	if(pilot)
    	{
	    	for(int i = 0; i < spawnPosition.Length; i++)
	    	{
	    		Instantiate(ammoBox, spawnPosition[i].position, Quaternion.identity);
	    	}	
    	}

    	if(soldier)	
    	{
    		StartCoroutine(AllowRun());
    	}

    	if(medic)
    	{
    		StartCoroutine(heal(50f, false));
    	}

    	if(mechanic)
    	{
    		if(inCar)
    		{
    			carController.Heal(vehicleHealAmount);
    		}
    	}
    }

    public void TakeDamage(float amount)
    {
        if(Dead)return;
        if(inCar || inHelicopter)return;

        currentHealth -= amount;
        audio.PlayOneShot(bloodSFX[Random.Range(0, bloodSFX.Length)]);
        bloodVFX.Play();

        healthSlider.value = currentHealth;

        if(currentHealth <= 40)
        {
            bleed = true;
            bleedVFX.Play();
        }

        if(currentHealth <= 0)
        {
            Debug.Log("Die");
            weaponManager.SelectGun(0);
            anim.SetTrigger("Dead");
            anim.SetLayerWeight(anim.GetLayerIndex("Rifle"), 0);
            anim.SetLayerWeight(anim.GetLayerIndex("Pistol"), 0);
            ui_Inventory.CloseShop();
            ui_Manager.Dead();
            GetComponent<ItemAssets>().RemoveItems();
            Dead = true;
        }
    }

    public void Throw()
    {
    	if(Smoke)
    	{
    		GameObject grenade = Instantiate(SmokeGrenadeOBJ, throwPosition.position, throwPosition.rotation);
    		Rigidbody rb = grenade.GetComponent<Rigidbody>();
    		rb.AddForce(throwPosition.forward * force);

            inventory.RemoveItem(new Item { itemType = Item.ItemType.smoke, amount = 1 });
        }

    	if(Grenade)
    	{
    		GameObject grenade = Instantiate(FragGrenadeOBJ, throwPosition.position, throwPosition.rotation);
    		Rigidbody rb = grenade.GetComponent<Rigidbody>();
    		rb.AddForce(throwPosition.forward * force);

            inventory.RemoveItem(new Item { itemType = Item.ItemType.grenade, amount = 1 });
        }

    	if(LandMine)
    	{
    		GameObject grenade = Instantiate(landMineOBJ, transform.position, transform.rotation);
    	}

    	if(Medkit)
    	{
    		GameObject grenade = Instantiate(medkitOBJ, throwPosition.position, throwPosition.rotation);
    		Rigidbody rb = grenade.GetComponent<Rigidbody>();
    		rb.AddForce(throwPosition.forward * force);
    	}

        if(Molo)
        {
            GameObject grenade = Instantiate(MolotovOBJ, throwPosition.position, throwPosition.rotation);
            Rigidbody rb = grenade.GetComponent<Rigidbody>();
            rb.AddForce(throwPosition.forward * force);

            inventory.RemoveItem(new Item { itemType = Item.ItemType.molotov, amount = 1 });
        }

        if(woodWall)
        {
            woodWallItem.Throw();
        }

        if(metalWall)
        {
            metalWallItem.Throw();
        }

        if(woodDoor)
        {
            woodDoorItem.Throw();
        }

        if(metalDoor)
        {
            metalDoorItem.Throw();
        }
    }

    IEnumerator AllowRun()
    {
    	speed = runSpeed;

    	yield return new WaitForSeconds(runDuration);

    	speed = defaultSpeed;
    }

    public void HealOrder(float amount, bool bandage)
    {
        StartCoroutine(heal(amount, bandage));
    }

    public IEnumerator heal(float amount, bool bandage)
    {
    	healing = true;

        healingPanel.SetActive(true);

        if(bandage)
        {
            bleed = false;
            bleedVFX.Stop();
        }

        yield return new WaitForSeconds(healDuration);

        currentHealth += amount;

        healthSlider.value = currentHealth;

        healingSlider.currentPercent = 0f;

        healingPanel.SetActive(false);

        if (!bandage)
        {
            inventory.RemoveItem(new Item { itemType = Item.ItemType.medkit, amount = 1 });
            dataImporter.SendInventoryData();
        }
        else if (bandage)
        {
            inventory.RemoveItem(new Item { itemType = Item.ItemType.bandage, amount = 1 });
            dataImporter.SendInventoryData();
        }

        healing = false;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, vehicleCheckRadius);
    }

}
