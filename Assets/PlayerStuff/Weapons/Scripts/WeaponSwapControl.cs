using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public int points;
    public CauldronScript wsccs;
    public PlayerInflicts wscPI;
    public EnemySpawnScript ESS;
    public EnemyHealthScript EHS;
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
            tempobject.GetComponent<SpellController>().animator = animator;
            if (wscSE != null)wscSE.currentStaffEquipped = tempobject.GetComponent<SpellController>();
        }

        inputActions = new PlayerController();
        inputActions.PlayerControls.Enable();

        playerID.ToString();
        //ScoreboardKills.text = "0";
    }

    private void Update()
    {
        StaffSwap();
        DisplayPoints.SetText(points.ToString());
       
        ScoreboardPoints.SetText(points.ToString());



        ScoreboardKills.text = EnemyHealthScript.playerKillCount["Player" + playerID].ToString();
        
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
        // Subscribe to the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the scene loaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // This method is called when a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the scene is the "MainMenu" or other scenes that indicate the start of a new game/round
        if (scene.name == "MainMenu" || scene.name == "GameScene") // Replace "GameScene" with your actual game scene name
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
}
