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
    private void Start()
    {
        PlayerNotList = new Transform[Players.Count];
        AttackBox.SetActive(false);
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
        else if (!OutOfBounds)
        {
            Transform temp = GetClosestEnemy(PlayerNotList);
            transform.LookAt(temp);
            transform.position = Vector3.MoveTowards(transform.position, temp.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, temp.position) <= 1)
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
