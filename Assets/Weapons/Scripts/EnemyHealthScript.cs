using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthScript : MonoBehaviour
{
    public int MaxHealth;
    public int Health;
    public Slider HealthSlider;
    public DamageResistance damageRes;
    public DamageWeakness damageWeak;
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
    }
    private void Update()
    {
        HealthSlider.value = Health;
        if (Health <= 0)
        {
            Health = MaxHealth;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "PlayerAttack" || collision.collider.tag == "EnvironmentAttack")
        {
            DamageScript temp = collision.gameObject.GetComponent<DamageScript>();
            if ((int)temp.damageType == (int)damageRes)
            {
                Health -= (temp.Damage / 2);
            }
            else if ((int)temp.damageType == (int)damageWeak)
            {
                Health -= (temp.Damage * 2);
            }
            else
            {
                Health -= temp.Damage;
            }
        }
    }
}
