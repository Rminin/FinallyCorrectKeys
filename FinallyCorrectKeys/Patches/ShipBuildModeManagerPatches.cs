using FinallyCorrectKeys.Util;
using HarmonyLib;

namespace FinallyCorrectKeys.Patches;

[HarmonyPatch(typeof(ShipBuildModeManager))]
public class ShipBuildModeManagerPatches
{
    [HarmonyPatch(nameof(ShipBuildModeManager.CreateGhostObjectAndHighlight))]
    [HarmonyPostfix]
    private static void CreateGhostObjectAndHighlight(ShipBuildModeManager __instance)
    {
        string replace = HUDManager.Instance.buildModeControlTip.text;
        replace = BindingReplacer.Replace(replace, "[B]", ActionBindings.buildBinding);
        replace = BindingReplacer.Replace(replace, "[R]", ActionBindings.rotateBinding);
        replace = BindingReplacer.Replace(replace, "[X]", ActionBindings.storeBinding);
        HUDManager.Instance.buildModeControlTip.text = replace;
        FinallyCorrectKeys.Logger.LogDebug(string.Format("[{0}] Replaced the {1} and {2} and {3} binding."
            , nameof(ShipBuildModeManagerPatches), ActionBindings.buildBinding, ActionBindings.rotateBinding, ActionBindings.storeBinding));
    }
}
