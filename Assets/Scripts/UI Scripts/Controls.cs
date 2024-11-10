using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{
    // Start is called before the first frame update
    public void Audio()
    {
        SceneManager.LoadScene("Settings 1");
    }

    public void Graphics()
    {
        SceneManager.LoadScene("Settings 2");
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void Brightness()
    {
        SceneManager.LoadScene("Settings 4");
    }

    public void Return()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
