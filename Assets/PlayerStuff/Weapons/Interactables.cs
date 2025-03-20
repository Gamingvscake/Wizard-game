using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class Interactables : MonoBehaviour
{
    public bool InInteractableRange;
    public bool Buyable, Removeable, CauldronBuyable;
    public float timer, Maxtimer;
    public InteractableVariables InterVaris;
    public WeaponSwapControl iwsc;
    public PlayerInflicts playerInflicts;
    public MovementController movementController;
    public TextMeshProUGUI txt;
    public Slider slider;
    private bool CanBuy;
    private bool HasBought;
    public GameObject[] cauldronEffectIconsHold;
    public GameObject[] cauldronEffectIconLocations;
    public List<GameObject> cauldronEffectsGot;
    bool healthbought;
    bool speedbought;
    bool weaponbought;
    bool manabought;

    private PlayerController controls;

    private void Awake()
    {
        // Initialize the input system
        controls = new PlayerController();

        // Subscribe to the Interact action
        controls.PlayerControls.Interact.started += ctx => StartInteraction();
        controls.PlayerControls.Interact.canceled += ctx => StopInteraction();
    }

    private void Start()
    {
        timer = Maxtimer;
        slider.maxValue = Maxtimer;
        slider.value = Maxtimer;
        slider.gameObject.SetActive(false);
        txt.transform.parent.gameObject.SetActive(false);
        for (int i = 0; i < cauldronEffectIconsHold.Length; i++)
        {
            cauldronEffectIconsHold[i].SetActive(false);
        }
        playerInflicts = GetComponentInParent<PlayerInflicts>();
        movementController = GetComponentInParent<MovementController>();
    }

    private void Update()
    {
        if (InInteractableRange)
        {
            if (InterVaris.BuyableObject != null)
            {
                if (InterVaris.HealthUp || InterVaris.ManaUp || InterVaris.WeaponUp || InterVaris.SpeedUp)
                {
                    txt.text = "Buy " + InterVaris.gameObject.name + " for " + InterVaris.Cost;
                    txt.transform.parent.gameObject.SetActive(true);
                }
                else
                {
                    txt.text = "Buy " + InterVaris.BuyableObject.name + " for " + InterVaris.Cost;
                    txt.transform.parent.gameObject.SetActive(true);
                }
            }
            else
            {
                txt.text = "Buy barrier for " + InterVaris.Cost;
                txt.transform.parent.gameObject.SetActive(true);
            }
        }
        else
        {
            txt.transform.parent.gameObject.SetActive(false);
        }

        if (InInteractableRange && slider.gameObject.activeSelf)
        {
            timer -= Time.deltaTime;
            slider.value = timer;

            if (timer <= 0)
            {
                if (Removeable)
                {
                    iwsc.points -= InterVaris.Cost;
                    Destroy(InterVaris.gameObject);
                    ResetInteractState();
                }
                else if (Buyable)
                {
                    CanBuy = true;
                    for (int e = 0; e < iwsc.EquippedStaffs.Length; e++)
                    {
                        if (iwsc.EquippedStaffs[e] == InterVaris.BuyableObject)
                        {
                            CanBuy = false;
                        }
                    }

                    if (CanBuy)
                    {
                        iwsc.points -= InterVaris.Cost;

                        for (int i = 0; i < iwsc.EquippedStaffs.Length; i++)
                        {
                            if (iwsc.EquippedStaffs[i] == null)
                            {
                                iwsc.EquippedStaffs[i] = InterVaris.BuyableObject;
                                iwsc.EquippedStaffs[i].gameObject.GetComponent<SpellController>().movementController = iwsc.movementController;
                                HasBought = true;
                                break;
                            }
                        }

                        if (!HasBought)
                        {
                            iwsc.EquippedStaffs[iwsc.tempstaff] = InterVaris.BuyableObject;
                            iwsc.CurrentEquippedStaff = InterVaris.BuyableObject;
                            Destroy(iwsc.tempobject);
                            iwsc.tempobject = Instantiate(iwsc.CurrentEquippedStaff, iwsc.StaffSpawnPoint);
                            iwsc.tempobject.GetComponent<SpellController>().movementController = iwsc.movementController;
                            iwsc.tempobject.GetComponent<SpellController>().enabled = true;
                            HasBought = true;
                        }

                        ResetInteractState();
                    }
                }
                else if (CauldronBuyable)
                {

                    if (InterVaris.HealthUp && !HasBought && !healthbought)
                    {
                        playerInflicts.PlayerMaxHealth += 50;
                        AddCauldronEffect(0);
                        iwsc.points -= InterVaris.Cost;
                        healthbought = true;
                        HasBought = true;
                    }
                    else if (InterVaris.SpeedUp && !HasBought && !speedbought)
                    {
                        movementController.walkSpeed = movementController.walkSpeed += 1f;
                        movementController.sprintSpeed = movementController.sprintSpeed += 1f;
                        AddCauldronEffect(1);
                        iwsc.points -= InterVaris.Cost;
                        speedbought = true;
                        HasBought = true;
                    }
                    else if (InterVaris.WeaponUp && !HasBought && !weaponbought)
                    {
                        iwsc.MaxNumberOfStaffs += 1;
                        iwsc.UpdateStaffArray();
                        AddCauldronEffect(2);
                        iwsc.points -= InterVaris.Cost;
                        weaponbought = true;
                        HasBought = true;
                    }
                    else if (InterVaris.ManaUp && !HasBought && !manabought)
                    {
                        iwsc.MaxMana += 50;
                        AddCauldronEffect(3);
                        iwsc.points -= InterVaris.Cost;
                        manabought = true;
                        HasBought = true;
                    }
                    else if (InterVaris.RevivePotion && !HasBought && movementController.numberOfAvailablePotions != movementController.maxNumberOfPotions)
                    {
                        movementController.numberOfAvailablePotions += 1;
                        iwsc.points -= InterVaris.Cost;
                        HasBought = true;
                    }
                    ResetInteractState();
                }
            }
        }
    }

    private void StartInteraction()
    {
        if (InInteractableRange && InterVaris.Cost <= iwsc.points)
        {
            slider.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Too Broke");
        }
    }

    private void StopInteraction()
    {
        ResetInteractState();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Removeable"))
        {
            InInteractableRange = true;
            InterVaris = other.GetComponent<InteractableVariables>();
            Removeable = true;
        }

        if (other.CompareTag("Buyable"))
        {
            InInteractableRange = true;
            InterVaris = other.GetComponent<InteractableVariables>();
            Buyable = true;
        }
        if (other.CompareTag("CauldronBuyable"))
        {
            InInteractableRange = true;
            InterVaris = other.GetComponent<InteractableVariables>();
            CauldronBuyable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Removeable") || other.CompareTag("Buyable"))
        {
            ResetInteractState();
        }
    }

    private void ResetInteractState()
    {
        InInteractableRange = false;
        timer = Maxtimer;
        InterVaris = null;
        Removeable = false;
        Buyable = false;
        CanBuy = false;
        CauldronBuyable = false;
        HasBought = false;
        slider.value = Maxtimer;
        slider.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        controls.PlayerControls.Enable();
    }

    private void OnDisable()
    {
        controls.PlayerControls.Disable();
    }
    public void AddCauldronEffect(int k)
    {
        if (!cauldronEffectsGot.Contains(cauldronEffectIconsHold[k]))
        {
            cauldronEffectsGot.Add(cauldronEffectIconsHold[k]);
            cauldronEffectIconsHold[k].SetActive(true);
        }
        if (cauldronEffectsGot.Count > 0)
        {
            for (int i = 0; i < cauldronEffectsGot.Count; i++)
            {
                cauldronEffectsGot[i].transform.position = cauldronEffectIconLocations[i].transform.position;
            }
        }
    }
}