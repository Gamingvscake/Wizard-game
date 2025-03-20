using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMoveScript : MonoBehaviour
{
    public GameObject spawnSpell;
    public SphereCollider detectionRadius;
    public float maxDetectionRadius;
    public float fireRate;
    public Transform attackPoint;
    public float castForce, upwardForce;
    public List<GameObject> targetEnemy;
    public DamageScript damageScript;
    float temptimer;
    public int hasenemy;
    Quaternion attackpointrotation;
    Vector3 attackpointposition;
    public bool die;
    public WeaponSwapControl TMSWSC;


    public AudioSource shootingNoise;
    private void Start()
    {
        detectionRadius.radius = maxDetectionRadius;
    }
    private void Update()
    {
        if (die)
        {
            TMSWSC.numberOfTurrets -= 1;
            die = false;
        }
        if (targetEnemy != null && hasenemy > 0)
        {
            Fire(0);
        }
    }
    private void Fire(int i)
    {
        transform.LookAt(targetEnemy[i].transform.position);
        attackpointrotation = attackPoint.rotation;
        attackpointposition = attackPoint.position;
        if (temptimer < fireRate) temptimer += Time.deltaTime;
        if (temptimer >= fireRate && targetEnemy != null)
        {
            Vector3 fwd = attackPoint.transform.TransformDirection(Vector3.forward);
            RaycastHit hit;
            Vector3 targetPoint;
            if (Physics.Raycast(attackPoint.transform.position, fwd, out hit, maxDetectionRadius))
                targetPoint = hit.point;
            else targetPoint = fwd;

            Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
            GameObject currentSpell = Instantiate(spawnSpell, attackpointposition, attackpointrotation);
            currentSpell.GetComponentInChildren<DamageScript>().cs = damageScript.cs;
            currentSpell.GetComponentInChildren<DamageScript>().dswsc = damageScript.dswsc;
            currentSpell.GetComponentInChildren<DamageScript>().dsPI = damageScript.dsPI;
            currentSpell.transform.forward = directionWithoutSpread.normalized;

            shootingNoise.Play();
            currentSpell.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.None;
            currentSpell.GetComponentInChildren<Rigidbody>().AddForce(directionWithoutSpread.normalized * castForce, ForceMode.Impulse);
            currentSpell.GetComponentInChildren<Rigidbody>().AddForce(attackPoint.transform.up * upwardForce, ForceMode.Impulse);
            temptimer = 0;
        }
    }
}
