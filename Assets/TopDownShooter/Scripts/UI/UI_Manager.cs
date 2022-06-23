using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
	public GameObject GameplayPanel;
	public GameObject ShopPanel;
	public GameObject deathPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Dead()
    {
    	GameplayPanel.SetActive(false);
    	ShopPanel.SetActive(false);
    	deathPanel.SetActive(true);
    }

    public void LoadScene(string scene)
    {
    	SceneManager.LoadScene(scene);
        Time.timeScale = 1f;
    }
}
