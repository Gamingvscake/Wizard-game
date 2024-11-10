using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame()
    {
        Debug.Log("It is reacting!!");
        SceneManager.LoadScene(7);
        //The scene of the actual game
    }
    public void SettingsMenu()
    {

        SceneManager.LoadScene("Settings");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}
