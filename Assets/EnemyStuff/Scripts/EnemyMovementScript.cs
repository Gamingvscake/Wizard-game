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
    public bool DevBoolToNotMove, isRanged, isBoss;
    public NavMeshAgent selfNavAgent;
    public GameObject RangedAttack;
    public float temprangedtimer, rangedFireRate;
    public Transform rangedAttackSpawn;
    public float forwardForce, upwardForce;
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
        //selfNavAgent.destination = temp.position;
        selfNavAgent.SetDestination(temp.position);
        //Debug.Log(selfNavAgent.pathStatus);

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
            if (isRanged == false)
            {
                if (Vector3.Distance(transform.position, temp.position) <= attackDistance)
                {
                    AttackBox.SetActive(true);
                }
                else AttackBox.SetActive(false);
            }
            else
            {
                if (temprangedtimer < rangedFireRate) temprangedtimer += Time.deltaTime;
                //make ranged attack
                if (Vector3.Distance(transform.position, temp.position) <= attackDistance)
                {
                    if (temprangedtimer >= rangedFireRate && selfNavAgent.destination != null)
                    {
                        Vector3 fwd = rangedAttackSpawn.transform.TransformDirection(Vector3.forward);
                        RaycastHit hit;
                        Vector3 targetPoint;
                        if (Physics.Raycast(transform.position, fwd, out hit, attackDistance))
                            targetPoint = hit.point;
                        else targetPoint = fwd;

                        Vector3 directionWithoutSpread = targetPoint - rangedAttackSpawn.transform.position;
                        GameObject currentSpell = Instantiate(RangedAttack, rangedAttackSpawn.transform.position, rangedAttackSpawn.transform.rotation);
                        currentSpell.transform.forward = directionWithoutSpread.normalized;

                        currentSpell.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.None;
                        currentSpell.GetComponentInChildren<Rigidbody>().AddForce(directionWithoutSpread.normalized * forwardForce, ForceMode.Impulse);
                        currentSpell.GetComponentInChildren<Rigidbody>().AddForce(rangedAttackSpawn.transform.up * upwardForce, ForceMode.Impulse);
                        temprangedtimer = 0;
                    }
                }
                else return;
            }
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
