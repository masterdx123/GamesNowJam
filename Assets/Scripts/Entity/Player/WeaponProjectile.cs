using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(BoxCollider2D))]
public class WeaponProjectile : MonoBehaviour
{

    public Weapon senderWeapon;
    private int finalDamage;
    private float projectileLifeRemain;
    private float weaponAngle;

    public Rigidbody2D rb;
    public BoxCollider2D boxCollider;

    private void Start()
    {
        projectileLifeRemain = senderWeapon.weaponData.projectileDuration * (1 + senderWeapon.RangeModifier);
        weaponAngle = senderWeapon.transform.rotation.eulerAngles.z;
    }

    public void DealDamage()
    {
        //Entity takes senderWeaponData.damage;
    }

    void Update()
    {

        Vector2 velocity = new Vector2(Mathf.Cos(weaponAngle * Mathf.Deg2Rad), Mathf.Sin(weaponAngle * Mathf.Deg2Rad)) * (senderWeapon.weaponData.projectileVelocity * (1 + senderWeapon.BulletVelocityModifier));
        rb.linearVelocity = velocity;

        if (projectileLifeRemain > 0) projectileLifeRemain -= Time.deltaTime;
        if (projectileLifeRemain <= 0) Destroy(this.gameObject);
    }
}
