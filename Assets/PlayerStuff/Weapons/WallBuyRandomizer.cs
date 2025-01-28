using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuyRandomizer : MonoBehaviour
{
    public GameObject[] wallBuys;
    public int numberOfAvailableStaffs;
    void Start()
    {
        for (int k = 0; k < wallBuys.Length; k++)
        {
            wallBuys[k].gameObject.SetActive(false);
        }
        for (int i = 0; i < numberOfAvailableStaffs; i++)
        {
            int temprand = Random.Range(0, wallBuys.Length);
            if (wallBuys[temprand].activeInHierarchy == false)
            { 
                wallBuys[temprand].SetActive(true); 
            }
            else
            {
                i--;
            }
        }
    }
}
