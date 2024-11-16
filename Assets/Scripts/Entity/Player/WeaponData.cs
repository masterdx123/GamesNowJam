using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Create New Weapon")]
public class WeaponData : ScriptableObject
{
	public const int MaxNumberAttacks = 4;

	[Header("Stats")]
    public string weaponName;
    public Sprite sprite;
	public int damage;
	public float fireRateInterval;

	[Header("Attack Settings")]
    public GameObject attackObject;
    [Tooltip("If set to 0 means the projectile is STATIC!")] public float projectileVelocity;
	public float projectileDuration;

	public int projectileAmount;

    [Space(15), Header("Display Settings")]
	[Range(1, MaxNumberAttacks)] public int numberUniqueAttacks;
    [Tooltip("Value is in pixels.")] public Vector2 offsetInHand;
    [Tooltip("Value is in pixels.")] public Vector2 offsetProjectile;
    public float angleInHand;

	[Space(15), Header("Animations")]
	public RuntimeAnimatorController animations;
}
