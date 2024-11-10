using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update
    public void Audio()
    {
        Debug.Log("It is reacting!!");
        SceneManager.LoadScene("Settings 1");
    }

    public void Graphics()
    {
        Debug.Log("It is reacting!!");
        SceneManager.LoadScene("Settings 2");
    }

    public void Controls()
    {
        Debug.Log("It is reacting!!");
        SceneManager.LoadScene("Settings 3");
    }

    public void Brightness()
    {
        Debug.Log("It is reacting!!");
        SceneManager.LoadScene("Settings 4");
    }

    public void Return()
    {
        Debug.Log("It is reacting!!");
        SceneManager.LoadScene("Main Menu");
    }

}
