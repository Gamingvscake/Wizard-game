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
    public enum DamageType 
    { 
        DONTUSE,
        Neutral,
        Fire,
        Water,
        Earth,
        Air,
        Light,
        Dark
    }
    private void Start()
    {
        item = transform.parent.gameObject;
        SplashRadius.enabled = false;
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
    private void SplashDelay()
    {
        SplashRadius.enabled = false;
    }
    private void OnCollisionEnter(Collision collision)
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
            }
            else
            {
                Destroy(item, DeleteDelay);
            }
            Invoke("SplashDelay", 0.1f);
        }
    }
}
