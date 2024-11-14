using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Attack Dictionary", menuName = "Add Attack Dictionary")]
public class EnemyData : ScriptableObject
{
    public Dictionary<string, UnityAction<EnemyBehaviour>> MovesDictionary = new Dictionary<string, UnityAction<EnemyBehaviour>>();
    public Dictionary<string, UnityAction<GameObject>> AttacksDictionary = new Dictionary<string, UnityAction<GameObject>>();

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
        Debug.Log("Tried to Move to Target");
    }
    #endregion

    #region Attacks
    public void SingleShot(GameObject target)
    {
        Debug.Log("Tried to Shoot Once");
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
    [SerializeField] public UnityAction<GameObject> func;

    public StringActionGO(string name, UnityAction<GameObject> func)
    {
        this.name = name;
        this.func = func;
    }
}
