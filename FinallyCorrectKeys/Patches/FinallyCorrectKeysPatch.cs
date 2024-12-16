using BepInEx.Logging;
using HarmonyLib;
using UnityEngine.InputSystem;

namespace FinallyCorrectKeys.Patches;

[HarmonyPatch(typeof(HUDManager))]
public class FinallyCorrectKeysPatch
{
    private static readonly InputActionAsset actions = IngamePlayerSettings.Instance.playerInput.actions;

    private static readonly string discardBinding = "Discard";
    private static readonly string useBinding = "Use"; // Don't know where it gets used
    private static readonly string activateItemBinding = "ActivateItem";
    private static readonly string secondaryUseBinding = "ItemSecondaryUse";
    private static readonly string tertiaryUseBinding = "ItemTertiaryUse";
    private static readonly string inspectItemBinding = "InspectItem";

    [HarmonyPatch(nameof(HUDManager.ChangeControlTipMultiple))]
    [HarmonyPostfix]
    private static void ChangeControlTipMultiplePatch(HUDManager __instance)
    {
        // Change text
        var lines = __instance.controlTipLines;
        for (int i = 0; i < lines.Length; i++)
        {
            string lineText = lines[i].text;
            if (lineText.Contains("[G]"))
            {
                lines[i].text = lineText.Replace("[G]", "[" + GetInputBinding(discardBinding).ToDisplayString() + "]");
                FinallyCorrectKeys.Logger.LogDebug("Replaced the " + discardBinding + " binding.");
            }
            else if (lineText.Contains("[LMB]"))
            {
                lines[i].text = lineText.Replace("[LMB]", "[" + GetInputBinding(activateItemBinding).ToDisplayString() + "]");
                FinallyCorrectKeys.Logger.LogDebug("Replaced the " + activateItemBinding + " binding.");
            }
            else if (lineText.Contains("[Q]"))
            {
                lines[i].text = lineText.Replace("[Q]", "[" + GetInputBinding(secondaryUseBinding).ToDisplayString() + "]");
                FinallyCorrectKeys.Logger.LogDebug("Replaced the " + secondaryUseBinding + " binding.");
            }
            else if (lineText.Contains("[E]"))
            {
                lines[i].text = lineText.Replace("[E]", "[" + GetInputBinding(tertiaryUseBinding).ToDisplayString() + "]");
                FinallyCorrectKeys.Logger.LogDebug("Replaced the " + tertiaryUseBinding + " binding.");
            }
        }
    }

    private static InputBinding GetInputBinding(string actionName)
    {
        return actions.FindAction(actionName).bindings[0];
    }
}
