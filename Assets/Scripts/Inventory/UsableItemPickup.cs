using ScriptableObjects;
using UnityEngine;
using UnityEngine.Audio;

namespace Inventory
{
    public class UsableItemPickup : ItemPickup
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            audioSource = this.GetComponent<AudioSource>();
            if (transform.childCount <= 0) return;
            SpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            if (SpriteRenderer && ItemData)
            {
                SpriteRenderer.sprite = ItemData.Icon;
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        protected override void Triggered(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            UsableItemData usableItemData = (UsableItemData)ItemData;
            if (usableItemData)
            {
                PlayerController player = other.gameObject.GetComponent<PlayerController>();
                if (!player.CanHeal()) return;

                audioSource.clip = pickupClip;
                AudioSource.PlayClipAtPoint(pickupClip, transform.position);

                usableItemData.Use(other.gameObject.GetComponent<PlayerController>());
                Destroy(gameObject);
            }
        }
    }
}
