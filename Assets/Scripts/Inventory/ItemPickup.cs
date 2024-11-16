using System;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

namespace Inventory
{

    [ExecuteInEditMode]
    [RequireComponent(typeof(CircleCollider2D))]


    public class ItemPickup : MonoBehaviour
    {
        public ItemData ItemData
        {
            get => itemData;
            set => itemData = value;
        }

        [SerializeField]
        private ItemData itemData;
        
        protected SpriteRenderer SpriteRenderer;
        private CircleCollider2D _circleCollider;

        [SerializeField] protected AudioClip pickupClip;
        protected AudioSource audioSource;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            audioSource = this.GetComponent<AudioSource>();
            _circleCollider = GetComponent<CircleCollider2D>();
            _circleCollider.isTrigger = true;
            if (transform.childCount <= 0) return;
            SpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            if (SpriteRenderer && itemData)
            {
                SpriteRenderer.sprite = itemData.Icon;
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        protected void OnTriggerEnter2D(Collider2D other)
        {
            Triggered(other);
        }

        protected virtual void Triggered(Collider2D other)
        {
            Debug.Log("PARENT");
            if (!other.CompareTag("Player")) return;
            
            // Add to inventory and if successfull destroy
            Inventory inventory = other.gameObject.GetComponent<Inventory>();
            if (!inventory) return;

            if (inventory.AddItem(itemData))
            {
                audioSource.clip = pickupClip;
                AudioSource.PlayClipAtPoint(pickupClip, transform.position);
                Destroy(gameObject);
            }
        }
    }
}
