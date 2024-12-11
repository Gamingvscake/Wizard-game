using System.Collections;
using UnityEngine;
using TMPro;
public class SpellController : MonoBehaviour
{
    //Fireball object
    public GameObject fireball, upgradedStaff;

    //Fireball force
    public float castForce, upwardForce;

    //Fireball stats
    public float castTime, spread, reloadTime, castSpeed, hitscanRange, hitscanTrailDuration;
    public int manaSize, spellsPerTap, manaPerShot;
    public bool allowButtonHold;
    public float maxtimer;
    private float temptimer;
    public int spellsLeft, spellsCasted;

    //Bool stuff
    public bool casting, readyToCast, reloading;
    private bool canSwing;

    public enum AttackType
    {
        Projectile,
        HitScan,
        Turret,
        Wall,
        Area,
        Melee
    }

    public AttackType attackType;
    //References
    public Camera playerCam;
    public Transform attackPoint;
    public GameObject meleeHitbox;
    //Graphics
    public TextMeshProUGUI manaDisplay;
    WeaponSwapControl WSC;
    public CauldronScript sccs;
    public AnvilScript scas;
    public PlayerInflicts scPI;
    public LineRenderer lineRenderer;
    //Bug fixing
    public bool allowInvoke = true;
    public int Level;


    private void Awake()
    {
        //Fills mana
        readyToCast = true;
        playerCam = GetComponentInParent<Camera>();
        WSC = GetComponentInParent<WeaponSwapControl>();
        scPI = WSC.wscPI;
        manaSize = WSC.MaxMana;
        manaDisplay = WSC.DisplayMana;
        spellsCasted = WSC.MaxMana;
        spellsLeft = WSC.Mana;
        sccs = WSC.wsccs;
        scas = WSC.wscas;
        scas.spellCon = this.gameObject.GetComponent<SpellController>();
        Physics.IgnoreLayerCollision(6,6);
        Physics.IgnoreLayerCollision(6,3);
        meleeHitbox.SetActive(false);
        temptimer = maxtimer;
        canSwing = false;
    }

    private void Update()
    {
        MyInput();
        //Set mana display
        WSC.UpdateManaDisplay(WSC.Mana, manaPerShot, WSC.MaxMana);
        spellsLeft = WSC.Mana;
        if (maxtimer > 0)
        {
            if (temptimer < maxtimer) temptimer += Time.deltaTime;
            if (temptimer >= maxtimer) canSwing = true;
        }
        else if (maxtimer <= 0)canSwing = true;
    }

    private void MyInput()
    {
        //Check if you are allowed to hold cast
        if (canSwing && allowButtonHold && sccs.InCauldronScreen == false) casting = ((Input.GetAxis("RTFire1") > 0.1f) || Input.GetKey(KeyCode.Mouse0));
        else if (canSwing && sccs.InCauldronScreen == false) casting = ((Input.GetAxis("RTFire1") > 0.1f) || Input.GetKeyDown(KeyCode.Mouse0));

        //if (allowButtonHold) casting = Input.GetKey(KeyCode.Mouse0);
        //else casting = Input.GetKeyDown(KeyCode.Mouse0);

        //Reloading
        if (Input.GetKeyDown(KeyCode.R) && spellsLeft < manaSize && !reloading) Reload();

        //Reload automatically when trying to cast without mana
        if (readyToCast && casting && !reloading && spellsLeft <= 0) Reload();

        //Casting
        if (readyToCast && casting && spellsLeft > 0)
        {
            //Set mana to 0
            spellsCasted = 0;

            Cast();
            
        }
    
    }

    private void Cast()
    {
        readyToCast = false;
        if (attackType == AttackType.Projectile)
        {
            //Find the exact hit position using a raycast
            Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            //Check if ray hit something
            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit))
                targetPoint = hit.point;
            else targetPoint = ray.GetPoint(75);

            //Calculate direction
            Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

            //Calculate spread
            //float x = Random.Range(-spread, spread);
            //float y = Random.Range(-spread, spread);

            //Calculate with spread
            //Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

            //Instantiate projectile
            GameObject currentSpell = Instantiate(fireball, attackPoint.position, attackPoint.rotation);
            currentSpell.GetComponentInChildren<DamageScript>().cs = sccs;
            currentSpell.GetComponentInChildren<DamageScript>().dswsc = WSC;
            currentSpell.GetComponentInChildren<DamageScript>().dsPI = scPI;
            //Rotate spell to shoot direction
            currentSpell.transform.forward = directionWithoutSpread.normalized;

