using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    [Header("Ranged Variables")]
    public bool isRanged;
    public GameObject RangedAttack;
    public float temprangedtimer, rangedFireRate;
    public Transform rangedAttackSpawn;
    public float forwardForce, upwardForce;
    [Header("Spawner Variables")]
    public bool isSpawner,isMinion;
    public float spawningTimer, maxSpawningTimer;
    public int amountSpawned;
    public GameObject toBeSpawned;
    public EnemySpawnScript spawnScript;
    [Header("Boss Variables")]
    public bool isBoss;
    public float phaseChange, maxPhaseTimer;
    public int phases;
    bool onPhaseChange;

    [SerializeField] private AudioSource metalchain;
    [SerializeField] private AudioSource walk1;
    [SerializeField] private AudioSource walk2;

    public float timer = 0f;
    public float timelimit = 1.8f;
    private void Start()
    {
        PlayerNotList = new Transform[Players.Count];
        metalchain = GetComponent<AudioSource>();
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
        if (timer >= timelimit)
        {
            //metalchain.Play();
            //StartCoroutine(cutoutSound());
           // timer = 0f;
        }
        timer += Time.deltaTime;
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
            //transform.position = Vector3.MoveTowards(transform.position, temp.position, speed * Time.deltaTime);
            selfNavAgent.destination = temp.position;
            if (Vector3.Distance(transform.position, temp.position) <= attackDistance)
            {
                AttackBox.SetActive(true);
            }
            else AttackBox.SetActive(false);
        }
        if (isBoss)
        {
            if (phaseChange >= maxPhaseTimer) onPhaseChange = false;
            else phaseChange += Time.deltaTime;
            if (onPhaseChange == false)
            {
                if (phases == 0)
                {
                    isSpawner = true;
                    isRanged = false;
                }
                else if (phases == 1)
                {
                    attackDistance = 25;
                    isSpawner = false;
                    isRanged = true;
                }
                else
                {
                    attackDistance = 2.5f;
                    isSpawner = false;
                    isRanged = false;
                }
                if (phases <= 1) phases++;
                else phases = 0;
                onPhaseChange = true;
                phaseChange = 0;
            }
        }
        if (!DMGScript.Attacking && DevBoolToNotMove == false)
        {
            if (isRanged == false && isSpawner == false)
            {
                if (Vector3.Distance(transform.position, temp.position) <= attackDistance)
                {
                    AttackBox.SetActive(true);
                }
                else AttackBox.SetActive(false);
            }
            else if (isRanged == true && isSpawner == false)
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
            else if (isRanged == false && isSpawner == true)
            {
                if (spawningTimer >= maxSpawningTimer)
                {
                    GameObject tempE = Instantiate(toBeSpawned, transform);
                    tempE.GetComponent<EnemyHealthScript>().enemySpawn = spawnScript;
                    tempE.GetComponent<EnemyHealthScript>().bossMovement = this;
                    tempE.GetComponent<EnemyMovementScript>().Players = spawnScript.Players;
                    tempE.GetComponent<EnemyMovementScript>().entryPoints = spawnScript.EntryPoints;
                    tempE.GetComponent<EnemyMovementScript>().spawnScript = spawnScript;
                    tempE.GetComponent<EnemyMovementScript>().isMinion = true;
                    spawnScript.amountOfEnemies += 1;
                    amountSpawned += 1;
                    spawningTimer = 0;
                }
                else spawningTimer += Time.deltaTime;
            }
            else return;
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
            if (potentialTarget.GetComponentInChildren<DeathScreen>().isPlayerDead == false &&
                    potentialTarget.GetComponentInChildren<UIGameOver>().isPlayerDead == false)
            {
                if (potentialTarget.GetComponentInChildren<DeathScreen>().isPlayerDead == false)
                {
                    if (dSqrToTarget < closestDistanceSqr)
                    {
                        closestDistanceSqr = dSqrToTarget;
                        bestTarget = potentialTarget;
                    }
                }
                if (potentialTarget.GetComponentInChildren<DeathScreen>().isPlayerDead == true)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }
        }

        return bestTarget;
    }

    IEnumerator cutoutSound ()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public void walkSound()
    {
        int x = Random.Range(0, 3);
        Debug.Log("x is" + x);
        if (x == 0)
        {
            metalchain.Play();
        }
        if (x == 1)
        {
            walk1.Play();
        }
        if (x == 2)
        {
            walk2.Play();
        }
        StartCoroutine("cutoutSound");
    }
}
    