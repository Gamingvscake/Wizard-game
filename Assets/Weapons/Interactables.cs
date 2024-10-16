using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour
{
    public bool InInteractableRange;
    public bool Buyable, Removeable;
    public float timer, Maxtimer;
    public InteractableVariables InterVaris;
    public WeaponSwapControl iwsc;
    bool CanBuy;
    bool HasBought;
    private void Start()
    {
        timer = Maxtimer;
    }
    private void Update()
    {
        if (InInteractableRange && Input.GetKey(KeyCode.E))
        {
            if (InterVaris != null && InterVaris.Cost <= iwsc.points)
            {
                timer -= Time.deltaTime;
                if (Removeable)
                {
                    if (timer <= 0)
                    {
                        iwsc.points -= InterVaris.Cost;
                        Destroy(InterVaris.gameObject);
                        InterVaris = null;
                        timer = Maxtimer;
                        InInteractableRange = false;
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
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Removeable")
        {
            InInteractableRange = true;
            InterVaris = other.GetComponent<InteractableVariables>();
            Removeable = true;
        }
        if (other.tag == "Buyable")
        {
            InInteractableRange = true;
            InterVaris = other.GetComponent<InteractableVariables>();
            Buyable = true;
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
        }
        if (other.tag == "Buyable")
        {
            InInteractableRange = false;
            timer = Maxtimer;
            InterVaris = null;
            Buyable = false;
            CanBuy = false;
            HasBought = false;
        }
    }
}
