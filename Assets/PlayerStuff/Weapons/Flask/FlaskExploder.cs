using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaskExploder : MonoBehaviour
{
    public GameObject HealCast;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("LevelGeom"))
        {
            Instantiate(HealCast, transform.position + Vector3.up * 0.25f, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}