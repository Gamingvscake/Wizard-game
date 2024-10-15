using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class EnemyHealthScript : MonoBehaviour
{
    public int MaxHealth;
    public int Health;
    public Slider HealthSlider;
    public DamageResistance damageRes;
    public DamageWeakness damageWeak;
    public WeaponSwapControl wsc;
    CauldronScript cs;
    public enum DamageResistance
    {
        Neutral,
        Fire,
        Water,
        Earth,
        Air
    }
    public enum DamageWeakness
    {
        Neutral,
        Fire,
        Water,
        Earth,
        Air
    }
    private void Start()
    {
        HealthSlider.maxValue = MaxHealth;
        Health = MaxHealth;
        wsc = GameObject.Find("FirstPersonController").GetComponent<WeaponSwapControl>();
        cs = GameObject.Find("FirstPersonController").GetComponent<CauldronScript>();
    }
    private void Update()
    {
        HealthSlider.value = Health;
        if (Health <= 0)
        {
            Health = MaxHealth;
            wsc.points += 1000;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "PlayerAttack" || collision.collider.tag == "EnvironmentAttack")
        {
            DamageScript temp = collision.gameObject.GetComponent<DamageScript>();
            if ((int)temp.damageType == (int)damageRes)
            {
                Health -= ((temp.Damage+cs.Damage2) / 2);
                wsc.points += 100;
            }
            else if ((int)temp.damageType == (int)damageWeak)
            {
                Health -= ((temp.Damage + cs.Damage2) * 2);
                wsc.points += 100;
            }
            else
            {
                Health -= (temp.Damage + cs.Damage2);
                wsc.points += 100;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerAttack" || other.tag == "EnvironmentAttack")
        {
            DamageScript temp = other.gameObject.GetComponent<DamageScript>();
            if ((int)temp.damageType == (int)damageRes)
            {
                Health -= ((temp.Damage + cs.Damage2) / 2);
                wsc.points += 100;
            }
            else if ((int)temp.damageType == (int)damageWeak)
            {
                Health -= ((temp.Damage + cs.Damage2) * 2);
                wsc.points += 100;
            }
            else
            {
                Health -= (temp.Damage + cs.Damage2);
                wsc.points += 100;
            }
        }
    }
}
