using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawnScript : MonoBehaviour
{
    public GameObject[] enemy;
    public GameObject[] RangedEnemies;
    public Transform[] spawnpoints;
    public int rounds;
    public int amountOfEnemiesLeft;
    public int amountOfEnemies;
    public int spawnIncreasePerRound;
    public int starterAmountOfEnemies;
    public float roundTimer;
    public float spawnTimer;
    public bool inTheRound;
    public bool rangedEnemiesAdded;
    float tempRTimer;
    float tempSTimer;
    bool tempcanspawn;
    bool roundup = true;
    public List<Transform> Players;
    public Transform[] EntryPoints;
    public EnemyMovementScript enemyMovementScript;


    //For Round Change
    public TextMeshProUGUI roundText;
    public int round = 0;

    private void Start()
    {
        amountOfEnemiesLeft = starterAmountOfEnemies;
    }
    private void Update()
    {
        roundText.text = rounds.ToString();
        if (inTheRound == false)
        {
            if (tempSTimer < spawnTimer)
            {
                tempSTimer += Time.deltaTime;
            }
            else tempcanspawn = true;
            if (tempRTimer < roundTimer)
            {
                tempRTimer += Time.deltaTime;
            }
            else
            {
                if (roundup)
                {
                    if (rangedEnemiesAdded == false && rounds == 4)
                    {
                        for (int i = 0; i < RangedEnemies.Length; i++)
                        {
                            enemy[i+1] = RangedEnemies[i];
                        }
                        rangedEnemiesAdded = true;
                    }
                    rounds += 1;
                    roundup = false;
                }
                if (tempcanspawn)
                {
                    GameObject temp = Instantiate(enemy[Random.Range(0,enemy.Length)], spawnpoints[Random.Range(0, spawnpoints.Length)]);
                    temp.GetComponent<EnemyHealthScript>().enemySpawn = this;
                    temp.GetComponent<EnemyMovementScript>().Players = Players;
                    temp.GetComponent<EnemyMovementScript>().entryPoints = EntryPoints;
                    amountOfEnemies += 1;
                    tempSTimer = 0;
                    tempcanspawn = false;
                }
                if (amountOfEnemies == amountOfEnemiesLeft)
                {
                    tempRTimer = 0;
                    amountOfEnemiesLeft += spawnIncreasePerRound;
                    inTheRound = true;
                }
            }
        }
        if (amountOfEnemies == 0)
        {
            inTheRound = false;
            roundup = true;
        }
    }
}
