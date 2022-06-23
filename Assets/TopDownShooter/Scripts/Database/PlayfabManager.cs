using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayfabManager : MonoBehaviour
{
    public TMP_Text infoTXT;

    [Header("UI")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button[] btn;

    [Header("Register")]
    public TMP_InputField usernameInput;
    public TMP_InputField RegisterEmailInput;
    public TMP_InputField RegisterPasswordInput;

    [Header("TXT")]
    public string username;

    [Header("Energy")]
    public int maxEnergy;
    public int coins;

    [Header("Cars")]
    public int car1940;
    public int carBlackHawk;
    public int carBubble;
    public int carHotrod;
    public int carIceCream;
    public int carMinivan;
    public int carMonsterTruck;
    public int carMuscle;
    public int carPickup;
    public int carPoop;
    public int carPork;
    public int carPrison;
    public int carWater;
    public int carWiener;
    public int carVehicleIndex;

    [Header("Dog")]
    public int akira;
    public int husky;
    public int dogIndex;

    [Header("Item Value")]
    public int ammoAmount;
    public int bandageAmount;
    public int chickenAmount;
    public int grenadeAmount;
    public int landmineAmount;
    public int medkitAmount;
    public int metalDoorAmount;
    public int metalWallAmount;
    public int molotovAmount;
    public int woodDoorAmount;
    public int woodWallAmount;
    public int smokeAmount;
    public int stoneAmount;
    public int woodAmount;

    [Header("SRV")]
    public int srvAmount;
    public int civilAmount;

    [Header("Weapons")]
    public int ak12Amount;
    public int ak74Amount;
    public int krissAmount;
    public int flamethrowerAmount;
    public int G36CAmount;
    public int G3A4Amount;
    public int Glock17Amount;
    public int MP5Amount;
    public int MP7Amount;
    public int Tec9Amount;
    public int UMPAmount;
    public int UZIAmount;

    Menu menu;

    public static PlayfabManager database;

    private void Awake()
    {
        if(database == null)
        {
            database = this;
        }else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        menu = GetComponent<Menu>();
    }


    private void Update()
    {
        
    }

    public void RegisterBTN()
    {
        if(RegisterPasswordInput.text.Length < 6)
        {
            infoTXT.text = "Password is short";
            return;
        }

        if (usernameInput.text.Length <= 0)
        {
            infoTXT.text = "Missing Username";
            return;
        }

        if (RegisterEmailInput.text.Length <= 0)
        {
            infoTXT.text = "Missing Email";
            return;
        }

        var request = new RegisterPlayFabUserRequest
        {
            Email = RegisterEmailInput.text,
            Password = RegisterPasswordInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    public void LoginButton()
    {
        if (passwordInput.text.Length < 6)
        {
            infoTXT.text = "Password is short";
            return;
        }

        if (emailInput.text.Length <= 0)
        {
            infoTXT.text = "Missing Email";
            return;
        }

        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text
        };

        for (int i = 0; i < btn.Length; i++)
        {
            btn[i].interactable = false;
        }

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult result)
    {
        infoTXT.text = "Logged In";
        GetData();
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        infoTXT.text = "Your Account Registered Successfully please login to continue";
        StartCoroutine(CreateData());
    }

    public void ResetPasswordButton()
    {
        if (emailInput.text.Length <= 0)
        {
            infoTXT.text = "Missing Email";
            return;
        }

        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
            TitleId = "5D42F"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        infoTXT.text = "Password Reset Mail Sended!!!";
    }

    public void GetData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecieved, OnError);
    }

    void OnDataRecieved(GetUserDataResult result)
    {
        //Debug.Log("Recieved");
        
        if (result.Data != null)
        {
            //infoTXT.text = "Recieved";

            

            username = result.Data["Username"].Value;

            int.TryParse(result.Data["A Energy"].Value, out maxEnergy);
            int.TryParse(result.Data["Coins"].Value, out coins);

            int.TryParse(result.Data["Item Ammo"].Value, out ammoAmount);
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
            int.TryParse(result.Data["Item Wood"].Value, out woodAmount);

            int.TryParse(result.Data["Car 1940"].Value, out car1940);
            int.TryParse(result.Data["Car BlackHawk"].Value, out carBlackHawk);
            int.TryParse(result.Data["Car Bubble"].Value, out carBubble);
            int.TryParse(result.Data["Car Hotrod"].Value, out carHotrod);
            int.TryParse(result.Data["Car IceCream"].Value, out carIceCream);
            int.TryParse(result.Data["Car Minivan"].Value, out carMinivan);
            int.TryParse(result.Data["Car MonsterTruck"].Value, out carMonsterTruck);
            int.TryParse(result.Data["Car Muscle"].Value, out carMuscle);
            int.TryParse(result.Data["Car Pickup"].Value, out carPickup);
            int.TryParse(result.Data["Car Poop"].Value, out carPoop);
            int.TryParse(result.Data["Car Pork"].Value, out carPork);
            int.TryParse(result.Data["Car Prison"].Value, out carPrison);
            int.TryParse(result.Data["Car Water"].Value, out carWater);
            int.TryParse(result.Data["Car Wiener"].Value, out carWiener);
            int.TryParse(result.Data["Car VehicleIndex"].Value, out carVehicleIndex);



            int.TryParse(result.Data["SRV Survival"].Value, out srvAmount);
            int.TryParse(result.Data["SRV Civils"].Value, out civilAmount);

            int.TryParse(result.Data["Dog Akira"].Value, out akira);
            int.TryParse(result.Data["Dog Husky"].Value, out husky);
            int.TryParse(result.Data["Dog Index"].Value, out dogIndex);



            int.TryParse(result.Data["Weapon AK12"].Value, out ak12Amount);
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
            int.TryParse(result.Data["Weapon UZI"].Value, out UZIAmount);

            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Login"))
                SceneManager.LoadScene("MenuEnvironment");
        }
        else
        {
            infoTXT.text = "Recieved but not loaded";
        }
    }

    IEnumerator CreateData()
    {
        SendData("Username", usernameInput.text);

        yield return new WaitForSeconds(0.01f);

        SendData("Item Ammo", "300");
        yield return new WaitForSeconds(0.01f);
        SendData("Item Medkit", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Item Bandage", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Item Grenade", "1");
        yield return new WaitForSeconds(0.01f);
        SendData("Item Chicken", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Item Landmine", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Item MetalDoor", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Item MetalWall", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Item WoodDoor", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Item WoodWall", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Item Molotov", "1");
        yield return new WaitForSeconds(0.01f);
        SendData("Item Smoke", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Item Stone", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Item Wood", "0");
        yield return new WaitForSeconds(0.01f);

        SendData("Car 1940", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Car BlackHawk", "0");
        SendData("Car Bubble", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Car Hotrod", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Car IceCream", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Car Minivan", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Car MonsterTruck", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Car Muscle", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Car Pickup", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Car Poop", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Car Pork", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Car Prison", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Car Water", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Car Wiener", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Car VehicleIndex", "0");
        yield return new WaitForSeconds(0.01f);

        SendData("SRV Civils", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("SRV Survival", "0");
        yield return new WaitForSeconds(0.01f);

        SendData("Weapon AK12", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Weapon AK74", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Weapon Kriss", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Weapon Flamethrower", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Weapon G36C", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Weapon G3A4", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Weapon Glock17", "1");
        yield return new WaitForSeconds(0.01f);
        SendData("Weapon MP5", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Weapon MP7", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Weapon Tec9", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Weapon UMP", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Weapon UZI", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("A Energy", maxEnergy.ToString());
        yield return new WaitForSeconds(0.01f);


        SendData("Dog Akira", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Dog Husky", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Dog Index", "0");
        yield return new WaitForSeconds(0.01f);
        SendData("Coins", "200");

        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecieved, Reload);
    }

    public void rawDataSend(string name, string amount)
    {
        SendData(name, amount);
    }

    public void SendData(string name, string amount)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {name, amount}
            }
        };
        Debug.Log("Sending");
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("User Data Send");
    }

    void OnError(PlayFabError error)
    {
        Debug.Log(error.ErrorMessage);
        infoTXT.text = error.ErrorMessage;
    }

    void Reload(PlayFabError error)
    {
        SendData("Username", usernameInput.text);

        SendData("Item Ammo", "300");
        SendData("Item Medkit", "0");
        SendData("Item Bandage", "0");
        SendData("Item Grenade", "1");
        SendData("Item Chicken", "0");
        SendData("Item Landmine", "0");
        SendData("Item MetalDoor", "0");
        SendData("Item MetalWall", "0");
        SendData("Item WoodDoor", "0");
        SendData("Item WoodWall", "0");
        SendData("Item Molotov", "1");
        SendData("Item Smoke", "0");
        SendData("Item Stone", "0");
        SendData("Item Wood", "0");

        SendData("Car 1940", "0");
        SendData("Car BlackHawk", "0");
        SendData("Car Bubble", "0");
        SendData("Car Hotrod", "0");
        SendData("Car IceCream", "0");
        SendData("Car Minivan", "0");
        SendData("Car MonsterTruck", "0");
        SendData("Car Muscle", "0");
        SendData("Car Pickup", "0");
        SendData("Car Pork", "0");
        SendData("Car Prison", "0");
        SendData("Car Water", "0");
        SendData("Car Wiener", "0");
        SendData("Car VehicleIndex", "0");

        SendData("SRV Civils", "0");
        SendData("SRV Survival", "0");

        SendData("Coins", "200");
        SendData("A Energy", maxEnergy.ToString());

        SendData("Weapon AK12", "0");
        SendData("Weapon AK74", "0");
        SendData("Weapon Kriss", "0");
        SendData("Weapon Flamethrower", "0");
        SendData("Weapon G36C", "0");
        SendData("Weapon G3A4", "0");
        SendData("Weapon Glock17", "1");
        SendData("Weapon MP5", "0");
        SendData("Weapon MP7", "0");
        SendData("Weapon Tec9", "0");
        SendData("Weapon UMP", "0");
        SendData("Weapon UZI", "0");
        SendData("Dog Akira", "0");
        SendData("Dog Husky", "0");
        SendData("Dog Index", "0");

        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecieved, Reload);
    }
}
