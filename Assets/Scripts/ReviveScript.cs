using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveScript : MonoBehaviour
{
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
}