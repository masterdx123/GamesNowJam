using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Inventory;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(Animator))]
public class EnemyBehaviour : MonoBehaviour, IDamageable
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;

    [Header("Actions")]
    [SerializeField] private List<string> movementList;
    [SerializeField] private List<string> attackList;
    [SerializeField] private List<string> onDeathList;

    [Header("Loot")]
    [SerializeField]
    private DroppedItem[] droppedItems;

    public GameObject target { get => GetTarget(); }

    [SerializeField] EnemyData enemyData;
    [SerializeField] private float speed;
    [SerializeField] private float maxHealth = 50.0f;
    private float _health;
    public float Speed { private set => speed = value; get => speed; }

    [SerializeField, Space(15)]
    private int creditValue = 1;
    public int CreditValue { get => creditValue; }

    [SerializeField]
    private int spawnAfterWave = 0;
    public int SpawnAfterWave { get => spawnAfterWave; }

    private UnityEvent<EnemyBehaviour> Move = new UnityEvent<EnemyBehaviour>();
    private UnityEvent<GameObject, GameObject> Attack = new UnityEvent<GameObject, GameObject>();
    private UnityEvent<GameObject, GameObject> OnDeath = new UnityEvent<GameObject, GameObject>();
    private float internalCooldown;
    [SerializeField] private float attackInterval;
    [SerializeField] private float attackRange;
    [SerializeField] private bool doesAttackOnAnimationCondition = false;
    [SerializeField] private bool stopOnRange = false;

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

        if (DistanceFromTarget(target.transform.position) >= attackRange || !stopOnRange) Move?.Invoke(this);
        else rb.linearVelocity = Vector3.zero;

        if (DistanceFromTarget(target.transform.position) < attackRange && internalCooldown <= 0 && doesAttackOnAnimationCondition == false)
        {
            OnAttack();
        }

        //Reduces cooldown as a timer.
        if (internalCooldown > 0)
        {
            internalCooldown -= Time.deltaTime;
        }
    }

    public void OnAttack()
    {
        Attack?.Invoke(this.gameObject, GetTarget());
        internalCooldown = attackInterval;
    }

    GameObject GetTarget()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }

    public float GetAngleToTarget(Vector3? targetPosition)
    {
        var differenceFromEnemyToTarget = targetPosition == null ?  target.transform.position - this.gameObject.transform.position : targetPosition.Value - this.gameObject.transform.position;
        return Mathf.Atan2(differenceFromEnemyToTarget.y, differenceFromEnemyToTarget.x) * Mathf.Rad2Deg;
    }
    public float DistanceFromTarget(Vector3? targetPosition)
    {
        var differenceFromEnemyToTarget = targetPosition == null ? target.transform.position - this.gameObject.transform.position : targetPosition.Value - this.gameObject.transform.position;
        return differenceFromEnemyToTarget.magnitude;
    }

    void SpriteFlip()
    {
        float normalizedDifferenceX = 0f;

        if ((target.transform.position - this.transform.position).normalized.x != 0) normalizedDifferenceX = (target.transform.position - this.transform.position).normalized.x;
        Debug.Log(normalizedDifferenceX);

        spriteRenderer.flipX = normalizedDifferenceX >= 0 ? true : false;
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
        ItemData[] items = enemyData.GetIdemDrops(null);
        items = items.Concat(enemyData.GetIdemDrops(droppedItems)).ToArray();
        
        foreach (var item in items)
        {
            GameObject pickup = new GameObject();
            pickup.AddComponent<ItemPickup>();
            pickup.layer = LayerMask.NameToLayer("ItemPickup");
            ItemPickup itemPickup = pickup.GetComponent<ItemPickup>();
            itemPickup.ItemData = item;
            Vector3 offset = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
            Instantiate(pickup, transform.position + offset, Quaternion.Euler(0,0, transform.localRotation.eulerAngles.z));
        }
        Destroy(gameObject);
    }
}
