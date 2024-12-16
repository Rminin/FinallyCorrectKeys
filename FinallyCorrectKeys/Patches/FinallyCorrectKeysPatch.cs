using BepInEx.Logging;
using HarmonyLib;
using UnityEngine.InputSystem;

namespace FinallyCorrectKeys.Patches;

[HarmonyPatch(typeof(HUDManager))]
public class FinallyCorrectKeysPatch
{
    private static readonly InputActionAsset actions = IngamePlayerSettings.Instance.playerInput.actions;

    [HarmonyPatch(nameof(HUDManager.ChangeControlTipMultiple))]
    [HarmonyPostfix]
    private static void DropKeysFix(HUDManager __instance)
    {
        // Find keybind
        var keybindDiscard = actions.FindAction("Discard").bindings[0].ToDisplayString();
        FinallyCorrectKeys.Logger.LogDebug("ACTION DISCARD DISPLAYNAME: " + keybindDiscard);

        var keybindUse = actions.FindAction("Use").bindings[0].ToDisplayString();
        FinallyCorrectKeys.Logger.LogDebug("ACTION USE DISPLAYNAME: " + keybindUse);

        var keybindActivateItem = actions.FindAction("ActivateItem").bindings[0].ToDisplayString();
        FinallyCorrectKeys.Logger.LogDebug("ACTION ACTIVATE_ITEM DISPLAYNAME: " + keybindActivateItem);

        // Change text
        var lines = __instance.controlTipLines;
        for (int i = 0; i < lines.Length; i++)
        {
            string lineText = lines[i].text;
            if (lineText.StartsWith("Drop"))
            {
                lines[i].text = lineText.Replace("[G]", "[" + keybindDiscard + "]");
                FinallyCorrectKeys.Logger.LogDebug("Replaced the discard binding.");
            }
            else if (lineText.StartsWith("Use") || lineText.StartsWith("Swing") || lineText.StartsWith("Toggle")
                || lineText.StartsWith("Throw") || lineText.StartsWith("Pop up"))
            {
                lines[i].text = lineText.Replace("[LMB]", "[" + keybindActivateItem + "]");
                FinallyCorrectKeys.Logger.LogDebug("Replaced the activate item binding.");
            }
            else if (lineText.StartsWith("Use"))
            {
                lines[i].text = lineText.Replace("[LMB]", "[" + keybindUse + "]");
                FinallyCorrectKeys.Logger.LogDebug("Replaced the use binding.");
            }
        }
    }
}
