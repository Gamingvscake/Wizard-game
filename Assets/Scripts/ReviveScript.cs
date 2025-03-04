using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
                    deathScreen.isDead = false;
                    deathScreen.isPlayerDead = false;
                    playerInflicts.PlayerCurrentHealth = 75;
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