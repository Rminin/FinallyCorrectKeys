using BepInEx.Bootstrap;
using BepInEx.Configuration;
using FinallyCorrectKeys.Patches;
using LethalConfig;
using LethalConfig.ConfigItems;

namespace FinallyCorrectKeys.Configuration;

internal class Config
{
    internal static ConfigEntry<bool> disableControlTips = null!;
    internal static ConfigEntry<bool> disableWordWrap = null!;

    internal static void Load(ConfigFile config)
    {
        disableControlTips = config.Bind("General",
            "Disable Control Tips",
            false,
            "Hides the control tips (should also work if other mods that use the control tips).\n" +
            "If you disable this setting while control tips are hidden, you need to change the inventory slot to reload them.");

        disableWordWrap = config.Bind("General",
            "Disable Word Wrap",
            true,
            "Changes the lines of the control tips to not wrap words if the line is long.");

        if (Chainloader.PluginInfos.ContainsKey("ainavt.lc.lethalconfig"))
        {
            LoadLethalConfig();
        }
    }

    internal static void LoadLethalConfig()
    {
        var controlTipBox = new BoolCheckBoxConfigItem(disableControlTips, requiresRestart: false);
        LethalConfigManager.AddConfigItem(controlTipBox);

        var wordWrap = new BoolCheckBoxConfigItem(disableWordWrap, requiresRestart: false);
        disableWordWrap.SettingChanged += (_, _) => HUDManagerPatches.ApplyWordWrapConfig();
        LethalConfigManager.AddConfigItem(wordWrap);
    }
}
