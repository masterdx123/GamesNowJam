using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class DraggableItem : MonoBehaviour
    {
        public ItemData ItemData => _itemData;
        
        private ItemData _itemData;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetDraggableData(ItemData itemData)
        {
            _itemData = itemData;
            Image draggedIcon = gameObject.GetComponent<Image>();
            if (draggedIcon)
            {
                draggedIcon.sprite = _itemData.Icon;
                // Strut shenanigans
                Color tempColor = draggedIcon.color;
                tempColor.a = 0.5f;
                draggedIcon.color = tempColor;
            }
        }
    }
}
