using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffects : MonoBehaviour
{
    public Status effects;
    public PlayerInflicts StatusPI;
    public WeaponSwapControl StatusWSC;
    public MovementController StatusMC;
    public SpellController currentStaffEquipped;
    float tempduration;
    int tempdamage;
    bool working;
    public float maxtimer, temptimer;
    public Image statusImage;
    public Sprite[] Sprites = new Sprite[8];
    float speedtemp;
    public enum Status
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
        statusImage.gameObject.SetActive(false);
    }
    void Update()
    {
        if (working)
        {
            currentStaffEquipped.castSpeed = speedtemp;
            if (statusImage.gameObject.activeSelf == false)statusImage.gameObject.SetActive(true);
            if (tempduration > 0)
            {
                tempduration -= Time.deltaTime;
                if (temptimer < maxtimer) temptimer += Time.deltaTime;
                else
                {
                    if (effects == Status.Burn)
                    {
                        StatusPI.PlayerCurrentHealth -= tempdamage;
                    }
                    else if (effects == Status.Poison)
                    {
                        StatusPI.PlayerCurrentHealth -= tempdamage;
                        StatusPI.regenTimer = StatusPI.HealthRegenDelay / 2;
                    }
                    else if (effects == Status.ManaDrain)
                    {
                        StatusWSC.Mana -= tempdamage;
                    }
                    temptimer = 0;
                }
            }
            else if (tempduration <= 0)
            {
                tempduration = 0;
                tempdamage = 0;
                if (effects == Status.Slow)
                {
                    StatusMC.walkSpeed = StatusMC.walkSpeed * 2;
                    StatusMC.sprintSpeed = StatusMC.sprintSpeed * 2;
                }
                if (effects == Status.FireRateLower)
                {
                    currentStaffEquipped.castSpeed = speedtemp / 2;
                }
                if(effects == Status.DamageIncrease)
                {
                    StatusPI.StatusIncreasedDamage = false;
                }
                working = false;
            }
        }
        else
        {
            if (statusImage.gameObject.activeSelf == true) statusImage.gameObject.SetActive(false);
            if (StatusPI.StatusIncreasedDamage == true) StatusPI.StatusIncreasedDamage = false;
        }
    }
    public void DoStatusWork(int statustemp, float duration, int damage)
    {
        if (statustemp == (int)Status.Burn)
        {
            effects = Status.Burn;
            statusImage.sprite = Sprites[1];
        }
        else if (statustemp == (int)Status.Poison)
        {
            effects = Status.Poison;
            statusImage.sprite = Sprites[2];
        }
        else if (statustemp == (int)Status.DamageIncrease)
        {
            effects = Status.DamageIncrease;
            statusImage.sprite = Sprites[3];
            StatusPI.StatusIncreasedDamage = true;
        }
        else if (statustemp == (int)Status.FireRateLower)
        {
            effects = Status.FireRateLower;
            statusImage.sprite = Sprites[4];
            speedtemp = currentStaffEquipped.castSpeed * 2;
            currentStaffEquipped.castSpeed = speedtemp;
        }
        else if (statustemp == (int)Status.ManaDrain)
        {
            effects = Status.ManaDrain;
            statusImage.sprite = Sprites[5];
        }
        else if (statustemp == (int)Status.Slow)
        {
            effects = Status.Slow;
            statusImage.sprite = Sprites[7];
            StatusMC.walkSpeed = StatusMC.walkSpeed / 2;
            StatusMC.sprintSpeed = StatusMC.sprintSpeed / 2;
        }
        if (statustemp != (int)Status.None && statustemp != (int)Status.Neutral)
        {
            speedtemp = currentStaffEquipped.castSpeed;
            tempduration = duration;
            tempdamage = damage;
            working = true;
        }
        else return;
    }
}
