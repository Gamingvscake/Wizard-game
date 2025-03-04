using System.Collections;
using UnityEngine;

public class PlayerInflicts : MonoBehaviour
{

    [Header("Health Settings")]
    public int PlayerMaxHealth;
    public int PlayerCurrentHealth;

    [Header("Regen Settings")]
    public float HealthRegenDelay = 3f;
    public float HealthRegenRate = 10f;

    [Header("Misc")]
    public int IFrameTime;
    public int MaxDamage; 
    public int MinDamage; //Max and Min Damage are altered by the DamageSource script, these do not need to be altered from here
    private bool DamageDealt;

    public StatusEffects PlayerInflictsSE;

    public float regenTimer;
    public bool isRegenerating;

    public bool IFrames = false;
    public bool TakingNormalDamage = false;
    public bool TakingDrainingDamage = false;

    public bool StatusIncreasedDamage;

    public bool wasrevived;
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
    public void LifeStealDo(int amount)
    {
        PlayerCurrentHealth += amount;
    }
    void Update()
    {
        if (TakingNormalDamage && !DamageDealt)
        {
            if (StatusIncreasedDamage == false) PlayerCurrentHealth -= Random.Range(MinDamage, MaxDamage);
            else PlayerCurrentHealth -= Random.Range(MinDamage, MaxDamage) * 2;
            DamageDealt = true;
            regenTimer = HealthRegenDelay;
            isRegenerating = false;
            StartCoroutine(WaitForIFrames());
            TakingNormalDamage = false;
        }

        if (TakingDrainingDamage && !DamageDealt)
        {
            if (StatusIncreasedDamage == false)PlayerCurrentHealth -= Random.Range(MinDamage, MaxDamage);
            else PlayerCurrentHealth -= Random.Range(MinDamage, MaxDamage)*2;
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
            //Debug.Log(regenTimer.ToString());
    }
}