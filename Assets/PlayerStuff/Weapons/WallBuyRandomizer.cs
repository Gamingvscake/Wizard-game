using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuyRandomizer : MonoBehaviour
{
    public GameObject[] wallBuys;
    public GameObject[] wallBuySpawnLocations;
    public List<int> wallBuyNumbersUsed;
    void Start()
    {
        for (int i = 0; i < wallBuySpawnLocations.Length; i++)
        {
            int temprand = Random.Range(0, wallBuys.Length);
            if (wallBuyNumbersUsed.Contains(temprand) == false)
            {
                wallBuyNumbersUsed.Add(temprand);
                wallBuys[temprand].transform.position = wallBuySpawnLocations[i].transform.position;
                wallBuys[temprand].transform.rotation = wallBuySpawnLocations[i].transform.rotation;
            }
            else
            {
                i--;
            }
        }
    }
}
