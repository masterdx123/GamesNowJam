using System.Collections;
using UnityEngine;

namespace UI.Tooltip
{
    public class TooltipSystem : MonoBehaviour
    {
        private static TooltipSystem _instance;

        [SerializeField]
        private Tooltip tooltip;
        [SerializeField]
        private float tooltipDelay = 0.5f;
        
        private IEnumerator _showTooltipCoroutine;

        public void Awake()
        {
            _instance = this;
        }

        public static void Show(string title, string description)
        {
            _instance.StartCoroutine(title, description);
        }

        public static void Hide()
        {
            _instance.StopCoroutine();
            _instance.tooltip.gameObject.SetActive(false);
        }

        private void StartCoroutine(string title, string description)
        {
            _showTooltipCoroutine = WaitForShowTooltipCoroutine(title, description);
            StartCoroutine(_showTooltipCoroutine);
        }

        private IEnumerator WaitForShowTooltipCoroutine(string title, string description)
        {
            yield return new WaitForSeconds(_instance.tooltipDelay);
            _instance.tooltip.SetText(title, description);
            _instance.tooltip.gameObject.SetActive(true);
        }

        private void StopCoroutine()
        {
            if (_showTooltipCoroutine == null) return;
            StopCoroutine(_showTooltipCoroutine);
        }
    }
}
