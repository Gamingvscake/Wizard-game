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

                    if (wsc != null)
                    {
                        int revivingPlayerID = wsc.playerID;

                        if (!reviveCounts.ContainsKey(revivingPlayerID))
                        {
                            reviveCounts[revivingPlayerID] = 0; 
                        }

                        reviveCounts[revivingPlayerID]++; 
                        print("Player " + revivingPlayerID + " has revived another player " + reviveCounts[revivingPlayerID] + " times.");
                    }

                    if (UIgameOver.redwiz != null)
                    {
                        UIgameOver.redwiz.gameObject.SetActive(true);
                    }
                    if (UIgameOver.pinkwiz != null)
                    {
                        UIgameOver.pinkwiz.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    private void UpdateReviveText(int playerID)
    {
        // Make sure the playerID is valid and that the text array is initialized
        if (playerID >= 1 && playerID <= reviveTexts.Length)
        {
            reviveTexts[playerID - 1].text = "Revives: " + reviveCounts[playerID];
        }
    }

    public static int GetReviveCount(int playerID)
    {
        return reviveCounts.ContainsKey(playerID) ? reviveCounts[playerID] : 0;
    }

}