using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public TextMeshProUGUI ScoreboardPoints;
    public TextMeshProUGUI ScoreboardKills;
    public TextMeshProUGUI ScoreboardRevives;
    public int points;
    public CauldronScript wsccs;
    public PlayerInflicts wscPI;
    public EnemySpawnScript ESS;
    public EnemyHealthScript EHS;
    public ReviveScript revive;
    public AnvilScript wscas;
    public StatusEffects wscSE;
    public int maxNumberOfTurrets;
    public int numberOfTurrets;
    [SerializeField] private AudioSource changeweapon;
    public int playerID;
    
    public MovementController movementController;
    public Animator animator;
    private PlayerController inputActions;
    private bool canSwitchStaff = true;
    public bool inTutorialScene;
    public TMP_Text[] reviveTexts;
    private static Dictionary<int, int> reviveCounts = new Dictionary<int, int>();

    private void Start()
    {
        Mana = MaxMana;
        EquippedStaffs = new GameObject[MaxNumberOfStaffs];
        TempStaffs = new GameObject[MaxNumberOfStaffs];
        EquippedStaffs[0] = StarterStaff;
        if (inTutorialScene == false) 
        {
            ESS = GameObject.Find("Spawning").GetComponent<EnemySpawnScript>();
            ESS.Players.Add(this.transform);
        }
        movementController = GetComponent<MovementController>();

        if (tempobject == null)
        {
            CurrentEquippedStaff = EquippedStaffs[0];
            tempobject = Instantiate(CurrentEquippedStaff, StaffSpawnPoint);
            tempobject.GetComponent<SpellController>().enabled = true;
            tempobject.GetComponent<SpellController>().movementController = movementController;
            tempobject.GetComponent<SpellController>().animator = animator;
            if (wscSE != null)wscSE.currentStaffEquipped = tempobject.GetComponent<SpellController>();
        }

        inputActions = new PlayerController();
        inputActions.PlayerControls.Enable();
        playerID.ToString();

        /*int revivingPlayerID = playerID;

        if (!reviveCounts.ContainsKey(revivingPlayerID))
        {
            reviveCounts[revivingPlayerID] = 0;
        }

        reviveCounts[revivingPlayerID]++;
        print("Player " + revivingPlayerID + " has revived another player " + reviveCounts[revivingPlayerID] + " times.");*/

    }

    private void Update()
    {
        StaffSwap();
        DisplayPoints.SetText(points.ToString());
       
        if (ScoreboardPoints != null)ScoreboardPoints.SetText(points.ToString());



        if (ScoreboardKills != null)
        {
            ScoreboardKills.text = EnemyHealthScript.playerKillCount["Player" + playerID].ToString();
        }

        if (ScoreboardRevives != null)
        {
            if (reviveCounts.ContainsKey(playerID)) // Check if key exists
            {
                ScoreboardRevives.SetText(reviveCounts[playerID].ToString());
            }
            else
            {
                // If the playerID does not exist, set the revive count to 0
                reviveCounts[playerID] = 0; // Initialize the value for this player
                ScoreboardRevives.SetText("0"); // Set the scoreboard to show 0 revives
            }
        }

    }

    private void StaffSwap()
    {
        float switchStaffValue = 0;
        if (!movementController.DevKeyboardOn)
        {
             if (inputActions != null)switchStaffValue = inputActions.PlayerControls.SwitchStaff.ReadValue<float>();
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
                    tempobject.GetComponent<SpellController>().animator = animator;
                    wscSE.currentStaffEquipped = tempobject.GetComponent<SpellController>();
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
                    tempobject.GetComponent<SpellController>().animator = animator;
                    wscSE.currentStaffEquipped = tempobject.GetComponent<SpellController>();
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
        tempobject.GetComponent<SpellController>().movementController = movementController;
        tempobject.GetComponent<SpellController>().animator = animator;
        wscSE.currentStaffEquipped = tempobject.GetComponent<SpellController>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu" || scene.name == "Tavern") 
        {
            ResetKillCount(); // Reset kill count when the game starts
        }
        
    }

    public static void ResetKillCount()
    {
        // Reset kill count for all players to 0
        EnemyHealthScript.playerKillCount["Player1"] = 0;
        EnemyHealthScript.playerKillCount["Player2"] = 0;
        EnemyHealthScript.playerKillCount["Player3"] = 0;
        EnemyHealthScript.playerKillCount["Player4"] = 0;

        
    }

    private void UpdateReviveText(int playerID)
    {
        // Make sure the playerID is valid and that the text array is initialized
        if (playerID >= 1 && playerID <= reviveTexts.Length)
        {
            reviveTexts[playerID - 1].text = "Revives: " + reviveCounts[playerID];
        }
    }

    public static int GetReviveCount(int playerID)
    {
        return reviveCounts.ContainsKey(playerID) ? reviveCounts[playerID] : 0;
    }
}
