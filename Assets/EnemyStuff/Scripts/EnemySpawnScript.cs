using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawnScript : MonoBehaviour
{
    public GameObject[] enemy;
    public GameObject[] RangedEnemies;
    public GameObject boss;
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
    int roundstillboss;
    public bool BossDefeated;
    public List<Transform> Players;
    public Transform[] EntryPoints;
    public EnemyMovementScript enemyMovementScript;
    int playeramountdead;
    float timer;
    public bool work;
    //For Round Change
    public TextMeshProUGUI player1RoundText;
    public TextMeshProUGUI player2RoundText;
    public TextMeshProUGUI player3RoundText;
    public TextMeshProUGUI player4RoundText;


    public AudioSource roundChange;

    private void Start()
    {
        amountOfEnemiesLeft = starterAmountOfEnemies;
        work = true;
    }
    private void Update()
    {
        if (work)
        {
            timer += Time.deltaTime;
            if (timer >= 1)
            {
                playeramountdead = 0;
                for (int i = 0; i < Players.Count; i++)
                {
                    if (Players[i].GetComponentInChildren<DeathScreen>().isGameOver == true ||
                        Players[i].GetComponentInChildren<DeathScreen>().isPlayerDead == true ||
                        Players[i].GetComponentInChildren<UIGameOver>().isGameOver == true ||
                        Players[i].GetComponentInChildren<UIGameOver>().isGameOver == true)
                    {
                        playeramountdead += 1;
                    }
                }
                if (playeramountdead >= Players.Count)
                {
                    work = false;
                }
                timer = 0;
            }
            if (player1RoundText != null) player1RoundText.text = rounds.ToString();
            if (player2RoundText != null) player2RoundText.text = rounds.ToString();
            if (player3RoundText != null) player3RoundText.text = rounds.ToString();
            if (player4RoundText != null) player4RoundText.text = rounds.ToString();
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
                                enemy[i + 1] = RangedEnemies[i];
                            }
                            rangedEnemiesAdded = true;
                        }

                        //sound ends
                        rounds += 1;
                        roundstillboss += 1;
                        roundup = false;
                    }
                    if (tempcanspawn)
                    {
                        if (roundstillboss == 10)
                        {
                            GameObject temp = Instantiate(boss, spawnpoints[Random.Range(0, spawnpoints.Length)]);
                            temp.GetComponent<EnemyHealthScript>().enemySpawn = this;
                            temp.GetComponent<EnemyMovementScript>().Players = Players;
                            temp.GetComponent<EnemyMovementScript>().entryPoints = EntryPoints;
                            temp.GetComponent<EnemyMovementScript>().spawnScript = this;
                            roundstillboss = 0;
                        }
                        else
                        {
                            GameObject temp = Instantiate(enemy[Random.Range(0, enemy.Length)], spawnpoints[Random.Range(0, spawnpoints.Length)]);
                            temp.GetComponent<EnemyHealthScript>().enemySpawn = this;
                            temp.GetComponent<EnemyMovementScript>().Players = Players;
                            temp.GetComponent<EnemyMovementScript>().entryPoints = EntryPoints;
                            temp.GetComponent<EnemyMovementScript>().spawnScript = this;
                        }

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
                //sound plays
                roundChange.Play();
            }
        }
    }
}
