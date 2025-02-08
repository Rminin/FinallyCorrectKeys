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
        ApplyWordWrapConfig();
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
        FinallyCorrectKeys.Logger.LogDebug($"[{nameof(HUDManagerPatches)}] Parameters of ChangeControlTip:\nToolTipNumber: {toolTipNumber}\nChangeTo: {changeTo}");
        TextMeshProUGUI line = __instance.controlTipLines[toolTipNumber];
        ApplyWordWrapGreater(line);
        ReplaceKeysInControlTip(line);
        if (__instance.forceChangeTextCoroutine != null)
        {
            __instance.StopCoroutine(__instance.forceChangeTextCoroutine);
            FinallyCorrectKeys.Logger.LogDebug($"[{nameof(HUDManagerPatches)}] Coroutine to force text change stopped.");
        }
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

    public static void ApplyWordWrapConfig()
    {
        foreach (var line in _instance.controlTipLines)
        {
            ApplyWordWrapGreater(line);
        }
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
        if (oldText.Contains(Bindings.DISCARD.ToHUDFormat()))
        {
            line.text = BindingReplacer.Replace(oldText, Bindings.DISCARD.ToHUDFormat(), Bindings.DISCARD);
            replacedBinding = Bindings.DISCARD;
        }
        else if (oldText.Contains(Bindings.ACTIVATE_ITEM.ToHUDFormat()))
        {
            line.text = BindingReplacer.Replace(oldText, Bindings.ACTIVATE_ITEM.ToHUDFormat(), Bindings.ACTIVATE_ITEM);
            replacedBinding = Bindings.ACTIVATE_ITEM;
        }
        else if (oldText.Contains(Bindings.SECONDARY_USE.ToHUDFormat()))
        {
            line.text = BindingReplacer.Replace(oldText, Bindings.SECONDARY_USE.ToHUDFormat(), Bindings.SECONDARY_USE);
            replacedBinding = Bindings.SECONDARY_USE;
        }
        else if (oldText.Contains(Bindings.TERTIARY_USE.ToHUDFormat()))
        {
            line.text = BindingReplacer.Replace(oldText, Bindings.TERTIARY_USE.ToHUDFormat(), Bindings.TERTIARY_USE);
            replacedBinding = Bindings.TERTIARY_USE;
        }
        else if (oldText.Contains(Bindings.INSPECT_ITEM.ToHUDFormat()))
        {
            line.text = BindingReplacer.Replace(oldText, Bindings.INSPECT_ITEM.ToHUDFormat(), Bindings.INSPECT_ITEM);
            replacedBinding = Bindings.INSPECT_ITEM;
        }
        else if (oldText.Contains(string.Format("[{0}/{1}]", Bindings.SECONDARY_USE.StandardKey, Bindings.TERTIARY_USE.StandardKey))) // In case of clipboard
        {
            string replace = string.Format("[{0}/{1}]", ActionBindings.GetInputBindingString(Bindings.SECONDARY_USE), ActionBindings.GetInputBindingString(Bindings.TERTIARY_USE));
            line.text = oldText.Replace(string.Format("[{0}/{1}]", Bindings.SECONDARY_USE.StandardKey, Bindings.TERTIARY_USE.StandardKey), replace);
            replacedBinding = Bindings.SECONDARY_USE + " and " + Bindings.TERTIARY_USE;
        }
        /* Commented out until I find out where it's applicable
        else if (oldText.StartsWith("Sprint")) // In case of round starting // DOESN'T WORK 😭
        {
            line.text = BindingReplacer.Replace(oldText, Bindings.SPRINT.ToHUDFormat(), Bindings.SPRINT);
            replacedBinding = Bindings.SPRINT;
        }
        else if (oldText.StartsWith("Scan")) // In case of round starting // DOESN'T WORK 😭
        {
            line.text = BindingReplacer.Replace(oldText, Bindings.SCAN.ToHUDFormat(), Bindings.SCAN);
            replacedBinding = Bindings.SCAN;
        }*/

        if (string.IsNullOrEmpty(replacedBinding))
        {
            FinallyCorrectKeys.Logger.LogDebug($"[{nameof(HUDManagerPatches)}] No keybind found to replace");
            return;
        }
        FinallyCorrectKeys.Logger.LogDebug($"[{nameof(HUDManagerPatches)}] Binding {replacedBinding} replaced:\nOld: {oldText}\nNew: {line.text}");
    }

    private static void ApplyWordWrapGreater(TextMeshProUGUI line)
    {
        if (Config.wordWrap.Value == Config.WordWrapOption.EnabledIfGreater)
        {
            line.enableWordWrapping = line.text.Length > Config.wordWrapLimit.Value;
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
        FinallyCorrectKeys.Logger.LogDebug($"[{nameof(HUDManagerPatches)}] Hid control tips.");
    }
}
