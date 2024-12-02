using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelection : MonoBehaviour
{
    //Cylinder assignment
    public GameObject[] cylinders;
    //Selection management (Didn't know this existed until Kevin, thanks Kevin)
    private int currentSelection = 0;

    void Start()
    {
        //Only outlines first cylinder on start
        UpdateOutline();
    }

    void Update()
    {
        //Select cylinders with A and D
        if (Input.GetKeyDown(KeyCode.D))
        {
            currentSelection = (currentSelection + 1) % 4;
            UpdateOutline();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            currentSelection = (currentSelection - 1 + 4) % 4;
            UpdateOutline();
        }

        //Loads Tavern level when spacebar is pressed
        //Once we have character models we will need to update this to actually assign model
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(3);
        }
    }

    void UpdateOutline()
    {
        for (int i = 0; i < cylinders.Length; i++)
        {
            Outline outline = cylinders[i].GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = (i == currentSelection);
            }
        }
    }
}
