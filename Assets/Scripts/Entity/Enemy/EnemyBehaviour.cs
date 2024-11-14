using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer),typeof(Rigidbody2D))]
public class EnemyBehaviour : MonoBehaviour
{
    [Header("Actions")]
    [SerializeField] private List<string> movementList;
    [SerializeField] private List<string> attackList;

    [SerializeField] EnemyData enemyData;

    private UnityEvent<EnemyBehaviour> Move = new UnityEvent<EnemyBehaviour>();
    private UnityEvent<GameObject> Attack = new UnityEvent<GameObject>();
    
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
        Attack?.Invoke(GameObject.FindGameObjectWithTag("Player"));
    }
}
