using TMPro;
using UnityEngine;

public class WeaponSwapControl : MonoBehaviour
{
    public int MaxNumberOfStaffs;
    public int MaxNumberOfSpells;
    public Transform StaffSpawnPoint;
    public GameObject[] EquippedStaffs;
    public GameObject[] EquippedSpells;
    public GameObject CurrentEquippedStaff;
    public GameObject CurrentEquippedSpell;
    int tempstaff;
    GameObject tempobject;
    public int Mana;
    public int MaxMana;
    public TextMeshProUGUI DisplayMana;
    private void Start()
    {
        Mana = MaxMana;
        if(tempobject == null)
        {
            CurrentEquippedStaff = EquippedStaffs[0];
            tempobject = Instantiate(CurrentEquippedStaff, StaffSpawnPoint);
            tempobject.GetComponent<SpellController>().enabled = true;
        }
    }
    private void Update()
    {
        StaffSwap();
    }
    private void StaffSwap()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            Destroy(tempobject);
            if (tempstaff != MaxNumberOfStaffs - 1)
            {
                tempstaff++;
            }
            CurrentEquippedStaff = EquippedStaffs[tempstaff];
            if (CurrentEquippedStaff != null)
            {
                tempobject = Instantiate(CurrentEquippedStaff, StaffSpawnPoint);
                tempobject.GetComponent<SpellController>().enabled = true;
            }
            if (tempstaff > MaxNumberOfStaffs)
            {
                tempstaff = 0;
            }
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            Destroy(tempobject);
            if (tempstaff != 0)
            {
                tempstaff--;
            }
            CurrentEquippedStaff = EquippedStaffs[tempstaff];
            if(CurrentEquippedStaff != null)
            {
                tempobject = Instantiate(CurrentEquippedStaff, StaffSpawnPoint);
                tempobject.GetComponent<SpellController>().enabled = true;
            }
        }
    }
    public void UpdateManaDisplay(int spellsLeft, int spellsPerTap, int manaSize)
    {
        if (DisplayMana != null)
            DisplayMana.SetText(spellsLeft / spellsPerTap + " / " + manaSize / spellsPerTap);
    }
}
