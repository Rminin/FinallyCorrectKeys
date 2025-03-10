﻿using FinallyCorrectKeys.Util;
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
        replace = BindingReplacer.Replace(replace, Bindings.BUILD.ToHUDFormat(), Bindings.BUILD);
        replace = BindingReplacer.Replace(replace, Bindings.ROTATE.ToHUDFormat(), Bindings.ROTATE);
        replace = BindingReplacer.Replace(replace, Bindings.STORE.ToHUDFormat(), Bindings.STORE);
        HUDManager.Instance.buildModeControlTip.text = replace;
        FinallyCorrectKeys.Logger.LogDebug(string.Format("[{0}] Replaced the {1} and {2} and {3} binding."
            , nameof(ShipBuildModeManagerPatches), Bindings.BUILD, Bindings.ROTATE, Bindings.STORE));
    }
}
