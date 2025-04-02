using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoardManager : MonoBehaviour
{
    public GameObject scoreboardCanvas;  
    public GameObject[] players;         // Array of all player GameObjects
    public PlayerInflicts playerInflicts;
    private int deadPlayersCount = 0;    // Count of players who are dead
    private int activePlayersCount = 0; // Count number of active players
    private bool allPlayersDead = false;
    

    public TMP_Text[] reviveCountTexts;  
    private Dictionary<GameObject, int> reviveCounts = new Dictionary<GameObject, int>();
    //private Dictionary<GameObject, int> revivesPerformed = new Dictionary<GameObject, int>();

    private void Start()
    {
        // Hide the scoreboard canvas at the start
        if (scoreboardCanvas != null)
        {
            scoreboardCanvas.SetActive(false);
        }

        foreach (GameObject player in players)
        {

            reviveCounts[player] = 0;
            /*killCounts[player] = 0;*/
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
        UpdateReviveCountUI();
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

    public void UpdateReviveCount(GameObject player)
    {
        if (reviveCounts.ContainsKey(player))
        {
            reviveCounts[player]++;
        }
    }

    private void UpdateReviveCountUI()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (reviveCountTexts.Length > i)
            {
                reviveCountTexts[i].text = reviveCounts[players[i]].ToString();
            }
        }
    }


}
