using HarmonyLib;
using UnityEngine.InputSystem;

namespace FinallyCorrectKeys.Patches;

[HarmonyPatch(typeof(ShipBuildModeManager))]
public class ShipBuildModeManagerPatches
{
    private static readonly InputActionAsset actions = IngamePlayerSettings.Instance.playerInput.actions;

    private static readonly string buildBinding = "BuildMode";
    private static readonly string rotateBinding = "ReloadBatteries";
    private static readonly string storeBinding = "Delete";

    [HarmonyPatch(nameof(ShipBuildModeManager.CreateGhostObjectAndHighlight))]
    [HarmonyPostfix]
    private static void CreateGhostObjectAndHighlight(ShipBuildModeManager __instance)
    {
        string replace = HUDManager.Instance.buildModeControlTip.text;
        replace = replace.Replace("[B]", string.Format("[{0}]", GetInputBinding(buildBinding).ToDisplayString()));
        replace = replace.Replace("[R]", string.Format("[{0}]", GetInputBinding(rotateBinding).ToDisplayString()));
        replace = replace.Replace("[X]", string.Format("[{0}]", GetInputBinding(storeBinding).ToDisplayString()));
        HUDManager.Instance.buildModeControlTip.text = replace;
        FinallyCorrectKeys.Logger.LogDebug(string.Format("Replaced the {0} and {1} and {2} binding.", buildBinding, rotateBinding, storeBinding));
    }

    private static InputBinding GetInputBinding(string actionName)
    {
        return actions.FindAction(actionName).bindings[0];
    }
}
