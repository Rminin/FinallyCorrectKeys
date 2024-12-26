using FinallyCorrectKeys.Configuration;
using FinallyCorrectKeys.Util;
using HarmonyLib;
using TMPro;

namespace FinallyCorrectKeys.Patches;

[HarmonyPatch(typeof(HUDManager))]
public class HUDManagerPatches
{
    private static HUDManager _instance = null!;

    [HarmonyPatch(nameof(HUDManager.Awake))]
    [HarmonyPostfix]
    public static void AwakePatch(HUDManager __instance)
    {
        _instance = __instance;

        ApplyWordWrapConfig();
        FinallyCorrectKeys.Logger.LogDebug(string.Format("[{0}] Changed word wrapping of controlTipLines.", nameof(HUDManagerPatches)));
    }

    [HarmonyPatch(nameof(HUDManager.ChangeControlTipMultiple))]
    [HarmonyPostfix]
    private static void ChangeControlTipMultiplePatch(HUDManager __instance)
    {
        if (Config.disableControlTips.Value)
        {
            HideControlTips(__instance.controlTipLines);
            return;
        }

        ReplaceKeysInControlTipMultiple(__instance.controlTipLines);
    }

    [HarmonyPatch(nameof(HUDManager.ChangeControlTip))]
    [HarmonyPostfix]
    public static void ChangeControlTipPatch(HUDManager __instance, int toolTipNumber, string changeTo)
    {
        if (Config.disableControlTips.Value)
        {
            HideControlTips(__instance.controlTipLines);
            return;
        }

        FinallyCorrectKeys.Logger.LogDebug(string.Format("[{0}] ToolTipNumber: {1}; Linetext: {2}; ChangeTo: {3}",
            nameof(HUDManagerPatches), toolTipNumber, __instance.controlTipLines[toolTipNumber].text, changeTo));
        ReplaceKeysInControlTip(__instance.controlTipLines[toolTipNumber]);
    }

    public static void ApplyWordWrapConfig()
    {
        foreach (var controlTipLine in _instance.controlTipLines)
        {
            controlTipLine.enableWordWrapping = !Config.disableWordWrap.Value;
        }
    }

    private static void ReplaceKeysInControlTip(TextMeshProUGUI line)
    {
        // Change text
        string oldText = line.text;
        if (string.IsNullOrWhiteSpace(oldText)) return;

        if (oldText.Contains("[G]"))
        {
            line.text = BindingReplacer.Replace(oldText, "[G]", ActionBindings.discardBinding);
            FinallyCorrectKeys.Logger.LogDebug(string.Format("[{0}] Replaced the {1}  binding.", nameof(HUDManagerPatches), ActionBindings.discardBinding));
        }
        else if (oldText.Contains("[LMB]"))
        {
            line.text = BindingReplacer.Replace(oldText, "[LMB]", ActionBindings.activateItemBinding);
            FinallyCorrectKeys.Logger.LogDebug(string.Format("[{0}] Replaced the {1}  binding.", nameof(HUDManagerPatches), ActionBindings.activateItemBinding));
        }
        else if (oldText.Contains("[Q]"))
        {
            line.text = BindingReplacer.Replace(oldText, "[Q]", ActionBindings.secondaryUseBinding);
            FinallyCorrectKeys.Logger.LogDebug(string.Format("[{0}] Replaced the {1}  binding.", nameof(HUDManagerPatches), ActionBindings.secondaryUseBinding));
        }
        else if (oldText.Contains("[E]"))
        {
            line.text = BindingReplacer.Replace(oldText, "[E]", ActionBindings.tertiaryUseBinding);
            FinallyCorrectKeys.Logger.LogDebug(string.Format("[{0}] Replaced the {1}  binding.", nameof(HUDManagerPatches), ActionBindings.tertiaryUseBinding));
        }
        else if (oldText.Contains("[Z]"))
        {
            line.text = BindingReplacer.Replace(oldText, "[Z]", ActionBindings.inspectItemBinding);
            FinallyCorrectKeys.Logger.LogDebug(string.Format("[{0}] Replaced the {1}  binding.", nameof(HUDManagerPatches), ActionBindings.inspectItemBinding));
        }
        else if (oldText.Contains("[Q/E]")) // In case of clipboard
        {
            string replace = string.Format("[{0}/{1}]", ActionBindings.GetInputBindingString(ActionBindings.secondaryUseBinding), ActionBindings.GetInputBindingString(ActionBindings.tertiaryUseBinding));
            line.text = oldText.Replace("[Q/E]", replace);
            FinallyCorrectKeys.Logger.LogDebug(string.Format("[{0}] Replaced the {1} and {2} binding.", nameof(HUDManagerPatches), ActionBindings.secondaryUseBinding, ActionBindings.tertiaryUseBinding));
        }
        else if (oldText.StartsWith("Sprint")) // In case of round starting // DOESN'T WORK 😭
        {
            line.text = BindingReplacer.Replace(oldText, "[Shift]", ActionBindings.sprintBinding);
            FinallyCorrectKeys.Logger.LogDebug(string.Format("[{0}] Replaced the {1}  binding.", nameof(HUDManagerPatches), ActionBindings.sprintBinding));
        }
        else if (oldText.StartsWith("Scan")) // In case of round starting // DOESN'T WORK 😭
        {
            line.text = BindingReplacer.Replace(oldText, "[RMB]", ActionBindings.scanBinding);
            FinallyCorrectKeys.Logger.LogDebug(string.Format("[{0}] Replaced the {1}  binding.", nameof(HUDManagerPatches), ActionBindings.scanBinding));
        }
    }

    private static void ReplaceKeysInControlTipMultiple(TextMeshProUGUI[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            ReplaceKeysInControlTip(lines[i]);
        }
    }
    
    private static void HideControlTips(TextMeshProUGUI[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].text = "";
        }
    }
}
