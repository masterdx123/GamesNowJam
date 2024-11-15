using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class WeaponProjectile : MonoBehaviour
{
    public Weapon senderWeapon;

    [Header("If there is a senderWeapon the values will be replaced with those of the senderWeapon!")]
    [SerializeField] private int finalDamage;
    [SerializeField] private float projectileLifeRemain;
    [SerializeField] private float bulletSpeed;
    public float angle;

    public Rigidbody2D rb;
    public BoxCollider2D boxCollider;

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
        Vector2 velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * projectileVelocity;
        rb.linearVelocity = velocity;

        if (projectileLifeRemain > 0) projectileLifeRemain -= Time.deltaTime;
        if (projectileLifeRemain <= 0) Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject)
        {
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
            float damage = senderWeapon
                ? finalDamage * (1 + senderWeapon.DamageModifier) + senderWeapon.DamageModifier
                : finalDamage;
            damageable.TakeDamage(damage);
            Destroy(gameObject);    
        }
    }
}
