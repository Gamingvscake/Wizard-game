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
    
    public GameObject bluePosePrefab;
    public GameObject redwiz;
    public GameObject pinkwiz;
    public Slider healthSlider;
    public GameObject manaGauge;
    public GameObject points;
    public GameObject statusicons;
    public GameObject staffspawnpoint;
    

    public bool isPlayerDead = false;     // Flag to check if the health is zero
    public bool isGameOver = false;       // Check if all players dead

    private void Start()
    {
        // Hide the game over image at the start
        if (DeathImage != null)
        {
            DeathImage.gameObject.SetActive(false);
            //Debug.Log("Game Over UI hidden at start.");
        }

     

    }


    public Transform player1;          // Reference to Player1
    public Transform player1Camera;    // Reference to Player1Camera
    
    private void HidePlayer()
    {
        // Detach the camera before hiding the player
        player1Camera.parent = null;

        // Hide the player
        player1.gameObject.SetActive(false);

    }

    private void Update()
    {
        // Check if PlayerCurrentHealth is 0 and the game over state hasn't been triggered yet
        if (playerInflicts.PlayerCurrentHealth <= 0 && !isGameOver)
        {
            isGameOver = true;
            //Debug.Log("Player health is 0 or below. Showing Game Over UI.");


            StartCoroutine(HandleGameOver());
        }
    }

    private IEnumerator HandleGameOver()
    {
        if (isGameOver)
        {
            

            // Show the death image for a few seconds
            if (DeathImage != null)
            {
                DeathImage.gameObject.SetActive(true);
            }

            // Wait for a few seconds (you can adjust the time)
            yield return new WaitForSeconds(2f);

            // Hide the DeathImage
           // if (DeathImage != null)
            {
                DeathImage.gameObject.SetActive(false);
            }

            

            // Continue the game over handling (hide other elements, disable scripts, etc.)
            if (bluePosePrefab != null)
            {
                MeshRenderer meshRenderer = bluePosePrefab.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = false;
                }
            }

            if (redwiz != null)
            {
                redwiz.gameObject.SetActive(false);
            }

            if (pinkwiz != null)
            {
                pinkwiz.gameObject.SetActive(false);
            }

            if (healthSlider != null)
            {
                healthSlider.gameObject.SetActive(false);
            }

            if (manaGauge != null)
            {
                manaGauge.SetActive(false);
            }

            if (points != null)
            {
                points.gameObject.SetActive(false);
            }

            if (statusicons != null)
            {
                statusicons.gameObject.SetActive(false);
            }

            if (staffspawnpoint != null)
            {
                staffspawnpoint.gameObject.SetActive(false);
            }

            // Disable the WeaponSwapControl script
            WeaponSwapControl weaponSwapControl = player1.GetComponent<WeaponSwapControl>();
            if (weaponSwapControl != null)
            {
                weaponSwapControl.enabled = false;
            }

            // Disable the MovementController
            MovementController movementController = player1.GetComponent<MovementController>();
            if (movementController != null)
            {
                movementController.enabled = false;
            }
        }
    }
}
