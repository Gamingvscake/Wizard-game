using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;

public class SpellController : MonoBehaviour
{
    //Fireball object
    public GameObject fireball;

    //Fireball force
    public float castForce, upwardForce;

    //Fireball stats
    public float castTime, spread, reloadTime, castSpeed;
    public int manaSize, spellsPerTap;
    public bool allowButtonHold;

    public int spellsLeft, spellsCasted;

    //Bool stuff
    public bool casting, readyToCast, reloading;

    //References
    public Camera playerCam;
    public Transform attackPoint;

    //Graphics
    public TextMeshProUGUI manaDisplay;
    WeaponSwapControl WSC;
    public CauldronScript sccs;
    public PlayerInflicts scPI;

    //Bug fixing
    public bool allowInvoke = true;


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
        Physics.IgnoreLayerCollision(6,6);
        Physics.IgnoreLayerCollision(6,3);
    }

    private void Update()
    {
        MyInput();

        //Set mana display
        WSC.UpdateManaDisplay(spellsLeft, spellsPerTap, manaSize);
    }

    private void MyInput()
    {
        //Check if you are allowed to hold cast
        if (allowButtonHold) casting = ((Input.GetAxis("RTFire1") > 0.1f) || Input.GetKey(KeyCode.Mouse0));
        else casting = ((Input.GetAxis("RTFire1") > 0.1f) || Input.GetKey(KeyCode.Mouse0));

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


        spellsLeft--;
        WSC.Mana--;
        spellsCasted++;
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

    private void ResetCast()
    {
        //Allow shooting and invoking again
        readyToCast = true;
        allowInvoke = true;
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
