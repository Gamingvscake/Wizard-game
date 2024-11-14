using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject TitleCam;
    public GameObject MenuCam;
    public GameObject SettingsCam;
    public GameObject AudioCam;
    public GameObject GraphicsCam;
    public GameObject ControlsCam;
    public GameObject BrightnessCam;

    public void BeginGame()
    {
        TitleCam.SetActive(false);
        MenuCam.SetActive(true);
        SettingsCam.SetActive(false);
        AudioCam.SetActive(false);
        GraphicsCam.SetActive(false);
        ControlsCam.SetActive(false);
        BrightnessCam.SetActive(false);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }
    public void SettingsMenu()
    {
        TitleCam.SetActive(false);
        MenuCam.SetActive(false);
        SettingsCam.SetActive(true);
        AudioCam.SetActive(false);
        GraphicsCam.SetActive(false);
        ControlsCam.SetActive(false);
        BrightnessCam.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void Audio()
    {
        TitleCam.SetActive(false);
        MenuCam.SetActive(false);
        SettingsCam.SetActive(false);
        AudioCam.SetActive(true);
        GraphicsCam.SetActive(false);
        ControlsCam.SetActive(false);
        BrightnessCam.SetActive(false);
    }

    public void Graphics()
    {
        TitleCam.SetActive(false);
        MenuCam.SetActive(false);
        SettingsCam.SetActive(false);
        AudioCam.SetActive(false);
        GraphicsCam.SetActive(true);
        ControlsCam.SetActive(false);
        BrightnessCam.SetActive(false);
    }

    public void Controls()
    {
        TitleCam.SetActive(false);
        MenuCam.SetActive(false);
        SettingsCam.SetActive(false);
        AudioCam.SetActive(false);
        GraphicsCam.SetActive(false);
        ControlsCam.SetActive(true);
        BrightnessCam.SetActive(false);
    }

    public void Brightness()
    {
        TitleCam.SetActive(false);
        MenuCam.SetActive(false);
        SettingsCam.SetActive(false);
        AudioCam.SetActive(false);
        GraphicsCam.SetActive(false);
        ControlsCam.SetActive(false);
        BrightnessCam.SetActive(true);
    }

    public void Return()
    {
        TitleCam.SetActive(false);
        MenuCam.SetActive(true);
        SettingsCam.SetActive(false);
        AudioCam.SetActive(false);
        GraphicsCam.SetActive(false);
        ControlsCam.SetActive(false);
        BrightnessCam.SetActive(false);
    }

}
