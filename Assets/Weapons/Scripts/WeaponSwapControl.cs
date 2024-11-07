using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class WeaponSwapControl : MonoBehaviour
{
    public int MaxNumberOfStaffs;
    public Transform StaffSpawnPoint;
    public GameObject[] EquippedStaffs;
    public GameObject[] TempStaffs;
    public GameObject CurrentEquippedStaff;
    public GameObject StarterStaff;
    public int tempstaff;
    public GameObject tempobject;
    public int Mana;
    public int MaxMana;
    public TextMeshProUGUI DisplayMana;
    public TextMeshProUGUI DisplayPoints;
    public int points;
    public CauldronScript wsccs;
    public PlayerInflicts wscPI;
    public EnemySpawnScript ESS;
    private void Start()
    {
        Mana = MaxMana;
        EquippedStaffs = new GameObject[MaxNumberOfStaffs];
        TempStaffs = new GameObject[MaxNumberOfStaffs];
        EquippedStaffs[0] = StarterStaff;
        ESS = GameObject.Find("Spawning").GetComponent<EnemySpawnScript>();
        ESS.Players.Add(this.transform);
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
        DisplayPoints.SetText(points.ToString());
        //print(Mana);
    }
    private void StaffSwap()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            if (EquippedStaffs.Length != 1 || EquippedStaffs[tempstaff += 1] != null)
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
                    tempstaff = EquippedStaffs.Length;
                }
            }
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            if (EquippedStaffs.Length != 1 || EquippedStaffs[tempstaff -=1] != null)
            {
                Destroy(tempobject);
                if (tempstaff != 0)
                {
                    tempstaff--;
                }
                CurrentEquippedStaff = EquippedStaffs[tempstaff];
                if (CurrentEquippedStaff != null)
                {
                    tempobject = Instantiate(CurrentEquippedStaff, StaffSpawnPoint);
                    tempobject.GetComponent<SpellController>().enabled = true;
                }
            }
        }
    }
    public void UpdateManaDisplay(int spellsLeft, int spellsPerTap, int manaSize)
    {
        if (DisplayMana != null)
            DisplayMana.SetText(spellsLeft / spellsPerTap + " / " + manaSize / spellsPerTap);
    }
    public void UpdateStaffArray()
    {
        for (int i = 0; i < EquippedStaffs.Length; i++)
        {
            TempStaffs[i] = EquippedStaffs[i];
        }
        EquippedStaffs = new GameObject[MaxNumberOfStaffs];
        for (int i = 0;i < TempStaffs.Length; i++)
        {
            EquippedStaffs[i] = TempStaffs[i];
        }
        TempStaffs = new GameObject[MaxNumberOfStaffs];
    }
}
