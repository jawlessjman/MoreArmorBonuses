using System.IO;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Jotunn;
using Jotunn.Managers;
using Jotunn.Utils;
using MoreArmorBonuses.Data;
using ServerSync;
using Newtonsoft.Json;

namespace MoreArmorBonuses;

/// <summary>
/// Main Plugin Class
/// </summary>
[BepInPlugin(ModGuid, ModName, ModVersion)]
[BepInDependency(Main.ModGuid)]
public class Plugin : BaseUnityPlugin
{
    internal new static ManualLogSource Logger;

    // Configs 
    public static ConfigSync ConfigSync = new ConfigSync(ModGuid)
    {
        DisplayName = ModName,
        CurrentVersion = ModVersion,
        MinimumRequiredVersion = "1.0.0",
        IsLocked = true,
        ModRequired = true
    };

    private static ConfigEntry<bool> _useOverrideStats;

    // Mod Info
    public const string ModGuid = "jawlessjman.MoreArmorBonuses";
    public const string ModName = "More Armor Bonuses";
    public const string ModVersion = "1.0.0";
        
    /// <summary>
    /// Called when the game starts.
    /// </summary>
    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        
        // Load English translations
        const string resourceName = "MoreArmorBonuses.Assets.Translations.English.MoreArmorBonuses.json";
        var localizedEnglish = AssetUtils.LoadTextFromResources(resourceName);
        if (string.IsNullOrEmpty(localizedEnglish))
        {
            Logger.LogError($"Failed to load localized English translations from resource '{resourceName}'");
        }
        else
        {
            LocalizationManager.Instance.GetLocalization().AddJsonFile("English", localizedEnglish);
        }
        
        // Load other translations
        LoadTranslations();
        
        // Load Configs
        BindConfigs();  

        // Load override stats
        if (_useOverrideStats.Value)
        {
            var stats = LoadOverrideStats();
            if (stats is not null && stats.Length > 0)
            {
                Logger.LogInfo("Using override stats for armor bonuses.");
                CreateArmorBonuses.OverrideArmorEffects = stats;
            }
        }

        ItemManager.OnItemsRegistered += CreateArmorBonuses.Initialize;
        
        Logger.LogInfo($"Plugin {ModName}-{ModVersion} is loaded!");
    }

    /// <summary>
    /// Binds the config values.
    /// </summary>
    private void BindConfigs()
    {
        _useOverrideStats = Config.Bind(
            "Settings", 
            "UseOverrideStats", 
            false, 
            "Use override stats for armor bonuses created in your own file. (place it in a folder named `Override` in the same directory as the mod)"
            );
    }

    /// <summary>
    /// Loads override stats from the Override folder.
    /// </summary>
    /// <returns></returns>
    private BonusEffect[] LoadOverrideStats()
    {
        var root = Path.Combine(
            Path.GetDirectoryName(Info.Location)!,
            "Override"
        );

        if (!Directory.Exists(root)) return null;
        
        foreach (var file in Directory.GetFiles(root, "*.json", SearchOption.AllDirectories))
        {
            Logger.LogInfo($"Loading override file: {file}");
            var jsonString = File.ReadAllText(file);
            var data = JsonConvert.DeserializeObject<BonusEffect[]>(jsonString);
            if (data is not null && data.Length > 0)
            {
                Plugin.Logger.LogInfo($"Loaded {data.Length} override stats from {file}");
                return data;
            }
        }

        return null;
    }
    
    /// <summary>
    /// Loads translations from the Translations folder.
    /// </summary>
    private void LoadTranslations()
    {
        var root = Path.Combine(
            Path.GetDirectoryName(Info.Location)!,
            "Translations"
        );

        if (!Directory.Exists(root)) return;
        
        foreach (var file in Directory.GetFiles(root, "MoreArmorBonuses.json", SearchOption.AllDirectories))
        {
            Logger.LogInfo($"Loading translation file: {file}");
            LocalizationManager.Instance.GetLocalization().AddFileByPath(file);
        }
    }
}
