using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffects : MonoBehaviour
{
    public Status effects;
    public PlayerInflicts StatusPI;
    public WeaponSwapControl StatusWSC;
    public FirstPersonController StatusFPC;
    float tempduration;
    int tempdamage;
    bool working;
    public float maxtimer, temptimer;
    public enum Status
    {
        None,
        NeutralTBD,
        Burn,
        Poison,
        EarthTBD,
        AirTBD,
        ManaDrain,
        DarkTBD,
        Slow
    }
    void Update()
    {
        if (working)
        {
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
                        StatusPI.regenTimer = StatusPI.HealthRegenDelay/2;
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
                    StatusFPC.walkSpeed = StatusFPC.walkSpeed * 2;
                    StatusFPC.sprintSpeed = StatusFPC.sprintSpeed * 2;
                }
                working = false;
            }
        }
    }
    public void DoStatusWork(int statustemp, float duration, int damage)
    {
        if (statustemp == (int)Status.Burn) effects = Status.Burn;
        else if (statustemp == (int)Status.Poison) effects = Status.Poison;
        else if (statustemp == (int)Status.ManaDrain)
        {
            effects = Status.ManaDrain;
        }
        else if (statustemp == (int)Status.Slow)
        {
            effects = Status.Slow;
            StatusFPC.walkSpeed = StatusFPC.walkSpeed / 2;
            StatusFPC.sprintSpeed = StatusFPC.sprintSpeed / 2;
        }
        tempduration = duration;
        tempdamage = damage;
        working = true;
    }
}
