using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Brightness : MonoBehaviour
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

    public void Controls()
    {
        SceneManager.LoadScene("Settings 3");
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void Return()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
