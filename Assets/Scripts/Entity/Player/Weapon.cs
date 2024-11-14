using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class Weapon : MonoBehaviour
{
    public Transform weaponPivot { get; private set; }
    private PlayerController playerController { get => GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); }
    [SerializeField] private Transform WeaponPivot;
    [SerializeField] private Transform projectilePivot;

    public WeaponData weaponData;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public InputAction attackAction;

    private int attackCounter;
    private float internalCooldown;

    [Space(15), Header("Upgrades")]
    [SerializeField] private List<UpgradeData> upgrades;
    private List<UpgradeData> lastUpgradeDisposition;

    [Space(15),Header("Modifiers")]
    [SerializeField] private int damageFlatModifier;
    [SerializeField] private float damageModifier;    
    [SerializeField] private float bulletVelocityModifier;
    [SerializeField] private float rangeModifier;
    [SerializeField] private float attackIntervalModifier;

    public int DamageFlatModifier { get => damageFlatModifier; set => damageFlatModifier = value; }
    public float DamageModifier { get => damageModifier; set => damageModifier = value; }
    public float BulletVelocityModifier { get => bulletVelocityModifier; set => bulletVelocityModifier = value; }
    public float RangeModifier { get => rangeModifier; set => rangeModifier = value; }
    public float AttackIntervalModifier { get => attackIntervalModifier; set => attackIntervalModifier = value; }



    void Start()
    {
        weaponPivot = WeaponPivot;
        attackAction.Enable();
        UpdateWeapon();
    }

    void Update()
    {
        if (attackAction.IsPressed() && internalCooldown <= 0 && playerController.currentPlayerGameState == PlayerStates.InGame)
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
        internalCooldown = weaponData.fireRateInterval * (1 + AttackIntervalModifier);

        animator.Play($"Attack{attackCounter}", 0, 0f);
        Attack();
        attackCounter++;
        if (attackCounter > weaponData.numberUniqueAttacks) attackCounter = 1;
    }
    private void Attack()
    {
        //ResetModifiers();
        var attackGo = Instantiate(weaponData.attackObject, projectilePivot.position, Quaternion.Euler(0,0, WeaponPivot.localRotation.eulerAngles.z));
        WeaponProjectile attackProjectileComponent = attackGo.GetComponent<WeaponProjectile>();
        attackProjectileComponent.senderWeapon = this;
        foreach (var upgrade in upgrades)
        {
            if(upgrade.GetType() == typeof(WeaponUpgradeData))
            {
                upgrade.ExecuteUpgrade(this);
                upgrade.ExecuteUpgrade(attackProjectileComponent);
            }
        }
    }

    public void PlayIdleAnimation()
    {
        animator.Play($"Idle", 0, 0f);
    }

    public bool HasUpgrade(UpgradeData upgrade)
    {
        return upgrades.Contains(upgrade);
    }

    public void RemoveUpgrade(UpgradeData upgrade)
    {
        upgrades.Remove(upgrade);
    }

    public void AddUpgrade(UpgradeData upgrade)
    {
        if (!upgrades.Contains(upgrade))
        {
            upgrades.Add(upgrade);
        }
    }

    public void ResetModifiers()
    {
        damageFlatModifier = 0;
        damageModifier=0;
        bulletVelocityModifier = 0;
        rangeModifier=0;
        attackIntervalModifier=0;
    }
}
