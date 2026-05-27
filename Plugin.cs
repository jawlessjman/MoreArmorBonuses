using BepInEx;
using BepInEx.Logging;
using Jotunn;
using Jotunn.Managers;
using MoreArmorBonuses.Data;
using ServerSync;

namespace MoreArmorBonuses;

/// <summary>
/// Main Plugin Class
/// </summary>
[BepInPlugin(ModGuid, ModName, ModVersion)]
[BepInDependency(Main.ModGuid)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    public static ConfigSync ConfigSync = new ConfigSync(ModGuid)
    {
        DisplayName = ModName,
        CurrentVersion = ModVersion,
        MinimumRequiredVersion = "1.0.0",
        IsLocked = true,
        ModRequired = true
    };

    public const string ModGuid = "MoreArmorBonuses";
    public const string ModName = "More Armor Bonuses";
    public const string ModVersion = "1.0.0";
        
    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        
        LoadConfigs();

        ItemManager.OnItemsRegistered += CreateArmorBonuses.Initialize;
        
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void LoadConfigs()
    {
        
    }
}
