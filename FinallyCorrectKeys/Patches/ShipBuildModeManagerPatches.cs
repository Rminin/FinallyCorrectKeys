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
        replace = replace.Replace("[B]", string.Format("[{0}]", ActionBindings.GetInputBindingString(ActionBindings.buildBinding)));
        replace = replace.Replace("[R]", string.Format("[{0}]", ActionBindings.GetInputBindingString(ActionBindings.rotateBinding)));
        replace = replace.Replace("[X]", string.Format("[{0}]", ActionBindings.GetInputBindingString(ActionBindings.storeBinding)));
        HUDManager.Instance.buildModeControlTip.text = replace;
        FinallyCorrectKeys.Logger.LogDebug(string.Format("Replaced the {0} and {1} and {2} binding."
            , ActionBindings.buildBinding, ActionBindings.rotateBinding, ActionBindings.storeBinding));
    }
}
