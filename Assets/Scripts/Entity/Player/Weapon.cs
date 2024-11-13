using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class Weapon : MonoBehaviour
{
    public Transform weaponPivot { get; private set; }
    [SerializeField] private Transform WeaponPivot;
    [SerializeField] private Transform projectilePivot;

    public WeaponData weaponData;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public InputAction attackAction;

    private int attackCounter;
    private float internalCooldown;

    void Start()
    {
        weaponPivot = WeaponPivot;
        attackAction.Enable();
        UpdateWeapon();
    }

    void Update()
    {
        if (attackAction.IsPressed() && internalCooldown <= 0)
        {
            OnWeaponAttack();
        }
        
        //Reduces cooldown as a timer.
        if (internalCooldown > 0)
        {
            internalCooldown -= Time.deltaTime;
        }

    }

    public void UpdateWeapon()
    {
        if (weaponData)
        {
            attackCounter = 1;
            spriteRenderer.sprite = weaponData.sprite;
            animator.runtimeAnimatorController = weaponData.animations;
            this.transform.localPosition = new Vector3(weaponData.offsetInHand.x/16, weaponData.offsetInHand.y/16, 0);
            projectilePivot.localPosition = new Vector3(weaponData.offsetProjectile.x / 16, weaponData.offsetProjectile.y / 16, 0);
            this.transform.localRotation = Quaternion.Euler(0, 0, weaponData.angleInHand);
        }
        else Debug.LogError("WeaponData was not set!");
    }

    private void OnWeaponAttack()
    {
        //Resets cooldown of the weapon based on the weapon attack speed.
        internalCooldown = weaponData.fireRateInterval;

        animator.Play($"attack{attackCounter}", 0, 0f);
        Attack();
        attackCounter++;
        if (attackCounter > weaponData.numberUniqueAttacks) attackCounter = 1;
    }
    private void Attack()
    {
        var attackGo = Instantiate(weaponData.attackObject, projectilePivot.position, Quaternion.Euler(0,0, WeaponPivot.localRotation.eulerAngles.z));
        attackGo.GetComponent<WeaponProjectile>().senderWeapon = this;
    }
}
