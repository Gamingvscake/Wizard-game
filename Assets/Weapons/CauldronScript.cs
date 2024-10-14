using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronScript : MonoBehaviour
{
    public bool InCauldronRange;
    private void Update()
    {
        if(InCauldronRange && Input.GetKeyDown(KeyCode.E))
        {
            Work();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cauldron")
        {
            InCauldronRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Cauldron")
        {
            InCauldronRange = false;
        }
    }
    private void Work()
    {
        print("PLACEHOLDER");
    }
}