            //Add forces to spell
            currentSpell.GetComponentInChildren<Rigidbody>().AddForce(directionWithoutSpread.normalized * castForce, ForceMode.Impulse);
            currentSpell.GetComponentInChildren<Rigidbody>().AddForce(playerCam.transform.up * upwardForce, ForceMode.Impulse);
        }
        else if (attackType == AttackType.HitScan)
        {
            Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            lineRenderer = GetComponentInChildren<LineRenderer>();
            lineRenderer.SetPosition(0, attackPoint.position);
            if (Physics.Raycast(ray, out hit, hitscanRange))
            {
                lineRenderer.SetPosition(1, hit.point);
                lineRenderer.enabled = true;
                GameObject currentSpell = Instantiate(fireball, attackPoint.position, attackPoint.rotation);
                currentSpell.GetComponentInChildren<DamageScript>().cs = sccs;
                currentSpell.GetComponentInChildren<DamageScript>().dswsc = WSC;
                currentSpell.GetComponentInChildren<DamageScript>().dsPI = scPI;
                DamageScript tempds = currentSpell.GetComponentInChildren<DamageScript>();
                Destroy(currentSpell);
                print(tempds + " " + tempds.Damage);
                if (hit.collider.tag == "Enemy" || hit.collider.gameObject.tag == "Enemy")
                {
                    EnemyHealthScript tempEHS = hit.collider.GetComponent<EnemyHealthScript>();
                    tempEHS.DODAMAGE(tempds);
                }
            }
            else
            {
                lineRenderer.SetPosition(1, ray.origin + (playerCam.transform.forward * hitscanRange));
            }
            StartCoroutine(ShootHitScan());
        }
        else if (attackType == AttackType.Melee)
        {
            GameObject currentSpell = Instantiate(fireball, attackPoint.position, attackPoint.rotation);
            currentSpell.GetComponentInChildren<DamageScript>().cs = sccs;
            currentSpell.GetComponentInChildren<DamageScript>().dswsc = WSC;
            currentSpell.GetComponentInChildren<DamageScript>().dsPI = scPI;
            DamageScript tempds = currentSpell.GetComponentInChildren<DamageScript>();
            Destroy(currentSpell);
            DamageScript temphitbox = meleeHitbox.GetComponentInChildren<DamageScript>();
            temphitbox.cs = sccs;
            temphitbox.dswsc = WSC;
            temphitbox.dsPI = scPI;
            temphitbox.damageType = tempds.damageType;
            temphitbox.Damage = tempds.Damage;
            meleeHitbox.SetActive(true);
        }
        else if (attackType == AttackType.Turret)
        {
            if (WSC.numberOfTurrets < WSC.maxNumberOfTurrets)
            {
                GameObject currentSpell = Instantiate(fireball,attackPoint.position, Quaternion.identity);
                currentSpell.GetComponentInChildren<DamageScript>().cs = sccs;
                currentSpell.GetComponentInChildren<DamageScript>().dswsc = WSC;
                currentSpell.GetComponentInChildren<DamageScript>().dsPI = scPI;
                currentSpell.GetComponentInChildren<TurretMoveScript>().castForce = castForce;
                currentSpell.GetComponentInChildren<TurretMoveScript>().upwardForce = upwardForce;
                WSC.numberOfTurrets += 1;
            }
        }
        spellsLeft-= manaPerShot;
        WSC.Mana-= manaPerShot;
        spellsCasted++;
        if (maxtimer > 0)canSwing = false;
        //print(spellsCasted);
        //Invoke resetCast function if not already invoked
        //Stop spells from spamming
        if (spellsCasted < spellsPerTap && spellsLeft > 0)
        {
            Invoke("Cast", castSpeed);
        }
        else if (allowInvoke)
        {
            Invoke("ResetCast", castSpeed);
            allowInvoke = false;
        }
    }
    IEnumerator ShootHitScan()
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(hitscanTrailDuration);
        lineRenderer.enabled = false;
    }
    private void ResetCast()
    {
        //Allow shooting and invoking again
        if (maxtimer > 0) canSwing = false;
        readyToCast = true;
        allowInvoke = true;
        casting = false;
        meleeHitbox.SetActive(false);
        temptimer = 0;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        spellsLeft = WSC.MaxMana;
        WSC.Mana = WSC.MaxMana;
        reloading = false;
    }
}
