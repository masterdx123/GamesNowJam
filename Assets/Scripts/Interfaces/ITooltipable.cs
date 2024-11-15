using UnityEngine;

public interface ITooltipable
{
    public string GetTitle();
    public string GetDescription();

    public bool CanShow();
}
