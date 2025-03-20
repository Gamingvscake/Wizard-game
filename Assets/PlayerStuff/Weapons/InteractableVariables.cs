using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableVariables : MonoBehaviour
{
    public int Cost;
    public GameObject BuyableObject;
    public bool HealthUp;
    public bool SpeedUp;
    public bool WeaponUp;
    public bool ManaUp;
    public bool RevivePotion;
    public TMP_Text text;
    private void Start()
    {
        if (text != null) text.text = Cost.ToString();
    }
}
