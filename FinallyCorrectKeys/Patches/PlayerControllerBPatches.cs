using System;
using FinallyCorrectKeys.Util;
using GameNetcodeStuff;
using HarmonyLib;

namespace FinallyCorrectKeys.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
public class PlayerControllerBPatches
{
    private static DateTime timestamp = DateTime.Now;

    [HarmonyPatch(nameof(PlayerControllerB.SetHoverTipAndCurrentInteractTrigger))]
    [HarmonyPostfix]
    private static void SetHoverTipAndCurrentInteractTriggerPatch(PlayerControllerB __instance)
    {
        var cursorTip = __instance.cursorTip;
        if (!cursorTip.text.Contains(Bindings.INTERACT.ToHUDFormat())) return;

        var oldTime = timestamp;
        timestamp = DateTime.Now;
#if DEBUG
        FinallyCorrectKeys.Logger.LogDebug($"[{nameof(PlayerControllerBPatches)}] TIME OLD: {oldTime.Millisecond}, TIME NOW: {timestamp.Millisecond}");
#endif
        // Check if more than 3 seconds have passed
        if ((timestamp - oldTime) > TimeSpan.FromSeconds(3))
        {
            var currentInputString = ActionBindings.GetInputBindingString(Bindings.INTERACT);
            if (ActionBindings.interactDisplayString.Equals(currentInputString))
            {
                FinallyCorrectKeys.Logger.LogDebug($"[{nameof(PlayerControllerBPatches)}] No change in Interaction Keybind detected.");
            }
            else
            {
                FinallyCorrectKeys.Logger.LogDebug($"[{nameof(PlayerControllerBPatches)}] Updating ActionBindings display string from: {ActionBindings.interactDisplayString} to: {currentInputString}");
                // Update interaction string
                ActionBindings.interactDisplayString = currentInputString;
            }
        }

        cursorTip.text = cursorTip.text.Replace(Bindings.INTERACT.ToHUDFormat(), "[" + ActionBindings.interactDisplayString + "]");
#if DEBUG
        // This method is called in "Update", only included in Debug to increase performance
        FinallyCorrectKeys.Logger.LogDebug(string.Format("[{0}] Replaced the {1}  binding.", nameof(PlayerControllerBPatches), Bindings.INTERACT));
#endif

    }
}
