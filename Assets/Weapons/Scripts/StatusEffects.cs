using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Image statusImage;
    public Sprite[] Sprites = new Sprite[8];
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
    private void Start()
    {
        statusImage.gameObject.SetActive(false);
    }
    void Update()
    {
        if (working)
        {
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
                    StatusFPC.walkSpeed = StatusFPC.walkSpeed * 2;
                    StatusFPC.sprintSpeed = StatusFPC.sprintSpeed * 2;
                }
                working = false;
            }
        }
        else
        {
            if (statusImage.gameObject.activeSelf == true) statusImage.gameObject.SetActive(false);
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
        else if (statustemp == (int)Status.ManaDrain)
        {
            effects = Status.ManaDrain;
            statusImage.sprite = Sprites[5];
        }
        else if (statustemp == (int)Status.Slow)
        {
            statusImage.sprite = Sprites[7];
            effects = Status.Slow;
            StatusFPC.walkSpeed = StatusFPC.walkSpeed / 2;
            StatusFPC.sprintSpeed = StatusFPC.sprintSpeed / 2;
        }
        tempduration = duration;
        tempdamage = damage;
        working = true;
    }
}
