using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject loginCanvas;
    public GameObject registerCanvas;

    public AudioSource BG_Music;
    public AudioSource UI_Sound;

    // Start is called before the first frame update
    void Start()
    {
        BG_Music.volume = PlayerPrefs.GetFloat("MusicVolume");
        UI_Sound.volume = PlayerPrefs.GetFloat("SoundVolume");

        ClearField();
        ClickLogin();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ClickRegister()
    {
        ClearField();
        registerCanvas.SetActive(true);
    }

    public void ClickLogin()
    {
        ClearField();
        loginCanvas.SetActive(true);
    }

    public void ClearField()
    {
        loginCanvas.SetActive(false);
        registerCanvas.SetActive(false);
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
