using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interactables : MonoBehaviour
{
    public bool InInteractableRange;
    public bool Buyable, Removeable;
    public float timer, Maxtimer;
    public InteractableVariables InterVaris;
    public WeaponSwapControl iwsc;
    public TextMeshProUGUI txt;
    public Slider slider;
    bool CanBuy;
    bool HasBought;
    private void Start()
    {
        timer = Maxtimer;
        slider.maxValue = Maxtimer;
        slider.value = Maxtimer;
        slider.gameObject.SetActive(false);
        txt.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (InInteractableRange)
        {
            if (InterVaris.BuyableObject != null)
            {
                txt.text = "Buy " + InterVaris.BuyableObject.name + " for " + InterVaris.Cost;
                txt.gameObject.SetActive(true);
            }
            else
            {
                txt.text = "Buy barrier for " + InterVaris.Cost;
                txt.gameObject.SetActive(true);
            }
        }
        else
        {
            txt.gameObject.SetActive(false);
        }
        if (InInteractableRange && Input.GetKey(KeyCode.E))
        {
            if (InterVaris != null && InterVaris.Cost <= iwsc.points)
            {
                timer -= Time.deltaTime;
                slider.value = timer;
                if (Removeable)
                {
                    if (timer <= 0)
                    {
                        iwsc.points -= InterVaris.Cost;
                        Destroy(InterVaris.gameObject);
                        InterVaris = null;
                        timer = Maxtimer;
                        InInteractableRange = false;
                        Removeable = false;
                        slider.value = Maxtimer;
                    }
                }
                if (Buyable)
                {
                    if (timer <= 0)
                    {
                        CanBuy = true;
                        for (int e = 0; e < iwsc.EquippedStaffs.Length; e++)
                        {
                            if (iwsc.EquippedStaffs[e] == InterVaris.BuyableObject) CanBuy = false;
                        }
                        if (CanBuy)
                        {
                            iwsc.points -= InterVaris.Cost;
                            for (int i = 0; i < iwsc.EquippedStaffs.Length; i++)
                            {
                                if (iwsc.EquippedStaffs[i] == null)
                                {
                                    iwsc.EquippedStaffs[i] = InterVaris.BuyableObject;
                                    i = iwsc.EquippedStaffs.Length;
                                    HasBought = true;
                                }
                            }
                            if (HasBought == false)
                            {
                                iwsc.EquippedStaffs[iwsc.tempstaff] = InterVaris.BuyableObject;
                                iwsc.CurrentEquippedStaff = InterVaris.BuyableObject;
                                Destroy(iwsc.tempobject);
                                iwsc.tempobject = Instantiate(iwsc.CurrentEquippedStaff, iwsc.StaffSpawnPoint);
                                iwsc.tempobject.GetComponent<SpellController>().enabled = true;
                                HasBought = true;
                            }
                            InterVaris = null;
                            timer = Maxtimer;
                            InInteractableRange = false;
                            HasBought = false;
                            slider.value = Maxtimer;
                        }
                    }
                }
            }
            else print("Too Broke");
        }
        else if (InInteractableRange && Input.GetKeyUp(KeyCode.E))
        {
            timer = Maxtimer;
            CanBuy = false;
            slider.value = Maxtimer;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Removeable")
        {
            InInteractableRange = true;
            InterVaris = other.GetComponent<InteractableVariables>();
            Removeable = true;
            slider.gameObject.SetActive(true);
        }
        if (other.tag == "Buyable")
        {
            InInteractableRange = true;
            InterVaris = other.GetComponent<InteractableVariables>();
            Buyable = true;
            slider.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Removeable")
        {
            InInteractableRange = false;
            timer = Maxtimer;
            InterVaris = null;
            Removeable = false;
            slider.gameObject.SetActive(false);
        }
        if (other.tag == "Buyable")
        {
            InInteractableRange = false;
            timer = Maxtimer;
            InterVaris = null;
            Buyable = false;
            CanBuy = false;
            HasBought = false;
            slider.gameObject.SetActive(false);
        }
    }
}
