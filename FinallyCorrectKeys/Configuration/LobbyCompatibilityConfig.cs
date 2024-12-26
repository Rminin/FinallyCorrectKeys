using BepInEx;
using BepInEx.Bootstrap;
using LobbyCompatibility.Enums;
using LobbyCompatibility.Features;

namespace FinallyCorrectKeys.Configuration;

internal static class LobbyCompatibilityConfig
{
    public static void Init(BaseUnityPlugin plugin)
    {
        if (Chainloader.PluginInfos.ContainsKey("BMX.LobbyCompatibility"))
        {
            LoadLobbyCompatibilityConfig(plugin.Info.Metadata);
        }
    }

    private static void LoadLobbyCompatibilityConfig(BepInPlugin plugin)
    {
        PluginHelper.RegisterPlugin(plugin.GUID, plugin.Version, CompatibilityLevel.ClientOnly, VersionStrictness.None);
    }
}
