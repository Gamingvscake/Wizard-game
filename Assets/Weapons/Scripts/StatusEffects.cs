using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffects : MonoBehaviour
{
    public Status effects;
    public PlayerInflicts StatusPI;
    public enum Status
    {
        None,
        NeutralTBD,
        Burn,
        Poison,
        EarthTBD,
        AirTBD,
        IncreaseManaCost,
        DarkTBD,
        Slow
    }
    void Update()
    {
        
    }
    public void DoStatusWork(int statustemp)
    {
        if (statustemp == (int)Status.Burn)
        {
            StatusPI.TakingDrainingDamage = true;
        }
    }
}
