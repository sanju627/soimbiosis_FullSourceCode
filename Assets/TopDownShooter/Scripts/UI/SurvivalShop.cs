using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalShop : MonoBehaviour
{
	public GameObject[] borders;
    public GameObject[] Weapons;
	public int selectedWeapon;
	public NPC npc;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < borders.Length; i++)
    	{
            borders[i].SetActive(false);
    	}

        for (int i = 0; i < Weapons.Length; i++)
        {
            Weapons[i].SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectWeapon(int choice)
    {
    	for(int i = 0; i < borders.Length; i++)
    	{
            borders[i].SetActive(false);
    	}
    	selectedWeapon = choice;
        borders[choice].SetActive(true);
    }

    public void EnableWeapon(int choice)
    {
        Weapons[choice].SetActive(true);
    }

    public void Equip()
    {
    	npc.weaponNum = selectedWeapon;
    	npc.Switch(selectedWeapon);
    }
}
