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
    public bool StaminaUp;
    public bool ManaUp;
    public TMP_Text text;
    private void Start()
    {
        if (text != null) text.text = Cost.ToString();
    }
}
