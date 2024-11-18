using UnityEngine;
using Entity.UpgradeConsole;

public interface ITooltipable
{
    public string GetTitle();
    public string GetDescription();
    public MaterialRequirement[] GetMaterialRequirements();

    public bool CanShow();
}
