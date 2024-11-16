using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    private float damage;
    public float Damage { get => damage; set => damage = value; }
    private GameObject _owner;
    public GameObject _Owner { get => _owner; set => _owner = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 0.3f);  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider && collider.gameObject && _owner && !_owner.CompareTag(collider.gameObject.tag))
        {
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
    }
}
