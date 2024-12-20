using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class BogFlowerBehaviour : MonoBehaviour, IDamageable
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] FlowerGrowthSystem flowerGrowthSystem;
    
    [Header("Loot")]
    [SerializeField]
    private DroppedItem[] droppedItems;

    private Transform bogFlowerPoison;

    [SerializeField] EnemyData enemyData;
    [SerializeField] private float maxHealth = 50.0f;
    private float health;
    private bool isMature;
    private bool isPoisoned;

    void Start()
    {
        bogFlowerPoison = transform.GetChild(0);
        isMature = false;
    }

    void Update()
    {
        if (!isMature)
        {
            CheckGrowth();
        }
        else
        {
            ReleasePoison();
        }

    }

    public void TakeDamage(float damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        ItemData[] items = enemyData.GetIdemDrops(null);
        items = items.Concat(enemyData.GetIdemDrops(droppedItems)).ToArray();
        
        foreach (var item in items)
        {
            Instantiate(item, transform.position, Quaternion.Euler(0,0, transform.localRotation.eulerAngles.z));
        }
        Destroy(gameObject);
    }

    private void CheckGrowth()
    {
        if (flowerGrowthSystem.GetGrowth())
        {
            isMature = true;
        }
    }

    private void ReleasePoison()
    {
        GameObject childObject = bogFlowerPoison.gameObject;
        childObject.SetActive(true);
    }
}
