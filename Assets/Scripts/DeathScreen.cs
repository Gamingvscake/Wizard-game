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
        
        if (playerInflicts.PlayerCurrentHealth <= 0 && !isDead)
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

        if (isDead)
        {

        }

        if (isPlayerDead)
        {
            CheckGameOver();
        }


        if (isGameOver)  // Camera is moved up
        {
            gameObject.transform.position = Vector3.Lerp(
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

        foreach ( GameObject player in players )
        {
            // Get the DeathScreen script attached to each player
            DeathScreen deathScreen = player.GetComponent<DeathScreen>();

            if (deathScreen != null && !deathScreen.isPlayerDead)
            {
                allPlayersDead = false;  // If any player is not dead, game over is not triggered
                break;
            }

            // Check if this player's gravestone is active
            if (deathScreen != null && deathScreen.gravestone_bevel != null && !deathScreen.gravestone_bevel.activeSelf)
            {
                Debug.Log("Bro This is being read");
                // If any player's gravestone is not active, game over should not be triggered
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
