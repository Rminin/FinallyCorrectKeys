using FinallyCorrectKeys.Util;
using GameNetcodeStuff;
using HarmonyLib;

namespace FinallyCorrectKeys.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
public class PlayerControllerBPatches
{
    [HarmonyPatch(nameof(PlayerControllerB.SetHoverTipAndCurrentInteractTrigger))]
    [HarmonyPostfix]
    private static void SetHoverTipAndCurrentInteractTriggerPatch(PlayerControllerB __instance)
    {
        var cursorTip = __instance.cursorTip;
        if (cursorTip.text.Contains(Bindings.INTERACT.ToHUDFormat()))
        {
            cursorTip.text = BindingReplacer.Replace(cursorTip.text, Bindings.INTERACT.ToHUDFormat(), Bindings.INTERACT);
#if DEBUG
            // This method is called in "Update", only included in Debug to increase performance
            FinallyCorrectKeys.Logger.LogDebug(string.Format("[{0}] Replaced the {1}  binding.", nameof(PlayerControllerBPatches), ActionBindings.interactBinding));
#endif
        }
    }
}
