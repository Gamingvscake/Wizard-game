using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoardManager : MonoBehaviour
{
    public GameObject scoreboardCanvas;  // Reference to the scoreboard canvas
    public GameObject[] players;         // Array of all player GameObjects
    //public GameObject[] gravestones;     // Array of gravestone objects for each player
    public PlayerInflicts playerInflicts;
    private int deadPlayersCount = 0;    // Count of players who are dead (i.e., gravestone is active)
    private bool allPlayersDead = false; // Boolean to check if all players are dead

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

        
        //for (int i = 0; i < players.Length; i++)
        {
            //if (gravestones[i] != null && gravestones[i].activeSelf) 
            {
                //deadPlayersCount++;
            }
        }

        for (int i = 0; i < players.Length; i++)
        {
            if (playerInflicts.PlayerCurrentHealth <= 0)
            {
                deadPlayersCount++;
            }
        }


        if (deadPlayersCount == players.Length && !allPlayersDead)
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
