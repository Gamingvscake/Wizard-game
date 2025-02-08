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
            Debug.Log("Game Over UI hidden at start.");
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
            Debug.Log("Player health is 0 or below. Showing Game Over UI.");


            // Show the game over image

            //isGameOver = true;

            if (DeathImage != null)
            {
                
                DeathImage.gameObject.SetActive(true);
            }

            // Turn off the MeshRenderer for "BluePose 1"
            if (bluePosePrefab != null)
            {
                MeshRenderer meshRenderer = bluePosePrefab.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = false;
                    Debug.Log("BluePose 1 MeshRenderer has been turned off.");
                }
                else
                {
                    Debug.LogWarning("No MeshRenderer found on BluePose 1 prefab.");
                }
            }

            // Turn off the MeshRenderer for "RedWiz"
            if (redwiz != null)
            {
                redwiz.gameObject.SetActive(false);
                Debug.Log("RedWiz has been hidden.");
            }

            


            // Hide the health slider
            if (healthSlider != null)
            {
                healthSlider.gameObject.SetActive(false);
                Debug.Log("Health slider has been hidden.");
            }

            // Hide the ManaGauge
            if (manaGauge != null)
            {
                manaGauge.SetActive(false);
                Debug.Log("ManaGauge has been hidden.");
            }

            //Hide the Points
            if (points != null)
            {
                points.gameObject.SetActive(false);
                Debug.Log("Points are hidden");
            }

            //Hide StatusIcons
            if (statusicons != null)
            {
                statusicons.gameObject.SetActive(false);
                Debug.Log("StatusIcons are hidden");
            }

            // Hide StaffSpawnPoint
            if (staffspawnpoint  != null)
            {
                staffspawnpoint.gameObject.SetActive(false);
                Debug.Log("Staffs are gone");
            }

           

            // Disable the WeaponSwapControl script
            WeaponSwapControl weaponSwapControl = player1.GetComponent<WeaponSwapControl>();
            if (weaponSwapControl != null)
            {
                weaponSwapControl.enabled = false;
                Debug.Log("WeaponSwapControl script disabled.");
            }

            // Disable the MovementController
            MovementController movementController = player1.GetComponent<MovementController>();
            if (movementController != null)
            {
                movementController.enabled = false;
                Debug.Log("MovementController script is disabled");
            }

        }
    }
}
