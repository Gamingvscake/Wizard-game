using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; // Required for the new Input System

public class SpellController : MonoBehaviour
{
    [SerializeField] private AudioSource icecracking;
    [SerializeField] private AudioSource firestaff;
    public GameObject fireballStaff;

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
            if (temptimer >= maxtimer) canSwing = true;
        }
        else if (maxtimer <= 0) canSwing = true;
    }

    private void MyInput()
    {
        // Check the trigger value for firing
        float fireStaffValue = fireStaffAction.ReadValue<float>();

        // Allow holding or tapping to cast based on the action value
        if (canSwing && allowButtonHold && sccs.InCauldronScreen == false)
            casting = fireStaffValue > 0.5f; // Trigger value threshold
        else if (canSwing && sccs.InCauldronScreen == false)
            casting = fireStaffValue > 0.5f;

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
            icecracking = GetComponent<AudioSource>();
            icecracking.Play();

            if (fireStaffAction.ReadValue<float>() > 0.5f) // Check the trigger press value again
            {
                firestaff = GetComponent<AudioSource>();
                firestaff.Stop();
                firestaff.Play();
            }
        }
        spellsLeft -= manaPerShot;
        WSC.Mana -= manaPerShot;
        spellsCasted++;
        if (maxtimer > 0) canSwing = false;

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
