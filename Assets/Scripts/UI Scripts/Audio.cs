using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Audio : MonoBehaviour
{
    // Start is called before the first frame update
    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void Graphics()
    {
        SceneManager.LoadScene("Settings 2");
    }

    public void Controls()
    {
        SceneManager.LoadScene("Settings 3");
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
