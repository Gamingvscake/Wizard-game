using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public PlayerInflicts playerInflicts;
    public UIGameOver uigameover;
    public bool isDead;
    public float moveSpeed = 5f;            

    public bool isPlayerDead = false;        // Flag to check if the health is zero
    public bool isGameOver = false;          // All players are dead
    private Vector3 targetPosition = new Vector3(0, 80, 0); // Target position for this GameObject

    public GameObject gravestone_bevel;  //reference to players gravestone

    private void Update()
    {
        
        if (playerInflicts.PlayerCurrentHealth <= 0 && !isPlayerDead)
        {
            isDead = true;
            isPlayerDead = true;

            // Show the gravestone
            if (gravestone_bevel != null)
            {
                gravestone_bevel.gameObject.SetActive(true);
                Debug.Log("Grave has been shown");
            }
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
            print(players[i]);
            // Get the DeathScreen script attached to each player
            DeathScreen deathScreen = players[i].GetComponentInChildren<DeathScreen>();
            //if (deathScreen != null && !deathScreen.isPlayerDead)
            
               // allPlayersDead = false;  // If any player is not dead, game over is not triggered
               // break;
            
            print(deathScreen);
            print(deathScreen.gravestone_bevel);
            // Check if this player's gravestone is active
            if (deathScreen != null && deathScreen.gravestone_bevel != null)
            {
                
                if (!deathScreen.gravestone_bevel.activeSelf)
                {
                    // If any player's gravestone is not active, set allPlayersDead to false and break
                    
                    allPlayersDead = false;
                    break;
                }
            }
            else if (deathScreen == null && deathScreen.gravestone_bevel == null)
            {
                // If deathScreen or gravestone is missing, log an issue
                
                allPlayersDead = false;  // We cannot proceed if any player is missing these components
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
