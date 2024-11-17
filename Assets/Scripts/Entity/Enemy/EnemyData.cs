using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Library;
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
    public Dictionary<string, UnityAction<EnemyBehaviour>> TargetAcquisitionDictionary = new Dictionary<string, UnityAction<EnemyBehaviour>>();

    public GameObject[] AttacksProjectiles;
    
    [SerializeField]
    private DroppedItem[] droppedItems;

    [SerializeField]
    private GameObject itemPickupObject;
    [SerializeField]
    private GameObject usableItemPickupObject;

    public void Start()
    {
        List<StringActionEB> movesDictionary = new List<StringActionEB>(){
            new StringActionEB("MoveToTarget", MoveToTarget)
        };

        List<StringActionGOGO> attacksDictionary = new List<StringActionGOGO>()
        {
            new StringActionGOGO("SingleShot", SingleShot),
            new StringActionGOGO("PoisonCloud", PoisonCloud)
        };
        List<StringActionEB> targetsDictionary = new List<StringActionEB>(){
            new StringActionEB("Player", Player)
        };

        foreach (var move in movesDictionary)
        {
            // TODO: Check if this is a valid fix. This "fixes" an error that happens if multiple enemies are spawned.
            if (MovesDictionary.ContainsKey(move.name)) continue;
            MovesDictionary.Add(move.name, move.func);
            //Debug.Log($"{move.name} loaded into dictionary!");
        }

        foreach (var attack in attacksDictionary)
        {
            // TODO: Check if this is a valid fix. This "fixes" an error that happens if multiple enemies are spawned.
            if (AttacksDictionary.ContainsKey(attack.name)) continue;
            AttacksDictionary.Add(attack.name, attack.func);
            //Debug.Log($"{attack.name} loaded into dictionary!");
        }

        foreach (var target in targetsDictionary)
        {
            // TODO: Check if this is a valid fix. This "fixes" an error that happens if multiple enemies are spawned.
            if (TargetAcquisitionDictionary.ContainsKey(target.name)) continue;
            TargetAcquisitionDictionary.Add(target.name, target.func);
            //Debug.Log($"{target.name} loaded into dictionary!");
        }
    }



    #region Movements
    public void MoveToTarget(EnemyBehaviour self)
    {
        Rigidbody2D rigidbody = self.GetComponent<Rigidbody2D>();

        float angle = self.GetAngleToTarget(null);
        Vector2 velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * self.Speed;

        rigidbody.linearVelocity = velocity;
    }
    #endregion

    #region Attacks
    public void SingleShot(GameObject self, GameObject target)
    {
        var selfBehaviour = self.GetComponent<EnemyBehaviour>();
        FunctionLibrary.ShootSpread(
            1,
            0, 
            AttacksProjectiles[0], 
            selfBehaviour.GetAngleToTarget(target.transform.position),
            selfBehaviour.gameObject.transform.position,
            selfBehaviour.gameObject,
            selfBehaviour.GetAngleToTarget(target.transform.position),
            null
            );
    }
    public void PoisonCloud(GameObject self, GameObject target)
    {
        Instantiate(AttacksProjectiles[1], self.transform.position, Quaternion.identity, self.transform);
    }
    #endregion

    #region TargetAcquisitons
    public void Player(EnemyBehaviour self)
    {
        self.SetTarget(GameObject.FindGameObjectWithTag("Player"));
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

    public GameObject GetPickupObject(ItemData itemData)
    {
        return itemData is UsableItemData ? usableItemPickupObject : itemPickupObject;
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
struct StringActionGOGO
{
    public string name;
    [SerializeField] public UnityAction<GameObject, GameObject> func;

    public StringActionGOGO(string name, UnityAction<GameObject, GameObject> func)
    {
        this.name = name;
        this.func = func;
    }
}
