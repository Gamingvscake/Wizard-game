using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialFinished : MonoBehaviour
{
    public GameObject targetObject;
    public int sceneToLoad = 1;

    void Update()
    {
        if (targetObject == null)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}