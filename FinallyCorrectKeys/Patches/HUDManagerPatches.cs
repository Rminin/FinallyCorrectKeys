using FinallyCorrectKeys.Util;
using HarmonyLib;
using TMPro;

namespace FinallyCorrectKeys.Patches;

[HarmonyPatch(typeof(HUDManager))]
public class HUDManagerPatches
{
    [HarmonyPatch(nameof(HUDManager.ChangeControlTipMultiple))]
    [HarmonyPostfix]
    private static void ChangeControlTipMultiplePatch(HUDManager __instance)
    {
        ReplaceKeysInControlTipMultiple(__instance.controlTipLines);
    }

    [HarmonyPatch(nameof(HUDManager.ChangeControlTip))]
    [HarmonyPostfix]
    public static void ChangeControlTipPatch(HUDManager __instance, int toolTipNumber, string changeTo)
    {
        FinallyCorrectKeys.Logger.LogDebug("ToolTipNumber: " + toolTipNumber
            + "; Linetext: " + __instance.controlTipLines[toolTipNumber].text 
            + "; ChangeTo: " + changeTo);
        ReplaceKeysInControlTip(__instance.controlTipLines[toolTipNumber]);
    }

    private static void ReplaceKeysInControlTip(TextMeshProUGUI line)
    {
        // Change text
        string lineText = line.text;
        if (lineText.Contains("[G]"))
        {
            line.text = lineText.Replace("[G]", "[" + ActionBindings.GetInputBindingString(ActionBindings.discardBinding) + "]");
            FinallyCorrectKeys.Logger.LogDebug("Replaced the " + ActionBindings.discardBinding + " binding.");
        }
        else if (lineText.Contains("[LMB]"))
        {
            line.text = lineText.Replace("[LMB]", "[" + ActionBindings.GetInputBindingString(ActionBindings.activateItemBinding) + "]");
            FinallyCorrectKeys.Logger.LogDebug("Replaced the " + ActionBindings.activateItemBinding + " binding.");
        }
        else if (lineText.Contains("[Q]"))
        {
            line.text = lineText.Replace("[Q]", "[" + ActionBindings.GetInputBindingString(ActionBindings.secondaryUseBinding) + "]");
            FinallyCorrectKeys.Logger.LogDebug("Replaced the " + ActionBindings.secondaryUseBinding + " binding.");
        }
        else if (lineText.Contains("[E]"))
        {
            line.text = lineText.Replace("[E]", "[" + ActionBindings.GetInputBindingString(ActionBindings.tertiaryUseBinding) + "]");
            FinallyCorrectKeys.Logger.LogDebug("Replaced the " + ActionBindings.tertiaryUseBinding + " binding.");
        }
        else if (lineText.Contains("[Z]"))
        {
            line.text = lineText.Replace("[Z]", "[" + ActionBindings.GetInputBindingString(ActionBindings.inspectItemBinding) + "]");
            FinallyCorrectKeys.Logger.LogDebug("Replaced the " + ActionBindings.inspectItemBinding + " binding.");
        }
        else if (lineText.Contains("[Q/E]")) // In case of clipboard
        {
            string replace = string.Format("[{0}/{1}]", ActionBindings.GetInputBindingString(ActionBindings.secondaryUseBinding), ActionBindings.GetInputBindingString(ActionBindings.tertiaryUseBinding));
            line.text = lineText.Replace("[Q/E]", replace);
            FinallyCorrectKeys.Logger.LogDebug(string.Format("Replaced the {0} and {1} binding.", ActionBindings.secondaryUseBinding, ActionBindings.tertiaryUseBinding));
        }
        else if (lineText.StartsWith("Sprint")) // In case of round starting // DOESN'T WORK 😭
        {
            line.text = lineText.Replace("[Shift]", "[" + ActionBindings.GetInputBindingString(ActionBindings.sprintBinding) + "]");
            FinallyCorrectKeys.Logger.LogDebug("Replaced the " + ActionBindings.sprintBinding + " binding.");
        }
        else if (lineText.StartsWith("Scan")) // In case of round starting // DOESN'T WORK 😭
        {
            line.text = lineText.Replace("[RMB]", "[" + ActionBindings.GetInputBindingString(ActionBindings.scanBinding) + "]");
            FinallyCorrectKeys.Logger.LogDebug("Replaced the " + ActionBindings.scanBinding + " binding.");
        }
    }

    private static void ReplaceKeysInControlTipMultiple(TextMeshProUGUI[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            ReplaceKeysInControlTip(lines[i]);
        }
    }
}
