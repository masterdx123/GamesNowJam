using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using ScriptableObjects;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Math = Unity.Mathematics.Geometry.Math;
using Random = UnityEngine.Random;

[Serializable]
public struct DroppedItem
{
    public ItemData itemData;
    public float dropChance;
}

[CreateAssetMenu(fileName = "Attack Dictionary", menuName = "Add Attack Dictionary")]
public class EnemyData : ScriptableObject
{
    public Dictionary<string, UnityAction<EnemyBehaviour>> MovesDictionary = new Dictionary<string, UnityAction<EnemyBehaviour>>();
    public Dictionary<string, UnityAction<GameObject, GameObject>> AttacksDictionary = new Dictionary<string, UnityAction<GameObject, GameObject>>();

    public GameObject[] AttacksProjectiles;
    
    [SerializeField]
    private DroppedItem[] droppedItems;

    public void Start()
    {
        List<StringActionEB> movesDictionary = new List<StringActionEB>(){
            new StringActionEB("MoveToTarget", MoveToTarget)
        };

        List<StringActionGO> attacksDictionary = new List<StringActionGO>()
        {
            new StringActionGO("SingleShot", SingleShot)
        };

        foreach (var move in movesDictionary)
        {
            // TODO: Check if this is a valid fix. This "fixes" an error that happens if multiple enemies are spawned.
            if (MovesDictionary.ContainsKey(move.name)) continue;
            MovesDictionary.Add(move.name, move.func);
            Debug.Log($"{move.name} loaded into dictionary!");
        }

        foreach (var attack in attacksDictionary)
        {
            // TODO: Check if this is a valid fix. This "fixes" an error that happens if multiple enemies are spawned.
            if (AttacksDictionary.ContainsKey(attack.name)) continue;
            AttacksDictionary.Add(attack.name, attack.func);
            Debug.Log($"{attack.name} loaded into dictionary!");
        }
    }



    #region Movements
    public void MoveToTarget(EnemyBehaviour enemy)
    {
        Rigidbody2D rigidbody = enemy.GetComponent<Rigidbody2D>();

        float angle = enemy.GetAngleToTarget();
        Vector2 velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * enemy.Speed;

        rigidbody.linearVelocity = velocity;
    }
    #endregion

    #region Attacks
    public void SingleShot(GameObject self, GameObject target)
    {
        var selfBehaviour = self.GetComponent<EnemyBehaviour>();
        ShootSpread(1, 0, AttacksProjectiles[0], selfBehaviour.GetAngleToTarget(), selfBehaviour);
    }
    #endregion

    #region Drops
    public ItemData[] GetIdemDrops([CanBeNull] DroppedItem[] inDroppedItems)
    {
        DroppedItem[] itemsToAnalyze = inDroppedItems ?? droppedItems;
        List<ItemData> drops = new List<ItemData>();
        for (int i = 0; i < itemsToAnalyze.Length; i++)
        {
            if (itemsToAnalyze[i].dropChance >= Random.Range(0.0f, 100.0f))
            {
                drops.Add(itemsToAnalyze[i].itemData);
            }
        }
        
        return drops.ToArray();
    }
    #endregion
    void ShootSpread(int numOfShotsNonInclusive, float spreadAngle, GameObject projectile, float angleToTarget, EnemyBehaviour selfBehaviour)
    {
        float leftMostPoint = angleToTarget - (spreadAngle/2);

        if (numOfShotsNonInclusive > 1)
        {
            float deltaSpread = spreadAngle/numOfShotsNonInclusive;
            for (int i = 0; i < numOfShotsNonInclusive; i++)
            {
                var attackGo = Instantiate(AttacksProjectiles[0], selfBehaviour.gameObject.transform.position, Quaternion.Euler(0, 0, leftMostPoint + deltaSpread * i));
                WeaponProjectile attackProjectileComponent = attackGo.GetComponent<WeaponProjectile>();
                attackProjectileComponent.Owner = selfBehaviour.gameObject;
                attackProjectileComponent.angle = selfBehaviour.GetAngleToTarget();
            }
        }
        else
        {
            var attackGo = Instantiate(AttacksProjectiles[0], selfBehaviour.gameObject.transform.position, Quaternion.Euler(0, 0, selfBehaviour.GetAngleToTarget()));
            WeaponProjectile attackProjectileComponent = attackGo.GetComponent<WeaponProjectile>();
            attackProjectileComponent.Owner = selfBehaviour.gameObject;
            attackProjectileComponent.angle = selfBehaviour.GetAngleToTarget();
        }
    }
}
[Serializable]
struct StringActionEB
{
    public string name;
    [SerializeField] public UnityAction<EnemyBehaviour> func;

    public StringActionEB(string name, UnityAction<EnemyBehaviour> func)
    {
        this.name = name;
        this.func = func;
    }
}
[Serializable]
struct StringActionGO
{
    public string name;
    [SerializeField] public UnityAction<GameObject, GameObject> func;

    public StringActionGO(string name, UnityAction<GameObject, GameObject> func)
    {
        this.name = name;
        this.func = func;
    }
}