using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlaskExploder : MonoBehaviour
{
    public GameObject HealCast;
    public GameObject reviverObject;
    

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("LevelGeom"))
        {
            GameObject temp = Instantiate(HealCast, transform.position + Vector3.up * 0.25f, Quaternion.identity);
            temp.GetComponent<ReviveScript>().reviverObject = reviverObject;
            
            Destroy(gameObject);
        }
    }
}