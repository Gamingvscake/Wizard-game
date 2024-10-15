using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInflicts : MonoBehaviour
{

    [Header("Health Settings")]
    public int PlayerMaxHealth;
    public static int PlayerCurrentHealth;

    [Header("Regen Settings")]
    public static float HealthRegenDelay = 3f;
    public static float HealthRegenRate = 10f;

    [Header("Misc")]
    public int IFrameTime;
    public static int MaxDamage; 
    public static int MinDamage; //Max and Min Damage are altered by the DamageSource script, these do not need to be altered from here
    private bool DamageDealt;



    public static float regenTimer;
    public static bool isRegenerating;

    public static bool IFrames = false;
    public static bool TakingNormalDamage = false;
    public static bool TakingDrainingDamage = false;

    void Start()
    {
        PlayerCurrentHealth = PlayerMaxHealth;
        regenTimer = HealthRegenDelay;
        isRegenerating = false;
    }

    private void StartHealthRegen()
    {
        if (!isRegenerating)
        {
            isRegenerating = true;
            StartCoroutine(RegenerateHealth());
        }
    }

    private IEnumerator WaitForIFrames()
    {
        yield return new WaitForSeconds(IFrameTime);
        IFrames = false;
        DamageDealt = false;
    }

    private IEnumerator RegenerateHealth()
    {
        while (regenTimer <= 0)
        {
            PlayerCurrentHealth += 1;
            PlayerCurrentHealth = Mathf.Min(PlayerCurrentHealth, PlayerMaxHealth);
            yield return new WaitForSeconds(1f / HealthRegenRate);
        }
        isRegenerating = false;
    }

    void Update()
    {
        if (TakingNormalDamage && !DamageDealt)
        {
            PlayerCurrentHealth -= Random.Range(MinDamage, MaxDamage);
            DamageDealt = true;
            regenTimer = HealthRegenDelay;
            isRegenerating = false;
            StartCoroutine(WaitForIFrames());
            TakingNormalDamage = false;
        }

        if (TakingDrainingDamage && !DamageDealt)
        {
            PlayerCurrentHealth -= Random.Range(MinDamage, MaxDamage);
            DamageDealt = true;
            regenTimer = HealthRegenDelay;
            isRegenerating = false;
            StartCoroutine(WaitForIFrames());
            TakingDrainingDamage = false;
        }
            if (regenTimer > 0)
            {
                regenTimer -= Time.deltaTime;
            }
            else
            {
                StartHealthRegen();
            }
            Debug.Log(regenTimer.ToString());
    }
}