using System;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

namespace Inventory
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(CircleCollider2D), typeof(SpriteRenderer))]
    public class ItemPickup : MonoBehaviour
    {
        public ItemData ItemData
        {
            private get => itemData;
            set => itemData = value;
        }

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
            if (!other.CompareTag("Player")) return;
            
            // Add to inventory and if successfull destroy
            Inventory inventory = other.gameObject.GetComponent<Inventory>();
            if (!inventory) return;

            if (inventory.AddItem(itemData))
            {
                Destroy(gameObject);
            }
        }
    }
}
