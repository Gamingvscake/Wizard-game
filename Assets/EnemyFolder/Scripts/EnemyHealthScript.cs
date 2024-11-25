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
    public WeaponSwapControl wsc;
    CauldronScript cs;
    public EnemySpawnScript enemySpawn;
    public EnemyMovementScript thisMovement;
    public enum DamageResistance
    {
        NONE,
        Neutral,
        Fire,
        Water,
        Earth,
        Air,
        Light,
        Dark,
        Ice
    }
    public enum DamageWeakness
    {
        NONE,
        Neutral,
        Fire,
        Water,
        Earth,
        Air,
        Light,
        Dark,
        Ice
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
            wsc.points += 1000;
            if (enemySpawn != null) enemySpawn.amountOfEnemies -= 1;
            if (thisMovement != null)
            {
                if (thisMovement.tempturrscrip != null)
                {
                    if (thisMovement.tempturrscrip.hasenemy > 0)
                    {
                        thisMovement.tempturrscrip.targetEnemy.Remove(this.gameObject);
                        thisMovement.tempturrscrip.hasenemy -= 1;
                    }
                }
            }
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "PlayerAttack" || collision.collider.tag == "EnvironmentAttack")
        {
            DamageScript temp = collision.gameObject.GetComponent<DamageScript>();
            DODAMAGE(temp);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerAttack" || other.tag == "EnvironmentAttack")
        {
            DamageScript temp = other.gameObject.GetComponent<DamageScript>();
            DODAMAGE(temp);
        }
    }
    public void DODAMAGE(DamageScript temp)
    {
        wsc = temp.dswsc;
        cs = temp.cs;
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
