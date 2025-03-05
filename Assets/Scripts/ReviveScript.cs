using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveScript : MonoBehaviour
{
    public WeaponSwapControl wsc;
    private static Dictionary<int, int> reviveCounts = new Dictionary<int, int>();

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == ("Player"))
        {
            Transform gravestoneParent = other.transform.parent;

            if (gravestoneParent != null)
            {
                PlayerInflicts playerInflicts = gravestoneParent.GetComponentInChildren<PlayerInflicts>();
                DeathScreen deathScreen = gravestoneParent.GetComponentInChildren<DeathScreen>();
                UIGameOver UIgameOver = gravestoneParent.GetComponentInChildren<UIGameOver>();

                if (playerInflicts != null && deathScreen != null && deathScreen.isDead == true && deathScreen.isPlayerDead == true)
                {
                    deathScreen.gravestone_bevel.gameObject.SetActive(false);
                    playerInflicts.PlayerCurrentHealth = 75;
                    deathScreen.isDead = false;
                    deathScreen.isPlayerDead = false;
                    UIgameOver.isPlayerDead = false;
                    playerInflicts.wasrevived = true;

                    //int playerID = playerInflicts.playerID;
                    //if (!reviveCounts.ContainsKey(playerID))
                    {
                       // reviveCounts[playerID] = 0; // Initialize the revive count for this player
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

    public static int GetReviveCount(int playerID)
    {
        if (reviveCounts.ContainsKey(playerID))
        {
            return reviveCounts[playerID];
        }
        return 0; // Return 0 if no revive data for this player
    }

}