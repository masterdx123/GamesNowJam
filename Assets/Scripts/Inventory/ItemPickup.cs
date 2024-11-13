using System;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

namespace Inventory
{
    [RequireComponent(typeof(CircleCollider2D), typeof(SpriteRenderer))]
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField]
        private ItemData itemData;
        
        private SpriteRenderer _spriteRenderer;
        private CircleCollider2D _circleCollider;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _circleCollider = GetComponent<CircleCollider2D>();
            _circleCollider.isTrigger = true;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer && itemData)
            {
                _spriteRenderer.sprite = itemData.Icon;
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.gameObject.name);
            Destroy(gameObject);
        }
    }
}
