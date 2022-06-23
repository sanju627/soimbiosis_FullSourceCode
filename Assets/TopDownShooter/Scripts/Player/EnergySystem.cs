using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EnergySystem : MonoBehaviour
{
    public float energy;
    public float fillSpeed;
    public float delay;
    public bool isSending;
    [Header("UI")]
    public Slider energySlider;
    public TMP_Text energyTXT;
    PlayfabManager database;
    DataImporter dataImporter;

    bool usingEnergy;
    

    // Start is called before the first frame update
    void Start()
    {
        database = GameObject.FindGameObjectWithTag("Database").GetComponent<PlayfabManager>();
        dataImporter = GetComponent<DataImporter>();

        

        if (dataImporter.dataLoaded)
        {
            //energy = database.maxEnergy;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isSending && !usingEnergy && dataImporter.dataLoaded)
        {
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Game"))
                StartCoroutine(SendEnergy());
            else if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Game"))
            {
                energy = database.maxEnergy;
                energySlider.value = energy;
                energyTXT.text = energy.ToString("0");
            }
        }
    }

    IEnumerator SendEnergy()
    {
        isSending = true;

        if (energy < 100)
            energy += fillSpeed;

        database.SendData("A Energy", energy.ToString());

        energySlider.value = energy;
        energyTXT.text = energy.ToString("0");

        yield return new WaitForSeconds(delay);

        isSending = false;
    }

    public void UseEnergy(float amount)
    {
        usingEnergy = true;

        energySlider.value = energy;
        energyTXT.text = energy.ToString("0");

        energy -= amount;
        database.SendData("A Energy", energy.ToString());
    }
}
