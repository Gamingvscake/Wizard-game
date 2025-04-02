using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReviveScript : MonoBehaviour
{
    public WeaponSwapControl wsc;
    public ScoreBoardManager scoreBoardManager;
    public GameObject reviverObject;

    /*public TMP_Text[] reviveTexts;
    private static Dictionary<int, int> reviveCounts = new Dictionary<int, int>();*/

    void Start()
    {
        if (scoreBoardManager == null)
        {
            scoreBoardManager = FindObjectOfType<ScoreBoardManager>();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == ("Player"))
        {
            
            Transform gravestoneParent = other.transform;
            print("playerrevivingtrigger");
            if (gravestoneParent != null)
            {
                PlayerInflicts playerInflicts = gravestoneParent.GetComponentInChildren<PlayerInflicts>();
                DeathScreen deathScreen = gravestoneParent.GetComponentInChildren<DeathScreen>();
                UIGameOver UIgameOver = gravestoneParent.GetComponentInChildren<UIGameOver>();
                if (playerInflicts != null && deathScreen != null && (deathScreen.isDead == true || deathScreen.isPlayerDead == true))
                {
                    deathScreen.gravestone_bevel.gameObject.SetActive(false);
                    playerInflicts.PlayerCurrentHealth = 75;
                    deathScreen.isDead = false;
                    deathScreen.isPlayerDead = false;
                    deathScreen.isGameOver = false;
                    UIgameOver.isPlayerDead = false;
                    UIgameOver.isGameOver = false;
                    playerInflicts.wasrevived = true;

                    if (scoreBoardManager != null)
                    {
                        scoreBoardManager.UpdateReviveCount(reviverObject);
                    }


                    if (UIgameOver.wizardPrefab != null)
                    {
                        UIgameOver.wizardPrefab.gameObject.SetActive(true);
                    }

                    if (UIgameOver.healthSlider != null)
                    {
                        UIgameOver.healthSlider.gameObject.SetActive(true); 
                    }

                    if (UIgameOver.manaGauge != null)
                    {
                        UIgameOver.manaGauge.SetActive(true); 
                    }

                    if (UIgameOver.points != null)
                    {
                        UIgameOver.points.gameObject.SetActive(true); 
                    }

                    if (UIgameOver.statusicons != null)
                    {
                        UIgameOver.statusicons.gameObject.SetActive(true); 
                    }

                    if (UIgameOver.staffspawnpoint != null)
                    {
                        UIgameOver.staffspawnpoint.gameObject.SetActive(true); 
                    }

                    if (UIgameOver.rounrrdUI != null)
                    {
                        UIgameOver.rounrrdUI.gameObject.SetActive(true);
                    }

                    if (UIgameOver.fullpotion != null)
                    {
                        UIgameOver.fullpotion.gameObject.SetActive(true);
                    }

                    if (UIgameOver.emptypotion != null)
                    {
                        UIgameOver.emptypotion.gameObject.SetActive(true);
                    }

                    if (UIgameOver.perkeffectsholder != null)
                    {
                        UIgameOver.perkeffectsholder.gameObject.SetActive(true);
                    }

                    WeaponSwapControl weaponSwapControl = gravestoneParent.GetComponent<WeaponSwapControl>();
                    if (weaponSwapControl != null)
                    {
                        weaponSwapControl.enabled = true;
                    }

                    
                    MovementController movementController = gravestoneParent.GetComponent<MovementController>();
                    if (movementController != null)
                    {
                        movementController.enabled = true;
                    }

                    

                }
            }
        }


    }



    

}