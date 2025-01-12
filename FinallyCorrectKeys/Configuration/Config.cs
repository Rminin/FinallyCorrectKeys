using BepInEx.Bootstrap;
using BepInEx.Configuration;
using FinallyCorrectKeys.Patches;
using LethalConfig;
using LethalConfig.ConfigItems;
using LethalConfig.ConfigItems.Options;

namespace FinallyCorrectKeys.Configuration;

internal class Config
{
    internal enum WordWrapOption
    {
        Enabled,
        EnabledIfGreater,
        Disabled
    }

    internal static ConfigEntry<bool> disableControlTips = null!;
    internal static ConfigEntry<WordWrapOption> wordWrap = null!;
    internal static ConfigEntry<int> wordWrapLimit = null!;

    internal static void Load(ConfigFile config)
    {
        disableControlTips = config.Bind("General",
            "Disable Control Tips",
            defaultValue: false,
            "Hides the control tips (should also work if other mods that use the control tips).\n" +
            "If you disable this setting while control tips are hidden, you need to change the inventory slot to reload them.");

        wordWrap = config.Bind("General",
            "Word Wrap",
            defaultValue: WordWrapOption.EnabledIfGreater,
            "Option for controlling word wrap.");

        wordWrapLimit = config.Bind("General",
            "Word Wrap Limit",
            defaultValue: 50,
            "Customizable limit of when word wrap should be enabled.");

        if (Chainloader.PluginInfos.ContainsKey("ainavt.lc.lethalconfig"))
        {
            LoadLethalConfig();
        }
    }

    internal static void LoadLethalConfig()
    {
        var controlTipBox = new BoolCheckBoxConfigItem(disableControlTips, requiresRestart: false);
        disableControlTips.SettingChanged += (_, _) =>
        {
            if (disableControlTips.Value)
            {
                HUDManagerPatches.HideControlTips();
            }
            FinallyCorrectKeys.Logger.LogDebug(string.Format("[{0}] Option {1} changed in LethalConfig", nameof(Config), nameof(disableControlTips)));
        };
        LethalConfigManager.AddConfigItem(controlTipBox);

        var wordWrapItem = new EnumDropDownConfigItem<WordWrapOption>(wordWrap, requiresRestart: false);
        LethalConfigManager.AddConfigItem(wordWrapItem);

        var wordWrapLimitItem = new IntSliderConfigItem(wordWrapLimit, new IntSliderOptions
        {
            Min = 0,
            Max = 100
        });
        LethalConfigManager.AddConfigItem(wordWrapLimitItem);
    }
}
