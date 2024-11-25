using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnvilScript : MonoBehaviour
{
    public bool InAnvilRange, InUpgradeScreen;
    public GameObject UpgradeScreen;
    public SpellController spellCon;
    WeaponSwapControl weaponSwapControl;
    public GameObject StaffPoint, UpgradedStaffPoint, SpellPoint, UpgradedSpellPoint;
    public TextMeshProUGUI LevelText, NextLevelText;
    void Start()
    {
        UpgradeScreen.SetActive(false);
        weaponSwapControl = GetComponent<WeaponSwapControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InAnvilRange && Input.GetKeyDown(KeyCode.E))
        {
            if (InUpgradeScreen == false)
            {
                InUpgradeScreen = true;
                Work();
            }
            else
            {
                InUpgradeScreen = false;
                UpgradeScreen.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Anvil")
        {
            InAnvilRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Anvil")
        {
            InAnvilRange = false;
            InUpgradeScreen = false;
            UpgradeScreen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void Work()
    {
        if (spellCon != null)
        {
            LevelText.SetText(spellCon.Level.ToString());
            if (spellCon.upgradedStaff != null) NextLevelText.SetText(spellCon.upgradedStaff.GetComponent<SpellController>().Level.ToString());
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UpgradeScreen.SetActive(true);
    }
    public void UPGRADE()
    {
        if (weaponSwapControl.points >= 1000)
        {
            weaponSwapControl.UpgradeStaff();
            weaponSwapControl.points -= 1000;
        }
    }
}
