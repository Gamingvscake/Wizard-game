using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronScript : MonoBehaviour
{
    public bool InCauldronRange;
    public PlayerInflicts PI;
    public int Damage2;
    WeaponSwapControl wsc;
    public float SplashSize;
    public GameObject CauldronScreen;
    public bool InCauldronScreen;
    public FirstPersonController fpc;
    public int LifeSteal;
    public GameObject CurScreen;
    private void Start()
    {
        wsc = GetComponent<WeaponSwapControl>();
        CauldronScreen.SetActive(false);
        CurScreen.SetActive(false);
    }
    private void Update()
    {
        if(InCauldronRange && Input.GetKeyDown(KeyCode.E))
        {
            if (InCauldronScreen == false)
            {
                InCauldronScreen = true;
                Work();
            }
            else
            {
                InCauldronScreen = false;
                CurScreen.SetActive(false);
                CauldronScreen.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cauldron")
        {
            InCauldronRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Cauldron")
        {
            InCauldronRange = false;
            InCauldronScreen = false;
            CurScreen.SetActive(false);
            CauldronScreen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void Work()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CurScreen.SetActive(true);
        CauldronScreen.SetActive(true);
    }
    private enum UpgradeType 
    {
        HealthUp,
        DamageUp,
        SplashUp,
        SpeedUp,
        LifeSteal,
        StaminaUp,
        ManaUp,
        WeaponSlotUp
    }
    public void BUTTONWORK(int UT)
    {
        if (UT == (int)UpgradeType.HealthUp)
        {
            if (wsc.points >= 1000)
            {
                PI.PlayerMaxHealth += 10;
                wsc.points -= 1000;
            }
        }
        else if (UT == (int)UpgradeType.DamageUp)
        {
            if (wsc.points >= 1000)
            {
                Damage2 += 5;
                wsc.points -= 1000;
            }
        }
        else if (UT == (int)UpgradeType.SplashUp)
        {
            if (wsc.points >= 1000)
            {
                SplashSize += 0.1f;
                wsc.points -= 1000;
            }
        }
        else if (UT == (int)UpgradeType.SpeedUp)
        {
            if (wsc.points >= 1000)
            {
                fpc.sprintSpeed += 0.5f;
                fpc.walkSpeed += 0.5f;
                wsc.points -= 1000;
            }
        }
        else if (UT == (int)UpgradeType.LifeSteal)
        {
            if (wsc.points >= 1000)
            {
                LifeSteal += 1;
                wsc.points -= 1000;
            }
        }
        else if (UT == (int)UpgradeType.StaminaUp)
        {
            if (wsc.points >= 1000)
            {
                fpc.sprintDuration += 1;
                wsc.points -= 1000;
            }
        }
        else if (UT == (int)UpgradeType.ManaUp)
        {
            if (wsc.points >= 1000)
            {
                wsc.MaxMana += 10;
                wsc.points -= 1000;
            }
        }
        else if (UT == (int)UpgradeType.WeaponSlotUp)
        {
            if (wsc.points >= 1000)
            {
                wsc.MaxNumberOfStaffs += 1;
                wsc.UpdateStaffArray();
                wsc.points -= 1000;
            }
        }
        else print("INT outside of Enum Range");
    }
    public void NextPage(GameObject GO)
    {
        CurScreen.SetActive(false);
        CurScreen = GO;
        CurScreen.SetActive(true);
    }
}
