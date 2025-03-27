using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReviveScript : MonoBehaviour
{
    public WeaponSwapControl wsc;
    public TMP_Text[] reviveTexts;
    private static Dictionary<int, int> reviveCounts = new Dictionary<int, int>();

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
                    UIgameOver.isPlayerDead = false;
                    playerInflicts.wasrevived = true;

                    int revivingPlayerID = wsc.playerID;

                    if (!reviveCounts.ContainsKey(revivingPlayerID))
                    {
                        reviveCounts[revivingPlayerID] = 0;
                    }

                    reviveCounts[revivingPlayerID]++;
                    print("Player " + revivingPlayerID + " has revived another player " + reviveCounts[revivingPlayerID] + " times.");

                    if (UIgameOver.wizardPrefab != null)
                    {
                        UIgameOver.wizardPrefab.gameObject.SetActive(true);
                    }
                    /*if (UIgameOver.pinkwiz != null)
                    {
                        UIgameOver.pinkwiz.gameObject.SetActive(true);
                    }*/
                }
            }
        }


    }

    

}