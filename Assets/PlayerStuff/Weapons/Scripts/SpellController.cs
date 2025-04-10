using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; // Required for the new Input System

public class SpellController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource icecracking;
    [SerializeField] private AudioSource firestaff;

    [SerializeField] private AudioSource icecrackingUPGRADED;
    [SerializeField] private AudioSource firestaffUPGRADED;

    [SerializeField] private AudioSource laserSound;
    [SerializeField] private AudioSource lightSound;
    [SerializeField] private AudioSource windSound;
    [SerializeField] private AudioSource IronSound;
    [SerializeField] private AudioSource defaultStaff;
    public GameObject fireballStaff;


    [SerializeField] private AudioSource crystalnoise;


    [Header("Variables")]
    // Fireball object
    public GameObject fireball, upgradedStaff;

    // Fireball force
    public float castForce, upwardForce;

    // Fireball stats
    public float castTime, spread, reloadTime, castSpeed, hitscanRange, hitscanTrailDuration;
    public int manaSize, spellsPerTap, manaPerShot;
    public bool allowButtonHold;
    public float maxtimer;
    private float temptimer;
    public int spellsLeft, spellsCasted;

    // Bool stuff
    public bool casting, readyToCast, reloading;
    private bool canSwing;

    public MovementController movementController;

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
    // References
    public Camera playerCam;
    public Transform attackPoint;
    public GameObject meleeHitbox;
    public Animator animator;
    // Graphics
    public TextMeshProUGUI manaDisplay;
    WeaponSwapControl WSC;
    public CauldronScript sccs;
    public AnvilScript scas;
    public PlayerInflicts scPI;
    public LineRenderer lineRenderer;
    // Bug fixing
    public bool allowInvoke = true;
    public int Level;

    private PlayerController inputActions; // Reference to PlayerController Input Map
    private InputAction fireStaffAction; // Reference to FireStaff action
    private InputAction moveStaffAction;

    private void Awake()
    {
        // Fills mana
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
        Physics.IgnoreLayerCollision(6, 6);
        Physics.IgnoreLayerCollision(6, 3);
        meleeHitbox.SetActive(false);
        temptimer = maxtimer;
        canSwing = false;

        // Initialize Input Map
        inputActions = new PlayerController();
        fireStaffAction = inputActions.PlayerControls.FireStaff;
        moveStaffAction = inputActions.PlayerControls.FireStaff;
        inputActions.Enable();
    }

    private void OnDestroy()
    {
        inputActions.Disable(); // Cleanup to avoid memory leaks
    }

    private void Update()
    {
        MyInput();
        // Set mana display
        WSC.UpdateManaDisplay(WSC.Mana, manaPerShot, WSC.MaxMana);
        spellsLeft = WSC.Mana;
        if (maxtimer > 0)
        {
            if (temptimer < maxtimer) temptimer += Time.deltaTime;
            if (temptimer >= maxtimer)
            {
                canSwing = true;
                animator.SetBool("ResetSwing", false);
            }
        }
        else if (maxtimer <= 0)
        {
            canSwing = true;
            animator.SetBool("ResetSwing", false);
        }
    }

    private void MyInput()
    {
        if (movementController.DevKeyboardOn == false)
        {
            // Check the trigger value for firing
            bool fireStaffValue = movementController.ShootSpell;

            // Allow holding or tapping to cast based on the action value
            if (canSwing && allowButtonHold && sccs.InCauldronScreen == false)
                casting = fireStaffValue; // Trigger value threshold
            else if (canSwing && sccs.InCauldronScreen == false)
                casting = fireStaffValue;
        }
        else
        {
            if (canSwing && allowButtonHold && sccs.InCauldronScreen == false && scas.InUpgradeScreen == false && Input.GetKey(KeyCode.Mouse0))
                casting = true; // Trigger value threshold
            else if (canSwing && sccs.InCauldronScreen == false && scas.InUpgradeScreen == false && Input.GetKey(KeyCode.Mouse0))
                casting = true;
        }
        // Reloading
        if (Input.GetKeyDown(KeyCode.R) && spellsLeft < manaSize && !reloading) Reload();

        // Reload automatically when trying to cast without mana
        if (readyToCast && casting && !reloading && spellsLeft <= 0) Reload();

        // Casting
        if (readyToCast && casting && spellsLeft > 0)
        {
            // Set mana to 0
            spellsCasted = 0;

            Cast();
            
        }
    
    }

    private void Cast()
    {
        readyToCast = false;
        if (attackType == AttackType.Projectile)
        {
            // Find the exact hit position using a raycast
            Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            // Check if ray hit something
            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit))
                targetPoint = hit.point;
            else targetPoint = ray.GetPoint(75);

            // Calculate direction
            Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

            // Instantiate projectile
            GameObject currentSpell = Instantiate(fireball, attackPoint.position, attackPoint.rotation);
            currentSpell.GetComponentInChildren<DamageScript>().cs = sccs;
            currentSpell.GetComponentInChildren<DamageScript>().dswsc = WSC;
            currentSpell.GetComponentInChildren<DamageScript>().dsPI = scPI;
            // Rotate spell to shoot direction
            currentSpell.transform.forward = directionWithoutSpread.normalized;

            // Add forces to spell
            currentSpell.GetComponentInChildren<Rigidbody>().AddForce(directionWithoutSpread.normalized * castForce, ForceMode.Impulse);
            currentSpell.GetComponentInChildren<Rigidbody>().AddForce(playerCam.transform.up * upwardForce, ForceMode.Impulse);

            // Play the ice cracking sound
           
            if (icecracking != null)
            {
                    icecracking.Play();
            }
           
            if (icecrackingUPGRADED != null)
            {
                icecrackingUPGRADED.Play();
            }

                                           

            if (moveStaffAction.ReadValue<float>() > 0.5f && windSound != null) 
            {
                
                
                    //StopCoroutine(); 
                

                // Start a new coroutine to play and stop the wind sound after 1 second
                StartCoroutine("PlayWindSoundWithDelay");
                //StartCoroutine(windSoundCoroutine);
            }

                        
            //Default Staff Sound
            
            if (defaultStaff != null) defaultStaff.Play();

            //Crystalnoise
            
            if (crystalnoise != null) crystalnoise.Play();

            if (fireStaffAction.ReadValue<float>() > 0.5f && firestaff != null) // Check the trigger press value again
            {
                firestaff = GetComponent<AudioSource>();
                firestaff.Stop();
                firestaff.Play();
            }
            //Upgraded fire noise
            if (fireStaffAction.ReadValue<float>() > 0.5f && firestaff != null) // Upgraded fire sound
            {
                firestaffUPGRADED = GetComponent<AudioSource>();
                firestaffUPGRADED.Stop();
                firestaffUPGRADED.Play();
            }
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

            if (lightSound != null) lightSound.Play();

            
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

            if (IronSound != null) IronSound.Play();
        }
        else if (attackType == AttackType.Turret)
        {
            if (WSC.numberOfTurrets < WSC.maxNumberOfTurrets)
            {
                GameObject currentSpell = Instantiate(fireball, attackPoint.position, Quaternion.identity);
                currentSpell.GetComponentInChildren<DamageScript>().cs = sccs;
                currentSpell.GetComponentInChildren<DamageScript>().dswsc = WSC;
                currentSpell.GetComponentInChildren<TurretMoveScript>().TMSWSC = WSC;
                currentSpell.GetComponentInChildren<DamageScript>().dsPI = scPI;
                currentSpell.GetComponentInChildren<TurretMoveScript>().castForce = castForce;
                currentSpell.GetComponentInChildren<TurretMoveScript>().upwardForce = upwardForce;

                WSC.numberOfTurrets += 1;
            }

            

        }
        spellsLeft -= manaPerShot;
        WSC.Mana -= manaPerShot;
        spellsCasted++;
/*        if (maxtimer > 0)
        {
            canSwing = false;
            animator.SetBool("ResetSwing", true);
        }*/
        // Invoke resetCast function if not already invoked
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

    private void ResetCast()
    {
        if (maxtimer > 0)
        {
            canSwing = false;
            animator.SetBool("ResetSwing", true);
        }
        readyToCast = true;
        allowInvoke = true;
        casting = false;
        meleeHitbox.SetActive(false);
        temptimer = 0;
    }

    private void Reload()
    {
        reloading = true;
        animator.SetBool("Reload", true);
        Invoke("ReloadFinished", reloadTime);
    }
    IEnumerator ShootHitScan()
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(hitscanTrailDuration);
        lineRenderer.enabled = false;
    }
    private void ReloadFinished()
    {
        spellsLeft = WSC.MaxMana;
        WSC.Mana = WSC.MaxMana;
        animator.SetBool("Reload", false);
        reloading = false;
    }

    private IEnumerator PlayWindSoundWithDelay()
    {
        windSound.Stop();
        
        yield return new WaitForSeconds(0.3f);
        windSound.Play();
        
    }
    
}
