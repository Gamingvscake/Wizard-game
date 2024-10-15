using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    public bool DrainEnemy;

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
                PlayerInflicts.MaxDamage = 36;
                PlayerInflicts.MinDamage = 12;
            }

            PlayerInflicts.IFrames = true;
            PlayerInflicts.regenTimer = PlayerInflicts.HealthRegenDelay;
            PlayerInflicts.isRegenerating = false;

        }
    }
}