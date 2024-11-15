using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        public void SetText(string title, string description)
        {
            titleField.text = title;
            descriptionField.text = description;
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
