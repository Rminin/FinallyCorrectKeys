using BepInEx.Configuration;

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
            "Hides the control tips (should also work if other mods that use the control tips).");

        disableWordWrap = config.Bind("General",
            "Disable Word Wrap",
            true,
            "Changes the lines of the control tips to not wrap words if the line is long.");
    }
}
