using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIGameOver : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerInflicts playerInflicts; // Reference to PlayerInflicts
    public Image DeathImage;          

    private bool isGameOver = false;     // Flag to check if the health is zer

    private void Start()
    {
        // Hide the game over image at the start
        if (DeathImage != null)
        {
            DeathImage.gameObject.SetActive(false);
            Debug.Log("Game Over UI hidden at start.");
        }
    }

    private void Update()
    {
        // Check if PlayerCurrentHealth is 0 and the game over state hasn't been triggered yet
        if (playerInflicts.PlayerCurrentHealth <= 0 && !isGameOver)
        {
            isGameOver = true;
            Debug.Log("Player health is 0 or below. Showing Game Over UI.");


            // Show the game over image

            //isGameOver = true;

            if (DeathImage != null)
            {
                
                DeathImage.gameObject.SetActive(true);
            }
        }
    }
}
