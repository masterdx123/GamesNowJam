using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Entity.UpgradeConsole;

namespace UI.Tooltip
{
    [ExecuteInEditMode]
    public class Tooltip : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI titleField;
        [SerializeField]
        private TextMeshProUGUI descriptionField;
        [SerializeField]
        private Image material1;
        [SerializeField]
        private TextMeshProUGUI material1Quantity;
        [SerializeField]
        private Image material2;
        [SerializeField]
        private TextMeshProUGUI material2Quantity;
        [SerializeField]
        private Image material3;
        [SerializeField]
        private TextMeshProUGUI material3Quantity;
        [SerializeField]
        private Image material4;
        [SerializeField]
        private TextMeshProUGUI material4Quantity;
        [SerializeField]
        private TextMeshProUGUI cheatText;

        [SerializeField]
        private LayoutElement layoutElement;
        [SerializeField]
        private int characterWrapLimit;

        [SerializeField] 
        private Vector2 tooltipOffset;
        
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Application.isEditor)
            {
                UpdateSize();
            }
            
            Vector2 position = Input.mousePosition;
            
            float pivotX = (position.x + tooltipOffset.x) / Screen.width;
            float pivotY = (position.y + tooltipOffset.y) / Screen.height;
            
            _rectTransform.pivot = new Vector2(pivotX, pivotY);
            transform.position = position;
        }

        public void SetText(string title, string description, MaterialRequirement[] materials)
        {
            titleField.text = title;
            descriptionField.text = description;

            if(materials == null || materials.Length < 1) {
                material1.color = Color.clear;

                material1Quantity.text = "";

                cheatText.text = "";
            }
            else 
            {
                material1.color = Color.white;
                material1.sprite = materials[0].itemData.Icon;

                material1Quantity.text = "x" + materials[0].quantity;

                cheatText.text = "_____";
            }

            if(materials == null || materials.Length < 2) {
                material2.color = Color.clear;

                material2Quantity.text = "";
            }
            else 
            {
                material2.color = Color.white;
                material2.sprite = materials[1].itemData.Icon;

                material2Quantity.text = "x" + materials[1].quantity;

                cheatText.text = "__________";
            }

            if(materials == null || materials.Length < 3) {
                material3.color = Color.clear;

                material3Quantity.text = "";
            }
            else 
            {
                material3.color = Color.white;
                material3.sprite = materials[2].itemData.Icon;

                material3Quantity.text = "x" + materials[2].quantity;

                cheatText.text = "_______________";
            }

            if(materials == null || materials.Length < 4) {
                material4.color = Color.clear;

                material4Quantity.text = "";
            }
            else 
            {
                material4.color = Color.white;
                material4.sprite = materials[3].itemData.Icon;

                material4Quantity.text = "x" + materials[3].quantity;

                cheatText.text = "____________________";
            }
            UpdateSize();
        }

        private void UpdateSize()
        {
            int titleLength = titleField.text.Length;
            int descriptionLength = descriptionField.text.Length;
            
            layoutElement.enabled = titleLength > characterWrapLimit || descriptionLength > characterWrapLimit;
        }
    }
}
