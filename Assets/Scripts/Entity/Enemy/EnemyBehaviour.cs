using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Inventory;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using Unity.Properties;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(Animator))]
public class EnemyBehaviour : MonoBehaviour, IDamageable
{

    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    private string currentState;

    [Header("Actions")]
    [SerializeField] private List<string> movementList;
    [SerializeField] private List<string> attackList;
    [SerializeField] private bool hasContactDamage;
    public float ContactDamage => _contactDamage;
    [SerializeField] protected float _contactDamage;
    [SerializeField] private List<string> targetList;
    [SerializeField] private List<string> onDeathList;

    [Header("Loot")]
    [SerializeField]
    private DroppedItem[] droppedItems;

    public GameObject target { get; private set; }

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

    [SerializeField]
    private bool spawnAtMachine = false;
    public bool SpawnAtMachine { get => spawnAtMachine; }

    protected UnityEvent<EnemyBehaviour> Move = new UnityEvent<EnemyBehaviour>();
    protected UnityEvent<GameObject, GameObject> Attack = new UnityEvent<GameObject, GameObject>();
    protected UnityEvent<EnemyBehaviour> TargetAcquisition = new UnityEvent<EnemyBehaviour>();
    protected UnityEvent<GameObject, GameObject> OnDeath = new UnityEvent<GameObject, GameObject>();
    protected float internalCooldown;
    [SerializeField] private float attackInterval;
    [SerializeField] private float attackRange;
    [SerializeField] private bool doesAttackOnAnimationCondition = false;
    [SerializeField] private bool doesNotHaveHitAnimation = false;
    [SerializeField] private bool stopOnRange = false;

    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip attackClip;
    private AudioSource audioSource;
    private bool _onHit = false;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        enemyData.Start();

        foreach (var move in movementList)
        {
            Move.AddListener(enemyData.MovesDictionary[move]);
        }
        foreach (var attack in attackList)
        {
            Attack.AddListener(enemyData.AttacksDictionary[attack]);
        }
        foreach (var target in targetList)
        {
            TargetAcquisition.AddListener(enemyData.TargetAcquisitionDictionary[target]);
        }
        foreach (var attack in onDeathList)
        {
            OnDeath.AddListener(enemyData.AttacksDictionary[attack]);
        }

        _health = maxHealth;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (targetList.Count != 0)
        {
            ChangeTarget();
            if (_onHit == false)
            {
                if (DistanceFromTarget(target.transform.position) >= attackRange || !stopOnRange) Move?.Invoke(this);
                else rb.linearVelocity = Vector3.zero;

                if (DistanceFromTarget(target.transform.position) < attackRange && internalCooldown <= 0 && doesAttackOnAnimationCondition == false)
                {
                    OnAttack();
                }
            }
            else { if (rb.bodyType != RigidbodyType2D.Static) rb.linearVelocity = Vector3.zero; }

            //Reduces cooldown as a timer.
            if (internalCooldown > 0)
            {
                internalCooldown -= Time.deltaTime;
            }

            SpriteFlip();
        }
    }

    public virtual void OnAttack()
    {
        Attack?.Invoke(this.gameObject, target);
        internalCooldown = attackInterval;
        audioSource.clip = attackClip;
        audioSource.Play();
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!other.isTrigger && hasContactDamage)
            {
                other.GetComponent<PlayerController>().TakeDamage(_contactDamage);
            }
        }
    }

    public virtual void ChangeTarget()
    {
        TargetAcquisition?.Invoke(this);
    }

    public void SetTarget(GameObject _target)
    {
        if(target != _target) target = _target;
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

        spriteRenderer.flipX = normalizedDifferenceX >= 0 ? true : false;
    }

    public void TakeDamage(float damage)
    {
        audioSource.clip = damageClip;
        audioSource.Play();
        _health = Mathf.Clamp(_health - damage, 0, maxHealth);
        
        if(doesNotHaveHitAnimation == false) ChangeAnimationState("OnHit");
        _onHit = true;

        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke(gameObject, target);
        ItemData[] items = enemyData.GetIdemDrops(null);
        items = items.Concat(enemyData.GetIdemDrops(droppedItems)).ToArray();
        audioSource.clip = deathClip;
        AudioSource.PlayClipAtPoint(deathClip, transform.position);

        foreach (var item in items)
        {
            GameObject pickup = enemyData.GetPickupObject(item);
            ItemPickup itemPickup = pickup.GetComponent<ItemPickup>();
            itemPickup.ItemData = item;
            Vector3 offset = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
            Instantiate(pickup, transform.position + offset, Quaternion.Euler(0,0, transform.localRotation.eulerAngles.z));
        }
        Destroy(gameObject);
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState != newState)
        {
            animator.Play(newState, 0, 0f);
            currentState = newState;
        }
    }

    public void OnHitFinish()
    {
        _onHit = false;
        ChangeAnimationState("Default");
    }
}
