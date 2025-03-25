using System.Collections;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    public bool DrainEnemy;
    public int maxDamage;
    public int minDamage;
    public int statusDamage;
    public float statusDuration;
    public PlayerInflicts UIPI;
    public bool Attacking; // Boolean for whether or not the enemy is attacking
    public bool Attacked;  // Boolean for when attack animation is done, check is still colliding
    private bool playerExitedDuringAttack = false; // Tracks if the player exited during the attack
    public bool inflictStatusEffect;
    public Animator animator; // Animator component reference
    private Coroutine attackCoroutine; // Reference to the attack coroutine
    public bool isRangedAttack;


    public HostileStatus effects;
    public enum HostileStatus
    {
        None,
        Neutral,
        Burn,
        Poison,
        DamageIncrease,
        FireRateLower,
        ManaDrain,
        DarkTBD,
        Slow
    }

    private void Start()
    {
        Physics.IgnoreLayerCollision(10,10);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (UIPI == null)
            {
                UIPI = other.GetComponent<PlayerInflicts>();
            }
            else if (UIPI != other.GetComponent<PlayerInflicts>())
            {
                UIPI = other.GetComponent<PlayerInflicts>();
            }

            if (UIPI != null && !UIPI.IFrames)
            {
                if (!Attacking)
                {
                    // Initiate attack and stop movement
                    Attacking = true;
                    if (isRangedAttack == false)
                    {
                        animator.SetTrigger("Attacking");

                        // Start the coroutine to wait for the animation to complete
                        attackCoroutine = StartCoroutine(CompleteAttack());
                    }
                }
                else if (Attacked && !playerExitedDuringAttack)
                {
                    // Apply damage after attack animation completes
                    UIPI.TakingNormalDamage = true;
                    UIPI.MaxDamage = maxDamage;
                    UIPI.MinDamage = minDamage;
                    UIPI.IFrames = true;
                    UIPI.regenTimer = UIPI.HealthRegenDelay;
                    UIPI.isRegenerating = false;
                    if (inflictStatusEffect) UIPI.PlayerInflictsSE.DoStatusWork((int)effects, statusDuration, statusDamage);

                    // Reset flags after applying damage
                    ResetAttackFlags();
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (UIPI == null)
            {
                UIPI = other.GetComponent<PlayerInflicts>();
            }
            else if (UIPI != other.GetComponent<PlayerInflicts>())
            {
                UIPI = other.GetComponent<PlayerInflicts>();
            }

            if (UIPI != null && !UIPI.IFrames)
            {
                if (!Attacking)
                {
                    // Initiate attack and stop movement
                    Attacking = true;
                    if (isRangedAttack == false)
                    {
                        animator.SetTrigger("Attacking");

                        // Start the coroutine to wait for the animation to complete
                        attackCoroutine = StartCoroutine(CompleteAttack());
                    }
                }
                else if (Attacked && !playerExitedDuringAttack)
                {
                    // Apply damage after attack animation completes
                    UIPI.TakingNormalDamage = true;
                    UIPI.MaxDamage = maxDamage;
                    UIPI.MinDamage = minDamage;
                    UIPI.IFrames = true;
                    UIPI.regenTimer = UIPI.HealthRegenDelay;
                    UIPI.isRegenerating = false;
                    if (inflictStatusEffect) UIPI.PlayerInflictsSE.DoStatusWork((int)effects, statusDuration, statusDamage);

                    // Reset flags after applying damage
                    ResetAttackFlags();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // If the player exits before the attack is completed, mark exit
            playerExitedDuringAttack = true;
        }
    }

    private IEnumerator CompleteAttack()
    {
        // Wait for the attack animation to complete
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        Attacked = true; // Set to true after animation completes

        // If the player has exited during the attack, reset flags
        if (playerExitedDuringAttack)
        {
            ResetAttackFlags();
        }
    }

    private void ResetAttackFlags()
    {
        // Resets all flags to prepare for the next attack cycle
        Attacking = false;
        Attacked = false;
        playerExitedDuringAttack = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isRangedAttack)
        {
            if (collision.collider.CompareTag("Player"))
            {
                UIPI = collision.collider.GetComponent<PlayerInflicts>();
                UIPI.TakingNormalDamage = true;
                UIPI.MaxDamage = maxDamage;
                UIPI.MinDamage = minDamage;
                UIPI.IFrames = true;
                UIPI.regenTimer = UIPI.HealthRegenDelay;
                UIPI.isRegenerating = false;
                if (inflictStatusEffect) UIPI.PlayerInflictsSE.DoStatusWork((int)effects, statusDuration, statusDamage);
            }
            Destroy(gameObject);
        }
    }
}