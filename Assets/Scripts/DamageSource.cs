using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    public bool DrainEnemy;
    public int maxDamage;
    public int minDamage;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerInflicts.IFrames)
        {
            if (DrainEnemy)
            {
                PlayerInflicts.TakingDrainingDamage = true;
            }
            else
            {
                PlayerInflicts.TakingNormalDamage = true;
                PlayerInflicts.MaxDamage = maxDamage;
                PlayerInflicts.MinDamage = minDamage;
            }

            PlayerInflicts.IFrames = true;
            PlayerInflicts.regenTimer = PlayerInflicts.HealthRegenDelay;
            PlayerInflicts.isRegenerating = false;

        }
    }
}