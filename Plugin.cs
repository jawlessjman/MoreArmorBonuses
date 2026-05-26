using BepInEx;
using BepInEx.Logging;

namespace MoreArmorBonuses;

/// <summary>
/// Main Plugin Class
/// </summary>
[BepInPlugin(Mod_GUID, Mod_Name, Mod_Version)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    
    public const string Mod_GUID = "MoreArmorBonuses";
    public const string Mod_Name = "More Armor Bonuses";
    public const string Mod_Version = "1.0.0";
        
    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }
}
