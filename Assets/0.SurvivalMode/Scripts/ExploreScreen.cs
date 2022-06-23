using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreScreen : MonoBehaviour
{
    // Start is called before the first frame update

    public int selectedMap;
    public Player player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseMap(int index)
    {
        selectedMap = index;
        //player.GetComponent<WeaponManger>().OpenShop();
    }
}
