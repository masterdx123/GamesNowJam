using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class WeaponProjectile : MonoBehaviour
{
    public Weapon senderWeapon;
    public GameObject Owner{ set => _owner = value; }

    [Header("If there is a senderWeapon the values will be replaced with those of the senderWeapon!")]
    [SerializeField] private int finalDamage;
    [SerializeField] private float projectileLifeRemain;
    [SerializeField] private float bulletSpeed;
    
    [SerializeField] private bool isExplosive = false;
    [SerializeField] private bool isBoomerang = false;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private int enemiesPenetrated;
    public bool IsExplosive { get => isExplosive; set => isExplosive = value; }
    public bool IsBoomerang { get => isBoomerang; set => isBoomerang = value; }
    public int EnemiesPenetrated { get => enemiesPenetrated; set => enemiesPenetrated = value; }
    public float angle;

    public Rigidbody2D rb;
    public BoxCollider2D boxCollider;

    private GameObject _owner;
    private float initialProjectileLifetime;

    private int directionModifier = 1;

    private void Start()
    {
        if (senderWeapon)
        {
            projectileLifeRemain = senderWeapon.weaponData.projectileDuration * (1 + senderWeapon.RangeModifier);
            initialProjectileLifetime = senderWeapon.weaponData.projectileDuration * (1 + senderWeapon.RangeModifier);
            angle = senderWeapon.transform.rotation.eulerAngles.z;
        }
    }

    public void DealDamage()
    {
        // Entity takes senderWeaponData.damage;
        Debug.Log("Damage dealt: " + finalDamage);
    }

    void Update()
    {
        // Temp for test
        var projectileVelocity = senderWeapon ? senderWeapon.weaponData.projectileVelocity * (1 + senderWeapon.BulletVelocityModifier) : bulletSpeed;
        
        //If the player has the Boomerang Bullets upgrade, projectiles return to their origin point after they reach their maximum distance.
        if (projectileLifeRemain <= 0 && isBoomerang) {
            projectileLifeRemain = initialProjectileLifetime;
            isBoomerang = false;
            directionModifier = -1;
        }

        Vector2 velocity = gameObject.transform.right * projectileVelocity * directionModifier;
        rb.linearVelocity = velocity;

        if (projectileLifeRemain > 0) projectileLifeRemain -= Time.deltaTime;

        if (projectileLifeRemain <= 0 && !isBoomerang) Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider && collider.gameObject && _owner && !_owner.CompareTag(collider.gameObject.tag))
        {
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                float damage = senderWeapon
                    ? (finalDamage + senderWeapon.DamageFlatModifier) * (1 + senderWeapon.DamageModifier)
                    : finalDamage;

                damageable.TakeDamage(damage);

                //If the player has the Explosive Ammo upgrade, the first hit of each projectile generates an explosion (via a prefab)
                if(isExplosive) {
                    GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
                    Debug.Log("explosion name:" + explosion.name);
                    explosion.GetComponent<Explosion>()._Owner = _owner;
                    explosion.GetComponent<Explosion>().Damage = damage/2;
                    isExplosive = false;
                }
            }

            //If the player has the Penetrating Bullets upgrade, projectiles may penetrate up to an "enemiesPenetrated" amount of damageable hitboxes
            if(enemiesPenetrated == 0) {
                Destroy(gameObject);    
            }
            else {
                enemiesPenetrated--;
            }

        }
    }
}
