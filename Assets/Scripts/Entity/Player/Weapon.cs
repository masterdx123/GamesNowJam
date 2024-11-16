using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Library;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class Weapon : MonoBehaviour
{

    private const float MINIMUM_ATTACK_INTERVAL = 0.1f;
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
    private OxygenSystem _oxygenSystem;
    private float currentOxygenCount;

    [Space(15), Header("Upgrades")]
    [SerializeField] private List<UpgradeData> upgrades;
    private List<UpgradeData> lastUpgradeDisposition;

    [Space(15),Header("Modifiers")]
    [SerializeField] private int damageFlatModifier;
    [SerializeField] private float damageModifier;    
    [SerializeField] private float bulletVelocityModifier;
    [SerializeField] private float rangeModifier;
    [SerializeField] private float attackIntervalModifier;
    [SerializeField] private int projectileAmountModifier;
    [SerializeField] private bool isFrenzied = false;

    public int DamageFlatModifier { get => damageFlatModifier; set => damageFlatModifier = value; }
    public float DamageModifier { get => damageModifier; set => damageModifier = value; }
    public float BulletVelocityModifier { get => bulletVelocityModifier; set => bulletVelocityModifier = value; }
    public float RangeModifier { get => rangeModifier; set => rangeModifier = value; }
    public float AttackIntervalModifier { get => attackIntervalModifier; set => attackIntervalModifier = value; }
    public int ProjectileAmountModifier { get => projectileAmountModifier; set => projectileAmountModifier = value; }
    public bool IsFrenzied { get => isFrenzied; set => isFrenzied = value; }

    [SerializeField] private AudioClip[] shoot;
    private AudioSource audioSource;
    private AudioClip shootClip;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        weaponPivot = WeaponPivot;
        attackAction.Enable();
        UpdateWeapon();
        _oxygenSystem = GameObject.FindGameObjectWithTag("MainConsole").GetComponentInChildren<OxygenSystem>();
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
    
        //When the player has the Frenzy Module upgrade, their firerate interval may be reduced by an additional 0.5 secs (scaling inversely with bubble size)
        if(isFrenzied){
            currentOxygenCount = _oxygenSystem.CurrentEnergy;
            internalCooldown = internalCooldown - (0.5f - currentOxygenCount/200);
        } 
        
        //Weapons CANNOT shoot faster than MINIMUM_ATTACK_INTERVAL (aka 0.1 seconds).
        if(internalCooldown < MINIMUM_ATTACK_INTERVAL) internalCooldown = MINIMUM_ATTACK_INTERVAL;

        int index = Random.Range(0, shoot.Length);
        shootClip = shoot[index];

        audioSource.clip = shootClip;
        audioSource.Play();

        animator.Play($"Attack{attackCounter}", 0, 0f);
        Attack();
        attackCounter++;
        if (attackCounter > weaponData.numberUniqueAttacks) attackCounter = 1;
    }
    private void Attack()
    {
        //When the player has the Spread Shot upgrade they shoot more than 3 bullets which fire at an angle of each other 
        WeaponProjectile[] weaponProjectiles = FunctionLibrary.ShootSpread(
            weaponData.projectileAmount + ProjectileAmountModifier, 
            90, 
            weaponData.attackObject, 
            WeaponPivot.localRotation.eulerAngles.z,
            projectilePivot.position,
            playerController.gameObject,
            WeaponPivot.localRotation.eulerAngles.z,
            this
        );

        foreach (var projectile in weaponProjectiles)
        {
            foreach (var upgrade in upgrades)
            {
                if(upgrade.GetType() == typeof(WeaponUpgradeData))
                {
                    upgrade.ExecuteUpgrade(projectile);
                }
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
        if (upgrades.Contains(upgrade))
        {
            upgrades.Remove(upgrade);

            if (upgrade.GetType() == typeof(WeaponUpgradeData))
            {
                upgrade.RemoveUpgrade(this);
            }
        }
    }

    public void AddUpgrade(UpgradeData upgrade)
    {
        if (!upgrades.Contains(upgrade))
        {
            upgrades.Add(upgrade);

            if (upgrade.GetType() == typeof(WeaponUpgradeData))
            {
                upgrade.AddUpgrade(this);
            }
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
