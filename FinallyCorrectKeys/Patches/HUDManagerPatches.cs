using HarmonyLib;
using TMPro;
using UnityEngine.InputSystem;

namespace FinallyCorrectKeys.Patches;

[HarmonyPatch(typeof(HUDManager))]
public class HUDManagerPatches
{
    private static readonly InputActionAsset actions = IngamePlayerSettings.Instance.playerInput.actions;

    private static readonly string discardBinding = "Discard";
    private static readonly string useBinding = "Use"; // Don't know where it gets used
    private static readonly string activateItemBinding = "ActivateItem";
    private static readonly string secondaryUseBinding = "ItemSecondaryUse";
    private static readonly string tertiaryUseBinding = "ItemTertiaryUse";
    private static readonly string inspectItemBinding = "InspectItem";
    private static readonly string sprintBinding = "Sprint";
    private static readonly string scanBinding = "PingScan";

    [HarmonyPatch(nameof(HUDManager.ChangeControlTipMultiple))]
    [HarmonyPostfix]
    private static void ChangeControlTipMultiplePatch(HUDManager __instance)
    {
        ReplaceKeysInControlTipMultiple(__instance.controlTipLines);
    }

    [HarmonyPatch(nameof(HUDManager.ChangeControlTip))]
    [HarmonyPostfix]
    private static void ChangeControlTipPatch(HUDManager __instance, int toolTipNumber)
    {

        ReplaceKeysInControlTip(__instance.controlTipLines[toolTipNumber]);
    }

    private static void ReplaceKeysInControlTip(TextMeshProUGUI line)
    {
        // Change text
        string lineText = line.text;
        if (lineText.Contains("[G]"))
        {
            line.text = lineText.Replace("[G]", "[" + GetInputBinding(discardBinding).ToDisplayString() + "]");
            FinallyCorrectKeys.Logger.LogDebug("Replaced the " + discardBinding + " binding.");
        }
        else if (lineText.Contains("[LMB]"))
        {
            line.text = lineText.Replace("[LMB]", "[" + GetInputBinding(activateItemBinding).ToDisplayString() + "]");
            FinallyCorrectKeys.Logger.LogDebug("Replaced the " + activateItemBinding + " binding.");
        }
        else if (lineText.Contains("[Q]"))
        {
            line.text = lineText.Replace("[Q]", "[" + GetInputBinding(secondaryUseBinding).ToDisplayString() + "]");
            FinallyCorrectKeys.Logger.LogDebug("Replaced the " + secondaryUseBinding + " binding.");
        }
        else if (lineText.Contains("[E]"))
        {
            line.text = lineText.Replace("[E]", "[" + GetInputBinding(tertiaryUseBinding).ToDisplayString() + "]");
            FinallyCorrectKeys.Logger.LogDebug("Replaced the " + tertiaryUseBinding + " binding.");
        }
        else if (lineText.Contains("[Z]"))
        {
            line.text = lineText.Replace("[Z]", "[" + GetInputBinding(inspectItemBinding).ToDisplayString() + "]");
            FinallyCorrectKeys.Logger.LogDebug("Replaced the " + inspectItemBinding + " binding.");
        }
        else if (lineText.Contains("[Q/E]")) // In case of clipboard
        {
            string replace = string.Format("[{0}/{1}]", GetInputBinding(secondaryUseBinding).ToDisplayString(), GetInputBinding(tertiaryUseBinding).ToDisplayString());
            line.text = lineText.Replace("[Q/E]", replace);
            FinallyCorrectKeys.Logger.LogDebug(string.Format("Replaced the {0} and {1} binding.", secondaryUseBinding, tertiaryUseBinding));
        }
        else if (lineText.StartsWith("Sprint")) // In case of round starting
        {
            line.text = lineText.Replace("[Shift]", "[" + GetInputBinding(sprintBinding).ToDisplayString() + "]");
            FinallyCorrectKeys.Logger.LogDebug("Replaced the " + sprintBinding + " binding.");
        }
        else if (lineText.StartsWith("Scan")) // In case of round starting
        {
            line.text = lineText.Replace("[RMB]", "[" + GetInputBinding(scanBinding).ToDisplayString() + "]");
            FinallyCorrectKeys.Logger.LogDebug("Replaced the " + scanBinding + " binding.");
        }
    }

    private static void ReplaceKeysInControlTipMultiple(TextMeshProUGUI[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            ReplaceKeysInControlTip(lines[i]);
        }
    }

    private static InputBinding GetInputBinding(string actionName)
    {
        return actions.FindAction(actionName).bindings[0];
    }
}
