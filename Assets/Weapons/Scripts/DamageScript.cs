using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    GameObject item;
    public int Damage;
    public bool DeleteOnImpact;
    public float DeleteDelay;
    public DamageType damageType;
    public enum DamageType 
    { 
        Neutral,
        Fire,
        Water,
        Earth,
        Air
    }
    private void Start()
    {
        item = transform.parent.gameObject;
    }
    private void Update()
    {
        Destroy(item, DeleteDelay + 1);
    }
    private void NewMethod()
    {
        Destroy(item, DeleteDelay);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            if (DeleteOnImpact)
            {
                Destroy(item);
            }
            else
            {
                Destroy(item, DeleteDelay);
            }
        }
        else
        {
            Destroy(item, DeleteDelay);
        }
    }
}
