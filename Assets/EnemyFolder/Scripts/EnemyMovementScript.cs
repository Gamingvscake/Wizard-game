using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementScript : MonoBehaviour
{
    public Transform[] entryPoints;
    public List<Transform> Players;
    public Transform[] PlayerNotList;
    public bool OutOfBounds = true;
    public float speed;
    public GameObject AttackBox;
    private Animator animator; // Animator component reference
    public DamageSource DMGScript;
    public TurretMoveScript tempturrscrip;
    public float attackDistance;

    private void Start()
    {
        PlayerNotList = new Transform[Players.Count];
        AttackBox.SetActive(false);
        animator = GetComponent<Animator>();
        for (int i = 0; i < Players.Count; i++)
        {
            PlayerNotList[i] = Players[i];
        }
    }
    private void Update()
    {
        if (OutOfBounds)
        {
            Transform temp = GetClosestEnemy(entryPoints);
            transform.LookAt(temp);
            transform.position = Vector3.MoveTowards(transform.position, temp.position, speed * Time.deltaTime);
        }
        else if (!OutOfBounds && !DMGScript.Attacking)
        {
            Transform temp = GetClosestEnemy(PlayerNotList);
            transform.LookAt(temp);
            transform.position = Vector3.MoveTowards(transform.position, temp.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, temp.position) <= attackDistance)
            {
                AttackBox.SetActive(true);
            }
            else AttackBox.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("EnterPoint"))
        {
            OutOfBounds = false;
            transform.position = other.GetComponent<HolderScript>().EnterPoint;
        }
        if (other.tag == "Turret")
        {
            if (tempturrscrip == null)
            {
                tempturrscrip = other.GetComponentInParent<TurretMoveScript>();
                tempturrscrip.targetEnemy.Add(this.gameObject);
                tempturrscrip.hasenemy += 1;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Turret")
        {
            if (tempturrscrip != null)
            {
                tempturrscrip.targetEnemy.Remove(this.gameObject);
                tempturrscrip.hasenemy -= 1;
                tempturrscrip = null;
            }
        }
    }

    private Transform GetClosestEnemy(Transform[] enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
}
