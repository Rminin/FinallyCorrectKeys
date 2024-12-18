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
        string oldText = line.text;
        if (oldText.Contains("[G]"))
        {
            line.text = BindingReplacer.Replace(oldText, "[G]", ActionBindings.discardBinding);
            FinallyCorrectKeys.Logger.LogDebug("Replaced the " + ActionBindings.discardBinding + " binding.");
        }
        else if (oldText.Contains("[LMB]"))
        {
            line.text = BindingReplacer.Replace(oldText, "[LMB]", ActionBindings.activateItemBinding);
            FinallyCorrectKeys.Logger.LogDebug("Replaced the " + ActionBindings.activateItemBinding + " binding.");
        }
        else if (oldText.Contains("[Q]"))
        {
            line.text = BindingReplacer.Replace(oldText, "[Q]", ActionBindings.secondaryUseBinding);
            FinallyCorrectKeys.Logger.LogDebug("Replaced the " + ActionBindings.secondaryUseBinding + " binding.");
        }
        else if (oldText.Contains("[E]"))
        {
            line.text = BindingReplacer.Replace(oldText, "[E]", ActionBindings.tertiaryUseBinding);
            FinallyCorrectKeys.Logger.LogDebug("Replaced the " + ActionBindings.tertiaryUseBinding + " binding.");
        }
        else if (oldText.Contains("[Z]"))
        {
            line.text = BindingReplacer.Replace(oldText, "[Z]", ActionBindings.inspectItemBinding);
            FinallyCorrectKeys.Logger.LogDebug("Replaced the " + ActionBindings.inspectItemBinding + " binding.");
        }
        else if (oldText.Contains("[Q/E]")) // In case of clipboard
        {
            string replace = string.Format("[{0}/{1}]", ActionBindings.GetInputBindingString(ActionBindings.secondaryUseBinding), ActionBindings.GetInputBindingString(ActionBindings.tertiaryUseBinding));
            line.text = oldText.Replace("[Q/E]", replace);
            FinallyCorrectKeys.Logger.LogDebug(string.Format("Replaced the {0} and {1} binding.", ActionBindings.secondaryUseBinding, ActionBindings.tertiaryUseBinding));
        }
        else if (oldText.StartsWith("Sprint")) // In case of round starting // DOESN'T WORK 😭
        {
            line.text = BindingReplacer.Replace(oldText, "[Shift]", ActionBindings.sprintBinding);
            FinallyCorrectKeys.Logger.LogDebug("Replaced the " + ActionBindings.sprintBinding + " binding.");
        }
        else if (oldText.StartsWith("Scan")) // In case of round starting // DOESN'T WORK 😭
        {
            line.text = BindingReplacer.Replace(oldText, "[RMB]", ActionBindings.scanBinding);
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
