using NUnit.Framework;
using System.Collections.Generic;
using Interfaces;
using Inventory;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer),typeof(Rigidbody2D),typeof(Animator))]
public class EnemyBehaviour : MonoBehaviour, IDamageable
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;

    [Header("Actions")]
    [SerializeField] private List<string> movementList;
    [SerializeField] private List<string> attackList;
    [SerializeField] private List<string> onDeathList;

    public GameObject target {  get => GetTarget();}

    [SerializeField] EnemyData enemyData;
    [SerializeField] private float speed;
    [SerializeField] private float maxHealth = 50.0f;
    private float _health;
    public float Speed { private set => speed = value; get => speed; }

    private UnityEvent<EnemyBehaviour> Move = new UnityEvent<EnemyBehaviour>();
    private UnityEvent<GameObject, GameObject> Attack = new UnityEvent<GameObject, GameObject>();
    private UnityEvent<GameObject, GameObject> OnDeath = new UnityEvent<GameObject, GameObject>();
    private float internalCooldown;
    [SerializeField] private float attackInterval;
    
    void Start()
    {
        enemyData.Start();

        foreach (var move in movementList)
        {
            Move.AddListener(enemyData.MovesDictionary[move]);
        }
        foreach (var attack in attackList)
        {
            Attack.AddListener(enemyData.AttacksDictionary[attack]);
        }
        foreach (var attack in onDeathList)
        {
            OnDeath.AddListener(enemyData.AttacksDictionary[attack]);
        }

        _health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        SpriteFlip();

        Move?.Invoke(this);

        if (internalCooldown <= 0)
        {
            Attack?.Invoke(this.gameObject, GetTarget());
            internalCooldown = attackInterval;
        }

        //Reduces cooldown as a timer.
        if (internalCooldown > 0)
        {
            internalCooldown -= Time.deltaTime;
        }
    }

    GameObject GetTarget()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }

    public float GetAngleToTarget()
    {
        var differenceFromEnemyToTarget = target.transform.position - this.gameObject.transform.position;
        return Mathf.Atan2(differenceFromEnemyToTarget.y, differenceFromEnemyToTarget.x) * Mathf.Rad2Deg;
    }

    void SpriteFlip()
    {
        var normalizedDifferenceX = 0f;

        if (rb.linearVelocity.normalized.x != 0) normalizedDifferenceX = Mathf.Abs(rb.linearVelocity.normalized.x) / rb.linearVelocity.normalized.x;

        spriteRenderer.flipX = -normalizedDifferenceX != 1;
    }

    public void TakeDamage(float damage)
    {
        _health = Mathf.Clamp(_health - damage, 0, maxHealth);

        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke(gameObject, GetTarget());
        ItemData[] items = enemyData.GetIdemDrops();
        foreach (var item in items)
        {
            GameObject pickup = new GameObject();
            pickup.AddComponent<ItemPickup>();
            ItemPickup itemPickup = pickup.GetComponent<ItemPickup>();
            itemPickup.ItemData = item;
            Instantiate(pickup, transform.position, Quaternion.Euler(0,0, transform.localRotation.eulerAngles.z));
        }
        Destroy(gameObject);
    }
}
