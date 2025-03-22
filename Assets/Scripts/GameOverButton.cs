using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButton : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(WaitAndReturnMenu());
    }

    IEnumerator WaitAndReturnMenu()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }
}
