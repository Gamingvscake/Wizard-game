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
    public EnemyMovementScript bossMovement;
    public Renderer MyRenderer;
    public Material outlineMat;
    public bool DevBoolToNotMove;
    public DamageSource EHSDS;
    public GameObject[] statusIconsHold;
    public GameObject[] statusIconLocations;
    public List<GameObject> statusIconsInUse;
    float temptimer;
    float statusdonetemptimer;
    float temporarytimer;
    float tempspeed;
    float tempfirerate;

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
        if (DevBoolToNotMove == false)
        {
            MaxHealth *= 1 + (enemySpawn.rounds / 5);
            outlineMat = MyRenderer.materials[1];
            outlineMat.SetColor("_OutlineColor", new Color32(0, 0, 0, 255));
            int tempstatusint = Random.Range(0, 9);
            switch (tempstatusint)
            {
                case 0:
                    damageRes = DamageResistance.NONE;
                    damageWeak = DamageWeakness.NONE;
                    EHSDS.effects = DamageSource.HostileStatus.None;
                    outlineMat.SetColor("_OutlineColor", new Color32(109, 109, 109, 255));
                    break;
                case 1:
                    damageRes = DamageResistance.Neutral;
                    damageWeak = DamageWeakness.Ice;
                    EHSDS.effects = DamageSource.HostileStatus.Neutral;
                    outlineMat.SetColor("_OutlineColor", new Color32(110, 110, 110, 255));
                    break;
                case 2:
                    damageRes = DamageResistance.Fire;
                    damageWeak = DamageWeakness.Water;
                    EHSDS.effects = DamageSource.HostileStatus.Burn;
                    outlineMat.SetColor("_OutlineColor", new Color32(255, 0, 0, 255));
                    break;
                case 3:
                    damageRes = DamageResistance.Water;
                    damageWeak = DamageWeakness.Earth;
                    EHSDS.effects = DamageSource.HostileStatus.Poison;
                    outlineMat.SetColor("_OutlineColor", new Color32(0, 0, 255, 255));
                    break;
                case 4:
                    damageRes = DamageResistance.Earth;
                    damageWeak = DamageWeakness.Air;
                    EHSDS.effects = DamageSource.HostileStatus.EarthTBD;
                    outlineMat.SetColor("_OutlineColor", new Color32(170, 100, 0, 255));
                    break;
                case 5:
                    damageRes = DamageResistance.Air;
                    damageWeak = DamageWeakness.Fire;
                    EHSDS.effects = DamageSource.HostileStatus.FireRateLower;
                    outlineMat.SetColor("_OutlineColor", new Color32(180, 255, 150, 255));
                    break;
                case 6:
                    damageRes = DamageResistance.Light;
                    damageWeak = DamageWeakness.Dark;
                    EHSDS.effects = DamageSource.HostileStatus.ManaDrain;
                    outlineMat.SetColor("_OutlineColor", new Color32(255, 255, 255, 255));
                    break;
                case 7:
                    damageRes = DamageResistance.Dark;
                    damageWeak = DamageWeakness.Light;
                    EHSDS.effects = DamageSource.HostileStatus.DarkTBD;
                    outlineMat.SetColor("_OutlineColor", new Color32(0, 0, 0, 255));
                    break;
                case 8:
                    damageRes = DamageResistance.Ice;
                    damageWeak = DamageWeakness.Neutral;
                    EHSDS.effects = DamageSource.HostileStatus.Slow;
                    outlineMat.SetColor("_OutlineColor", new Color32(150, 230, 255, 255));
                    break;
            }
            for (int i = 0; i < statusIconsHold.Length; i++)
            {
                statusIconsHold[i].GetComponentInChildren<Slider>().value = 0;
                statusIconsHold[i].gameObject.SetActive(false);
            }
            tempspeed = thisMovement.speed;
            thisMovement.speed = tempspeed + (enemySpawn.rounds / 5);
            tempfirerate = thisMovement.rangedFireRate;
        }
        HealthSlider.maxValue = MaxHealth;
        Health = MaxHealth;
    }
    private void Update()
    {
        HealthSlider.value = Health;
        if (temptimer > 0) temptimer -=Time.deltaTime;
        if (statusdonetemptimer > 0) statusdonetemptimer -= Time.deltaTime;
        if (Health <= 0)
        {
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
            if (thisMovement.isBoss)
            {
                enemySpawn.BossDefeated = true;
                enemySpawn.amountOfEnemies -= thisMovement.amountSpawned;
            }
            if (thisMovement.isMinion) bossMovement.amountSpawned -=1;
            Destroy(this.gameObject);
        }
        if (statusIconsInUse.Count > 0)
        {
            for (int i = 0; i < statusIconsInUse.Count; i++)
            {
                statusIconsInUse[i].transform.position = statusIconLocations[i].transform.position;
            }
        }
        if (temptimer <= 0)
        {
            if (statusIconsInUse.Count > 0)
            {
                for (int i = 0;i < statusIconsInUse.Count; i++)
                {
                    statusIconsHold[i].GetComponentInChildren<Slider>().value = 0;
                    statusIconsInUse.Remove(statusIconsInUse[i]);
                    statusIconsHold[i].SetActive(false);
                }
            }
        }
        if (statusdonetemptimer > 0)
        {
            if (temporarytimer <= 0)
            {
                if (statusIconsHold[0].GetComponentInChildren<Slider>().value == statusIconsHold[0].GetComponentInChildren<Slider>().maxValue) 
                {
                    //if (damageRes == DamageResistance.Fire) Health -= (MaxHealth / 10) / 2;
                    if (damageRes != DamageResistance.Fire) Health -= MaxHealth / 10;
                }
                temporarytimer = 1;
            }
            if (temporarytimer > 0) temporarytimer -= Time.deltaTime;
            if (statusIconsHold[2].GetComponentInChildren<Slider>().value == statusIconsHold[2].GetComponentInChildren<Slider>().maxValue)
            {
                //if (damageRes == DamageResistance.Water) Health -= (int)(Time.deltaTime * 50) / 2;
                if (damageRes != DamageResistance.Water) Health -= (int)(Time.deltaTime * 50);
            }
            if (statusIconsHold[1].GetComponentInChildren<Slider>().value > 0)
            {
                //if (damageRes == DamageResistance.Ice) thisMovement.speed = tempspeed - (statusIconsHold[1].GetComponentInChildren<Slider>().value / 100) / 2;
                if (damageRes != DamageResistance.Ice) thisMovement.speed = tempspeed - (statusIconsHold[1].GetComponentInChildren<Slider>().value / 100);
            }
            if (statusIconsHold[3].GetComponentInChildren<Slider>().value > 0)
            {
                if (damageRes != DamageResistance.Air) thisMovement.rangedFireRate = tempfirerate * 2;
            }
        }
        else
        {
            if (thisMovement.rangedFireRate != tempfirerate) thisMovement.rangedFireRate = tempfirerate;
            for (int i = 0; i < statusIconsInUse.Count; i++)
            {
                if (statusIconsInUse[i].GetComponentInChildren<Slider>().value == statusIconsInUse[i].GetComponentInChildren<Slider>().maxValue)
                {
                    statusIconsInUse.Remove(statusIconsInUse[i]);
                }
            }
        }
    }
    /*
    Damage type to make status effect for:
        - Neutral,
        - Earth,
        - Air,
        - Light,
        - Dark,
*/
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
        temptimer = 10;
        if ((int)temp.damageType == (int)damageRes)
        {
            Health -= ((temp.Damage + cs.Damage2) / 2);
            wsc.points += 10;
        }
        else if ((int)temp.damageType == (int)damageWeak)
        {
            Health -= ((temp.Damage + cs.Damage2) * 2);
            wsc.points += 10;
        }
        else
        {
            Health -= (temp.Damage + cs.Damage2);
            wsc.points += 10;
        }
        if (DevBoolToNotMove == false)
        {
            if (temp.damageType == DamageScript.DamageType.Fire)
            {
                if (statusIconsInUse.Contains(statusIconsHold[0]) == false)
                {
                    statusIconsInUse.Add(statusIconsHold[0]);
                    statusIconsHold[0].SetActive(true);
                    statusIconsHold[0].GetComponentInChildren<Slider>().value += 10;
                }
                else
                {
                    statusIconsHold[0].GetComponentInChildren<Slider>().value += 10;
                    if (statusIconsHold[0].GetComponentInChildren<Slider>().value == statusIconsHold[0].GetComponentInChildren<Slider>().maxValue)
                    {
                        print("kaboom");
                        if (statusdonetemptimer >= 0) statusdonetemptimer = 10;
                    }
                }
            }
            if (temp.damageType == DamageScript.DamageType.Ice)
            {
                if (statusIconsInUse.Contains(statusIconsHold[1]) == false)
                {
                    statusIconsInUse.Add(statusIconsHold[1]);
                    statusIconsHold[1].SetActive(true);
                    statusIconsHold[1].GetComponentInChildren<Slider>().value += 10;
                }
                else
                {
                    statusIconsHold[1].GetComponentInChildren<Slider>().value += 10;
                    if (statusIconsHold[1].GetComponentInChildren<Slider>().value == statusIconsHold[1].GetComponentInChildren<Slider>().maxValue)
                    {
                        print("FREEZE");
                        if (statusdonetemptimer >= 0) statusdonetemptimer = 10;
                    }
                }
            }
            if (temp.damageType == DamageScript.DamageType.Water)
            {
                if (statusIconsInUse.Contains(statusIconsHold[2]) == false)
                {
                    statusIconsInUse.Add(statusIconsHold[2]);
                    statusIconsHold[2].SetActive(true);
                    statusIconsHold[2].GetComponentInChildren<Slider>().value += 10;
                }
                else
                {
                    statusIconsHold[2].GetComponentInChildren<Slider>().value += 10;
                    statusdonetemptimer = 5;
                    if (statusIconsHold[2].GetComponentInChildren<Slider>().value == statusIconsHold[2].GetComponentInChildren<Slider>().maxValue)
                    {
                        print("Blub");
                        if (statusdonetemptimer >= 0) statusdonetemptimer = 10;
                    }
                }
            }
            if (temp.damageType == DamageScript.DamageType.Air)
            {
                if (statusIconsInUse.Contains(statusIconsHold[3]) == false)
                {
                    statusIconsInUse.Add(statusIconsHold[3]);
                    statusIconsHold[3].SetActive(true);
                    statusIconsHold[3].GetComponentInChildren<Slider>().value += 10;
                }
                else
                {
                    statusIconsHold[3].GetComponentInChildren<Slider>().value += 10;
                    statusdonetemptimer = 5;
                    if (statusIconsHold[3].GetComponentInChildren<Slider>().value == statusIconsHold[3].GetComponentInChildren<Slider>().maxValue)
                    {
                        print("Blub");
                        if (statusdonetemptimer >= 0) statusdonetemptimer = 10;
                    }
                }
            }
        }
    }
}
