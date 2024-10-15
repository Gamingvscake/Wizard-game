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
    public bool StickInTarget;
    public Rigidbody rb;
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
    private void Delete()
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
    private void OnCollisionEnter(Collision collision)
    {
        if (StickInTarget)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            this.gameObject.GetComponent<Collider>().enabled = false;
            if (collision.collider.tag == "Enemy")
            {
                Delete();
            }
            else
            {
                Destroy(item, DeleteDelay);
            }
        }
        else
        {
            if (collision.collider.tag == "Enemy")
            {
                Delete();
            }
            else
            {
                Destroy(item, DeleteDelay);
            }
        }
    }
}
