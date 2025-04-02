using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlaskExploder : MonoBehaviour
{
    public GameObject HealCast;
    /*public TMP_Text[] reviveTexts;
    private static Dictionary<int, int> reviveCounts = new Dictionary<int, int>();
    public GameObject revivestuff;*/

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("LevelGeom"))
        {
            Instantiate(HealCast, transform.position + Vector3.up * 0.25f, Quaternion.identity);
            /*revivestuff.GetComponent<ReviveScript>().reviveTexts = reviveTexts;*/
            Destroy(gameObject);
        }
    }
}