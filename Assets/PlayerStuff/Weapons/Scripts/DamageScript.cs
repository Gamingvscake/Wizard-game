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

    public SphereCollider SplashRadius;
    public Collider thisCollider;
    public CauldronScript cs;
    public WeaponSwapControl dswsc;
    public PlayerInflicts dsPI;

    public bool isMelee;
    public bool isTurret;

    public GameObject instantiateVFX;

    public enum DamageType 
    { 
        DONTUSE,
        Neutral,
        Fire,
        Water,
        Earth,
        Air,
        Light,
        Dark,
        Ice
    }
    private void Start()
    {
        item = transform.parent.gameObject;
        if (SplashRadius != null ) SplashRadius.enabled = false;
    }
    private void Update()
    {
        if (isMelee != true)
        {
            if (StickInTarget == false && rb != null)rb.constraints = RigidbodyConstraints.None;
            Destroy(item, DeleteDelay + 1);
        }
        if (isTurret == true)
        {
            DeleteDelay -= Time.deltaTime;
            if(DeleteDelay <= 0.1f)
            {
                TurretMoveScript tms = GetComponent<TurretMoveScript>();
                tms.die = true;
                isTurret = false;
            }
        }
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
    private void SplashDelay()
    {
        if (SplashRadius != null) SplashRadius.enabled = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (SplashRadius != null)
        {
            SplashRadius.radius += cs.SplashSize;
            SplashRadius.enabled = true;
            if (StickInTarget)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
                thisCollider.enabled = false;
                if (collision.collider.tag == "Enemy")
                {
                    dsPI.LifeStealDo(cs.LifeSteal);
                    Delete();
                    Instantiate(instantiateVFX, collision.collider.transform.position + Vector3.up * 0.25f, Quaternion.identity);
                }
                else
                {
                    Destroy(item, DeleteDelay);
                }
                Invoke("SplashDelay", 0.1f);
            }
            else
            {
                if (collision.collider.tag == "Enemy")
                {
                    Delete();
                    Instantiate(instantiateVFX, collision.collider.transform.position + Vector3.up * 0.25f, Quaternion.identity);
                }
                else
                {
                    Destroy(item, DeleteDelay);
                }
                Invoke("SplashDelay", 0.1f);
            }
        }
    }
}
