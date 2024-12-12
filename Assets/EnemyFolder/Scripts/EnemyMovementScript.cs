using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public bool DevBoolToNotMove;
    public NavMeshAgent selfNavAgent;

    private void Start()
    {
        PlayerNotList = new Transform[Players.Count];
        AttackBox.SetActive(false);
        animator = GetComponent<Animator>();
        if (!DevBoolToNotMove)
        {
            selfNavAgent = GetComponent<NavMeshAgent>();
            selfNavAgent.speed = speed;
        }
        for (int i = 0; i < Players.Count; i++)
        {
            PlayerNotList[i] = Players[i];
        }
    }
    private void Update()
    {
        Transform temp = GetClosestEnemy(PlayerNotList);
        //transform.LookAt(temp);
        selfNavAgent.destination = temp.position;

        if (selfNavAgent != null && OutOfBounds && DevBoolToNotMove == false)
        {
            //Transform temp = GetClosestEnemy(entryPoints);
            //transform.LookAt(temp);
            transform.position = Vector3.MoveTowards(transform.position, temp.position, speed * Time.deltaTime);
        }
        else if (selfNavAgent != null && !OutOfBounds && !DMGScript.Attacking && DevBoolToNotMove == false)
        {
            //Transform temp = GetClosestEnemy(PlayerNotList);
            //transform.LookAt(temp);
                        transform.position = Vector3.MoveTowards(transform.position, temp.position, speed * Time.deltaTime);
            selfNavAgent.destination = temp.position;
            if (Vector3.Distance(transform.position, temp.position) <= attackDistance)
            {
                AttackBox.SetActive(true);
            }
            else AttackBox.SetActive(false);
        }

        //TESTING NAVMESH

        if (!DMGScript.Attacking && DevBoolToNotMove == false)
        {

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
