using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoardManager : MonoBehaviour
{
    public GameObject scoreboardCanvas;  
    public GameObject[] players;         // Array of all player GameObjects
    public PlayerInflicts playerInflicts;
    private int deadPlayersCount = 0;    // Count of players who are dead
    private int activePlayersCount = 0; // Count number of active players
    private bool allPlayersDead = false;


    private void Start()
    {
        // Hide the scoreboard canvas at the start
        if (scoreboardCanvas != null)
        {
            scoreboardCanvas.SetActive(false);
        }
    }

    private void Update()
    {
        deadPlayersCount = 0;
        activePlayersCount = 0;

        foreach (GameObject player in players)
        {
            if (player.activeInHierarchy) // Check for active players
            {
                activePlayersCount++;
                PlayerInflicts playerInflicts = player.GetComponent<PlayerInflicts>();
                if (playerInflicts != null && playerInflicts.PlayerCurrentHealth <= 0)
                {
                    deadPlayersCount++;
                }
            }
        }

        if (deadPlayersCount == activePlayersCount && activePlayersCount > 0 && !allPlayersDead)
        {
            allPlayersDead = true;
            ShowScoreboard();
        }
    }

    // Show the scoreboard canvas
    private void ShowScoreboard()
    {
        if (scoreboardCanvas != null)
        {
            scoreboardCanvas.SetActive(true);
            Debug.Log("All players are dead. Showing scoreboard.");
        }
    }
}
