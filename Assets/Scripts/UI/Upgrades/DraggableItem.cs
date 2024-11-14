using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Upgrades
{
    public class DraggableItem : MonoBehaviour
    {
        public UpgradeData UpgradeData => _upgradeData;
        
        private UpgradeData _upgradeData;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetDraggableData(UpgradeData upgradeData)
        {
            _upgradeData = upgradeData;
            Image draggedIcon = gameObject.GetComponent<Image>();
            if (draggedIcon)
            {
                draggedIcon.sprite = _upgradeData.Icon;
                // Strut shenanigans
                Color tempColor = draggedIcon.color;
                tempColor.a = 0.5f;
                draggedIcon.color = tempColor;
            }
        }
    }
}
