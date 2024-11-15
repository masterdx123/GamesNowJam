using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Tooltip
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            ITooltipable tooltipable = gameObject.GetComponent<ITooltipable>();
            if (tooltipable == null || !tooltipable.CanShow()) return;
            TooltipSystem.Show(tooltipable.GetTitle(), tooltipable.GetDescription());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipSystem.Hide();
        }
    }
}
