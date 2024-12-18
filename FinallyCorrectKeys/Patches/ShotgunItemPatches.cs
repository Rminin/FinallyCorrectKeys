using HarmonyLib;
using UnityEngine.InputSystem;

namespace FinallyCorrectKeys.Patches;

[HarmonyPatch(typeof(ShotgunItem))]
public class ShotgunItemPatches
{
    private static readonly InputActionAsset actions = IngamePlayerSettings.Instance.playerInput.actions;

    private static readonly string secondaryUseBinding = "ItemSecondaryUse";

    [HarmonyPatch(nameof(ShotgunItem.SetControlTipsForItem))]
    [HarmonyPostfix]
    private static void SetControlTipsForItemPatch(ShotgunItem __instance)
    {
        __instance.itemProperties.toolTips[2] = __instance.itemProperties.toolTips[2].Replace("[Q]", "[" + GetInputBinding(secondaryUseBinding).ToDisplayString() + "]");
        FinallyCorrectKeys.Logger.LogDebug(nameof(ShotgunItemPatches) + ": Replaced the " + secondaryUseBinding + " binding.");
        HUDManager.Instance.ChangeControlTipMultiple(__instance.itemProperties.toolTips, holdingItem: true, __instance.itemProperties);
    }

    [HarmonyPatch(nameof(ShotgunItem.SetSafetyControlTip))]
    [HarmonyPostfix]
    private static void SetSafetyControlTipPatch(ShotgunItem __instance)
    {
        string changeTo = ((!__instance.safetyOn) ? "Turn safety on: " : "Turn safety off: ");
        changeTo = changeTo + "[" + GetInputBinding(secondaryUseBinding).ToDisplayString() + "]";
        if (__instance.IsOwner)
        {
            FinallyCorrectKeys.Logger.LogDebug(nameof(ShotgunItemPatches) + ": Replaced the " + secondaryUseBinding + " binding.");
            FinallyCorrectKeys.Logger.LogDebug(nameof(ShotgunItemPatches) + ": Calling ChangeControlTip.");
            HUDManager.Instance.ChangeControlTip(3, changeTo);
        }
    }

    private static InputBinding GetInputBinding(string actionName)
    {
        return actions.FindAction(actionName).bindings[0];
    } 
}
