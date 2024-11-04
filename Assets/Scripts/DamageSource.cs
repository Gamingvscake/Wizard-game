using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    public bool DrainEnemy;
    public int maxDamage;
    public int minDamage;
    public PlayerInflicts UIPI;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (UIPI == null)
            {
                UIPI = other.GetComponent<PlayerInflicts>();
            }

            if (UIPI != null && !UIPI.IFrames)
            {
                if (DrainEnemy)
                {
                    UIPI.TakingDrainingDamage = true;
                }
                else
                {
                    UIPI.TakingNormalDamage = true;
                    UIPI.MaxDamage = maxDamage;
                    UIPI.MinDamage = minDamage;
                }

                UIPI.IFrames = true;
                UIPI.regenTimer = UIPI.HealthRegenDelay;
                UIPI.isRegenerating = false;
            }
        }
    }
}