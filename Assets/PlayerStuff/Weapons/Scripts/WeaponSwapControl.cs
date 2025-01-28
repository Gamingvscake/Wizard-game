using System.Collections;
using TMPro;
using UnityEngine;

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
    public AnvilScript wscas;
    public int maxNumberOfTurrets;
    public int numberOfTurrets;
    [SerializeField] private AudioSource changeweapon;

    public MovementController movementController;
    private PlayerController inputActions;
    private bool canSwitchStaff = true;

    private void Start()
    {
        Mana = MaxMana;
        EquippedStaffs = new GameObject[MaxNumberOfStaffs];
        TempStaffs = new GameObject[MaxNumberOfStaffs];
        EquippedStaffs[0] = StarterStaff;
        ESS = GameObject.Find("Spawning").GetComponent<EnemySpawnScript>();
        ESS.Players.Add(this.transform);
        movementController = GetComponent<MovementController>();

        if (tempobject == null)
        {
            CurrentEquippedStaff = EquippedStaffs[0];
            tempobject = Instantiate(CurrentEquippedStaff, StaffSpawnPoint);
            tempobject.GetComponent<SpellController>().enabled = true;
            tempobject.GetComponent<SpellController>().movementController = movementController;
        }

        inputActions = new PlayerController();
        inputActions.PlayerControls.Enable();
    }

    private void Update()
    {
        StaffSwap();
        DisplayPoints.SetText(points.ToString());
        //print(Mana);
    }

    private void StaffSwap()
    {
        float switchStaffValue = 0;
        if (!movementController.DevKeyboardOn)
        {
             switchStaffValue = inputActions.PlayerControls.SwitchStaff.ReadValue<float>();
        }
        else 
        { 
             switchStaffValue = Input.mouseScrollDelta.y; 
        }
        if (switchStaffValue > 0.5f && canSwitchStaff)
        {
            canSwitchStaff = false; // Prevent continuous swapping
            if (tempstaff < MaxNumberOfStaffs - 1 && EquippedStaffs[tempstaff + 1] != null)
            {
                changeweapon.Play();
                Destroy(tempobject);
                tempstaff++;
                CurrentEquippedStaff = EquippedStaffs[tempstaff];

                if (CurrentEquippedStaff != null)
                {
                    tempobject = Instantiate(CurrentEquippedStaff, StaffSpawnPoint);
                    tempobject.GetComponent<SpellController>().enabled = true;
                    tempobject.GetComponent<SpellController>().movementController = movementController;
                }
            }
            else
            {
                tempstaff = 0; // Wrap around to the first staff
                changeweapon.Play();
                Destroy(tempobject);
                CurrentEquippedStaff = EquippedStaffs[tempstaff];

                if (CurrentEquippedStaff != null)
                {
                    tempobject = Instantiate(CurrentEquippedStaff, StaffSpawnPoint);
                    tempobject.GetComponent<SpellController>().enabled = true;
                    tempobject.GetComponent<SpellController>().movementController = movementController;
                }
            }
        }
        else if (switchStaffValue <= 0.5f)
        {
            canSwitchStaff = true; // Allow swapping again once released
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
        for (int i = 0; i < TempStaffs.Length; i++)
        {
            EquippedStaffs[i] = TempStaffs[i];
        }
        TempStaffs = new GameObject[MaxNumberOfStaffs];
    }

    public void UpgradeStaff()
    {
        Destroy(tempobject);
        EquippedStaffs[tempstaff] = tempobject.GetComponent<SpellController>().upgradedStaff;
        CurrentEquippedStaff = EquippedStaffs[tempstaff];

        tempobject = Instantiate(CurrentEquippedStaff, StaffSpawnPoint);
        tempobject.GetComponent<SpellController>().enabled = true;
    }
}
