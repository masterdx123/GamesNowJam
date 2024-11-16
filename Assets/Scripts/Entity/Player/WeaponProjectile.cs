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
    [SerializeField] private GameObject explosionPrefab;
    public bool IsExplosive { get => isExplosive; set => isExplosive = value; }
    [SerializeField] private bool isBoomerang = false;
    public bool IsBoomerang { get => isBoomerang; set => isBoomerang = value; }
    public float angle;

    public Rigidbody2D rb;
    public BoxCollider2D boxCollider;
    
    private GameObject _owner;

    private void Start()
    {
        if (senderWeapon)
        {
            projectileLifeRemain = senderWeapon.weaponData.projectileDuration * (1 + senderWeapon.RangeModifier);
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
        Vector2 velocity = gameObject.transform.right * projectileVelocity;
        rb.linearVelocity = velocity;

        if (projectileLifeRemain > 0) projectileLifeRemain -= Time.deltaTime;
        if (projectileLifeRemain <= 0) Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider && collider.gameObject && _owner && !_owner.CompareTag(collider.gameObject.tag))
        {
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                float damage = senderWeapon
                    ? finalDamage * (1 + senderWeapon.DamageModifier) + senderWeapon.DamageModifier
                    : finalDamage;
                damageable.TakeDamage(damage);

                
                if(isExplosive) {
                    GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
                    Debug.Log("explosion name:" + explosion.name);
                    explosion.GetComponent<Explosion>()._Owner = _owner;
                    explosion.GetComponent<Explosion>().Damage = damage/2;
                }
            }
            Destroy(gameObject);    
        }
    }
}
