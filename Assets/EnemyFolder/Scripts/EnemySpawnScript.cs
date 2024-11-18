using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnScript : MonoBehaviour
{
    public GameObject enemy;
    public Transform[] spawnpoints;
    public int rounds;
    public int amountOfEnemiesLeft;
    public int amountOfEnemies;
    public int spawnIncreasePerRound;
    public int starterAmountOfEnemies;
    public float roundTimer;
    public float spawnTimer;
    public bool inTheRound;
    float tempRTimer;
    float tempSTimer;
    bool tempcanspawn;
    bool roundup = true;
    public List<Transform> Players;
    public Transform[] EntryPoints;
    public EnemyMovementScript enemyMovementScript;

    private void Start()
    {
        amountOfEnemiesLeft = starterAmountOfEnemies;
    }
    private void Update()
    {
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
                    rounds += 1;
                    roundup = false;
                }
                if (tempcanspawn)
                {
                    GameObject temp = Instantiate(enemy, spawnpoints[Random.Range(0, spawnpoints.Length)]);
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

        //Modified enemy stats based on round
        if (rounds >= 5)
        {
            enemyMovementScript.speed = 2;
        }
    }
}
