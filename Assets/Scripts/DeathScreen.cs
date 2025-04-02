using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public PlayerInflicts playerInflicts;
    public UIGameOver uigameover;
    public bool isDead;
    public float moveSpeed = 5f;
    public TextMeshProUGUI DisplayDeathCount;
    public bool isPlayerDead = false;        // Flag to check if the health is zero
    public bool isGameOver = false;          // All players are dead
    private Vector3 targetPosition = new Vector3(0, 80, 0); // Target position for this GameObject

    public GameObject gravestone_bevel;  //reference to players gravestone
    float timer;
    private void Update()
    {

        if (playerInflicts.PlayerCurrentHealth <= 0 && !isPlayerDead)
        {
            isDead = true;
            isPlayerDead = true;
            playerInflicts.deathCount++;

            // Show the gravestone
            if (gravestone_bevel != null)
            {
                gravestone_bevel.gameObject.SetActive(true);
                
            }

        }
        else if (playerInflicts.PlayerCurrentHealth > 0)
        {
            if (isDead == true) isDead = false;
            if (isPlayerDead == true) isPlayerDead = false;
            if (playerInflicts.wasrevived == true)
            {
                if (timer < 1) timer += Time.deltaTime;
                if (timer >= 1)
                {
                    playerInflicts.wasrevived = false;
                    timer = 0;
                }
            }
        }

        if (DisplayDeathCount != null)
        {
            DisplayDeathCount.SetText(playerInflicts.deathCount.ToString());
        }

        if (isPlayerDead)
        {
            CheckGameOver();
        }


        if (isGameOver)  // Camera is moved up
        {
            gameObject.transform.position = Vector3.Lerp
            (
                gameObject.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
        }

        
    }

    private void CheckGameOver()
    {
        //Find all the objects with the Player tag
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Check if all the players have their isPlayerDead true
        bool allPlayersDead = true;

        for(int i = 0; i < players.Length; i++)
        {
            // Get the DeathScreen script attached to each player
            DeathScreen deathScreen = players[i].GetComponentInChildren<DeathScreen>();
            

            // Check if this player's gravestone is active
            if (deathScreen == null)
            {
                Debug.LogWarning($"DeathScreen component is missing on player {players[i].name}!");
                allPlayersDead = false;
                continue; // Skip to the next player instead of breaking
            }

            if (deathScreen.gravestone_bevel == null)
            {
                Debug.LogWarning($"Gravestone reference is missing on player {players[i].name}!");
                allPlayersDead = false;
                continue;
            }

            if (!deathScreen.gravestone_bevel.activeSelf)
            {
                
                allPlayersDead = false;
                break;
            }
            
        }

        
        // If all players are dead, trigger game over
        if (allPlayersDead)
        {
           
            isGameOver = true;
            

            
        }
    }




}
