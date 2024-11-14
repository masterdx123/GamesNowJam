using Inventory;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class InventorySlotController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public int Slot
        {
            set => _slot = value;
        }

        public ItemData ItemData => _item;
        
        [SerializeField]
        private TextMeshProUGUI itemNameText;
        [SerializeField]
        private Image iconRenderer;
        [SerializeField]
        private GameObject draggableObjectPrefab;
        
        private ItemData _item;
        private int _slot;
        
        private GameObject _draggedObject;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void UpdateItem(int slot, Item? item)
        {
            if (slot != _slot) return;
            if (!item.HasValue || !item.Value.ItemData)
            {
                _item = null;
                iconRenderer.enabled = false;
                itemNameText.text = "";
                return;
            }
            
            iconRenderer.enabled = true;
            _item = item.Value.ItemData;
            iconRenderer.sprite = _item.Icon;
            itemNameText.text = _item.CanStack ? item.Value.Amount.ToString() : "";
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_item) return;
            iconRenderer.enabled = false;
            _draggedObject = Instantiate(draggableObjectPrefab, transform.root);
            DraggableItem draggableItem = _draggedObject.GetComponent<DraggableItem>();
            if (draggableItem)
            {
                draggableItem.SetDraggableData(_item);
            }
            _draggedObject.transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_item || !_draggedObject) return;
            _draggedObject.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_item || !_draggedObject) return;
            iconRenderer.enabled = true;
            Destroy(_draggedObject);
        }
    }
}
