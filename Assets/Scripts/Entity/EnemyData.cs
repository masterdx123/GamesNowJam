using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Attack Dictionary", menuName = "Add Attack Dictionary")]
public class EnemyData : ScriptableObject
{
    public Dictionary<string, UnityAction<EnemyBehaviour>> MovesDictionary = new Dictionary<string, UnityAction<EnemyBehaviour>>();
    public Dictionary<string, UnityAction<GameObject, GameObject>> AttacksDictionary = new Dictionary<string, UnityAction<GameObject, GameObject>>();

    public GameObject[] AttacksProjectiles;

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
            MovesDictionary.Add(move.name, move.func);
            Debug.Log($"{move.name} loaded into dictionary!");
        }

        foreach (var attack in attacksDictionary)
        {
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
        var attackGo = Instantiate(AttacksProjectiles[0], self.transform.position, Quaternion.Euler(0, 0, selfBehaviour.GetAngleToTarget()));
        WeaponProjectile attackProjectileComponent = attackGo.GetComponent<WeaponProjectile>();
        attackProjectileComponent.angle = selfBehaviour.GetAngleToTarget();
    }
    #endregion
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