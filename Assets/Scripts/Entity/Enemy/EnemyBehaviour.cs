using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer),typeof(Rigidbody2D))]
public class EnemyBehaviour : MonoBehaviour
{
    [Header("Actions")]
    [SerializeField] private List<string> movementList;
    [SerializeField] private List<string> attackList;

    public GameObject target {  get => GetTarget();}

    [SerializeField] EnemyData enemyData;
    [SerializeField] private float speed;
    public float Speed { private set => speed = value; get => speed; }

    private UnityEvent<EnemyBehaviour> Move = new UnityEvent<EnemyBehaviour>();
    private UnityEvent<GameObject, GameObject> Attack = new UnityEvent<GameObject, GameObject>();
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
    }

    // Update is called once per frame
    void Update()
    {
        Move?.Invoke(this);

        if (internalCooldown <= 0)
        {
            Attack?.Invoke(this.gameObject, GameObject.FindGameObjectWithTag("Player"));
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
}
