using System.Collections;
using UnityEngine;
using Entity.UpgradeConsole;

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

        public static void Show(string title, string description, MaterialRequirement[] materials)
        {
            if (!_instance || !_instance.tooltip)
            {
                Debug.LogError(
                    "No Tooltip Set! Go to Prefabs/UI/Tooltip and drag the TooltipCanvas prefab into the Scene");
                return;
            }
            _instance.StartCoroutine(title, description, materials);
        }

        public static void Hide()
        {
            if (!_instance || !_instance.tooltip)
            {
                Debug.LogError(
                    "No Tooltip Set! Go to Prefabs/UI/Tooltip and drag the TooltipCanvas prefab into the Scene");
                return;
            }
            _instance.StopCoroutine();
            _instance.tooltip.gameObject.SetActive(false);
        }

        private void StartCoroutine(string title, string description,  MaterialRequirement[] materials)
        {
            _showTooltipCoroutine = WaitForShowTooltipCoroutine(title, description, materials);
            StartCoroutine(_showTooltipCoroutine);
        }

        private IEnumerator WaitForShowTooltipCoroutine(string title, string description,  MaterialRequirement[] materials)
        {
            yield return new WaitForSeconds(_instance.tooltipDelay);
            _instance.tooltip.SetText(title, description, materials);
            _instance.tooltip.gameObject.SetActive(true);
        }

        private void StopCoroutine()
        {
            if (_showTooltipCoroutine == null) return;
            StopCoroutine(_showTooltipCoroutine);
        }
    }
}
