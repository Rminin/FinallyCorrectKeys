using FinallyCorrectKeys.Util;
using HarmonyLib;

namespace FinallyCorrectKeys.Patches;

[HarmonyPatch(typeof(ShotgunItem))]
public class ShotgunItemPatches
{
    [HarmonyPatch(nameof(ShotgunItem.SetControlTipsForItem))]
    [HarmonyPostfix]
    private static void SetControlTipsForItemPatch(ShotgunItem __instance)
    {
        __instance.itemProperties.toolTips[2] = BindingReplacer.Replace(__instance.itemProperties.toolTips[2], "[Q]", ActionBindings.secondaryUseBinding);
        FinallyCorrectKeys.Logger.LogDebug(string.Format("[{0}] Replaced the {1}  binding.", nameof(ShotgunItemPatches), ActionBindings.secondaryUseBinding));
        HUDManager.Instance.ChangeControlTipMultiple(__instance.itemProperties.toolTips, holdingItem: true, __instance.itemProperties);
    }

    [HarmonyPatch(nameof(ShotgunItem.SetSafetyControlTip))]
    [HarmonyPostfix]
    private static void SetSafetyControlTipPatch(ShotgunItem __instance)
    {
        string changeTo = ((!__instance.safetyOn) ? "Turn safety on: " : "Turn safety off: ");
        changeTo = changeTo + "[" + ActionBindings.GetInputBindingString(ActionBindings.secondaryUseBinding) + "]";
        if (__instance.IsOwner)
        {
            FinallyCorrectKeys.Logger.LogDebug(string.Format("[{0}] Replaced the {1}  binding.", nameof(ShotgunItemPatches), ActionBindings.secondaryUseBinding));
            FinallyCorrectKeys.Logger.LogDebug(string.Format("[{0}] Calling ChangeControlTip.", nameof(ShotgunItemPatches)));
            HUDManager.Instance.ChangeControlTip(3, changeTo);
        }
    }
}
