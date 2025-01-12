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

        if (Config.wordWrap.Value == Config.WordWrapOption.Disabled)
        {
            SetWordWrap(false);
        }
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

        FinallyCorrectKeys.Logger.LogDebug(string.Format("[{0}] Parameters of ChangeControlTip:\nToolTipNumber: {1}\nChangeTo: {2}",
            nameof(HUDManagerPatches), toolTipNumber, changeTo));
        ReplaceKeysInControlTip(__instance.controlTipLines[toolTipNumber]);
        
        if (__instance.forceChangeTextCoroutine != null)
        {
            __instance.StopCoroutine(__instance.forceChangeTextCoroutine);
            FinallyCorrectKeys.Logger.LogDebug($"[{nameof(HUDManagerPatches)}] Coroutine to force text change stopped.");
        }
        TextMeshProUGUI line = __instance.controlTipLines[toolTipNumber];
        __instance.forceChangeTextCoroutine = __instance.StartCoroutine(__instance.ForceChangeText(line, line.text));
        FinallyCorrectKeys.Logger.LogDebug($"[{nameof(HUDManagerPatches)}] Coroutine to force text change started.");
    }

    public static void SetWordWrap(bool enabled)
    {
        foreach (var controlTipLine in _instance.controlTipLines)
        {
            controlTipLine.enableWordWrapping = enabled;
        }
        FinallyCorrectKeys.Logger.LogDebug($"[{nameof(HUDManagerPatches)}] Set word wrapping of controlTipLines to: {enabled}");
    }

    public static void HideControlTips()
    {
        HideControlTips(_instance.controlTipLines);
    }

    private static void ReplaceKeysInControlTip(TextMeshProUGUI line)
    {
        // Change text
        string oldText = line.text;
        if (string.IsNullOrWhiteSpace(oldText)) return;

        string replacedBinding = string.Empty;
        if (oldText.Contains("[G]"))
        {
            line.text = BindingReplacer.Replace(oldText, "[G]", ActionBindings.discardBinding);
            replacedBinding = ActionBindings.discardBinding;
        }
        else if (oldText.Contains("[LMB]"))
        {
            line.text = BindingReplacer.Replace(oldText, "[LMB]", ActionBindings.activateItemBinding);
            replacedBinding = ActionBindings.activateItemBinding;
        }
        else if (oldText.Contains("[Q]"))
        {
            line.text = BindingReplacer.Replace(oldText, "[Q]", ActionBindings.secondaryUseBinding);
            replacedBinding = ActionBindings.secondaryUseBinding;
        }
        else if (oldText.Contains("[E]"))
        {
            line.text = BindingReplacer.Replace(oldText, "[E]", ActionBindings.tertiaryUseBinding);
            replacedBinding = ActionBindings.tertiaryUseBinding;
        }
        else if (oldText.Contains("[Z]"))
        {
            line.text = BindingReplacer.Replace(oldText, "[Z]", ActionBindings.inspectItemBinding);
            replacedBinding = ActionBindings.inspectItemBinding;
        }
        else if (oldText.Contains("[Q/E]")) // In case of clipboard
        {
            string replace = string.Format("[{0}/{1}]", ActionBindings.GetInputBindingString(ActionBindings.secondaryUseBinding), ActionBindings.GetInputBindingString(ActionBindings.tertiaryUseBinding));
            line.text = oldText.Replace("[Q/E]", replace);
            replacedBinding = ActionBindings.secondaryUseBinding + " and " + ActionBindings.tertiaryUseBinding;
        }
        else if (oldText.StartsWith("Sprint")) // In case of round starting // DOESN'T WORK 😭
        {
            line.text = BindingReplacer.Replace(oldText, "[Shift]", ActionBindings.sprintBinding);
            replacedBinding = ActionBindings.sprintBinding;
        }
        else if (oldText.StartsWith("Scan")) // In case of round starting // DOESN'T WORK 😭
        {
            line.text = BindingReplacer.Replace(oldText, "[RMB]", ActionBindings.scanBinding);
            replacedBinding = ActionBindings.scanBinding;
        }

        if (string.IsNullOrEmpty(replacedBinding))
        {
            FinallyCorrectKeys.Logger.LogDebug($"[{nameof(HUDManagerPatches)}] No keybind found to replace");
            return;
        }
        FinallyCorrectKeys.Logger.LogDebug($"[{nameof(HUDManagerPatches)}] Binding {replacedBinding} replaced:\nOld: {oldText}\nNew: {line.text}");
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
        FinallyCorrectKeys.Logger.LogDebug(string.Format("[{0}] Hid control tips.", nameof(HUDManagerPatches)));
    }
}
