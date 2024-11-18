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
    GameObject targetEnemy;
    public DamageScript damageScript;
    float temptimer;
    private void Start()
    {
        detectionRadius.radius = maxDetectionRadius;
    }
    private void Update()
    {
        if (targetEnemy != null)
        {
            transform.parent.LookAt(targetEnemy.transform.position);
            print("adf");
        }        
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
            GameObject currentSpell = Instantiate(spawnSpell, attackPoint.position, attackPoint.rotation);
            currentSpell.GetComponentInChildren<DamageScript>().cs = damageScript.cs;
            currentSpell.GetComponentInChildren<DamageScript>().dswsc = damageScript.dswsc;
            currentSpell.GetComponentInChildren<DamageScript>().dsPI = damageScript.dsPI;
            currentSpell.transform.forward = directionWithoutSpread.normalized;

            currentSpell.GetComponentInChildren<Rigidbody>().AddForce(directionWithoutSpread.normalized * castForce, ForceMode.Impulse);
            currentSpell.GetComponentInChildren<Rigidbody>().AddForce(attackPoint.transform.up * upwardForce, ForceMode.Impulse);
            temptimer = 0;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            targetEnemy = collision.gameObject;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            targetEnemy = other.gameObject;
        }
    }
}
