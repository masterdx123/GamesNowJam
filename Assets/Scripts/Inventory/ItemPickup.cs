using System;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

namespace Inventory
{
    [RequireComponent(typeof(SphereCollider), typeof(SpriteRenderer))]
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField]
        private ItemData itemData;
        
        private SpriteRenderer _spriteRenderer;
        private SphereCollider _sphereCollider;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _sphereCollider = GetComponent<SphereCollider>();
            _sphereCollider.isTrigger = true;
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

        void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject.name);
            Destroy(this);
        }
    }
}
