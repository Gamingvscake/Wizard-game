using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject item;
    // Update is called once per frame
    void Update()
    {
        NewMethod();

    }

    private void NewMethod()
    {
        Destroy(item, 1); //5 is how many seconds you want before the object deletes itself
    }
}
