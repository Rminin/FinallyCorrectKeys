using BepInEx.Logging;
using HarmonyLib;
using UnityEngine.InputSystem;

namespace FinallyCorrectKeys.Patches;

[HarmonyPatch(typeof(HUDManager))]
public class FinallyCorrectKeysPatch
{
    private static readonly InputActionAsset actions = IngamePlayerSettings.Instance.playerInput.actions;

    private static readonly InputBinding discardBinding = actions.FindAction("Discard").bindings[0];
    private static readonly InputBinding useBinding = actions.FindAction("Use").bindings[0];
    private static readonly InputBinding activateItemBinding = actions.FindAction("ActivateItem").bindings[0]; // Don't know where it gets used

    [HarmonyPatch(nameof(HUDManager.ChangeControlTipMultiple))]
    [HarmonyPostfix]
    private static void ChangeControlTipMultiplePatch(HUDManager __instance)
    {
        // Find keybind
        var keybindDiscard = discardBinding.ToDisplayString();
        FinallyCorrectKeys.Logger.LogDebug("ACTION DISCARD DISPLAYNAME: " + keybindDiscard);

        var keybindUse = useBinding.ToDisplayString();
        FinallyCorrectKeys.Logger.LogDebug("ACTION USE DISPLAYNAME: " + keybindUse);

        var keybindActivateItem = activateItemBinding.ToDisplayString();
        FinallyCorrectKeys.Logger.LogDebug("ACTION ACTIVATE_ITEM DISPLAYNAME: " + keybindActivateItem);

        // Change text
        var lines = __instance.controlTipLines;
        for (int i = 0; i < lines.Length; i++)
        {
            string lineText = lines[i].text;
            if (lineText.Contains("[G]"))
            {
                lines[i].text = lineText.Replace("[G]", "[" + keybindDiscard + "]");
                FinallyCorrectKeys.Logger.LogDebug("Replaced the discard binding.");
            }
            else if (lineText.Contains("[LMB]"))
            {
                lines[i].text = lineText.Replace("[LMB]", "[" + keybindActivateItem + "]");
                FinallyCorrectKeys.Logger.LogDebug("Replaced the activate item binding.");
            }
        }
    }
}
