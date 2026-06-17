using System.Collections.Generic;
using System.Linq;
using Jotunn.Entities;
using Jotunn.Utils;
using UnityEngine;
using Newtonsoft.Json;

namespace MoreArmorBonuses.Data;

/// <summary>
/// Class responsible for creating armour bonuses.
/// </summary>
public static class CreateArmorBonuses
{
    /// <summary>
    /// Whether the armour bonuses have been initialised.
    /// </summary>
    private static bool _initialized;
    
    /// <summary>
    /// Armour bonuses to override.
    /// </summary>
    public static BonusEffect[] OverrideArmorEffects = [];
    
    /// <summary>
    /// Armour that already has effects.
    /// </summary>
    private static readonly string[] ArmorThatAlreadyHasEffects = [
        "HelmetTrollLeather", "ArmorTrollLeatherChest", "ArmorTrollLeatherLegs",
        "HelmetBerserkerHood", "ArmorBerserkerChest", "ArmorBerserkerLegs",
        "HelmetRoot", "ArmorRootChest", "ArmorRootLegs",
        "HelmetFenring", "ArmorFenringChest", "ArmorFenringLegs",
        "HelmetBerserkerUndead", "ArmorBerserkerUndeadChest", "ArmorBerserkerUndeadLegs",
        "HelmetMage", "ArmorMageChest", "ArmorMageLegs",
        "HelmetAshlandsMediumHood", "ArmorAshlandsMediumChest", "ArmorAshlandsMediumLegs",
        "HelmetMage_Ashlands", "ArmorMageChest_Ashlands", "ArmorMageLegs_Ashlands"
    ];

    /// <summary>
    /// Initialises the armour bonuses.
    /// </summary>
    public static void Initialize()
    {
        if (_initialized) return; // If already initialised, return

        if (OverrideArmorEffects.Length == 0) // If no override stats, load from JSON
        {
            const string resourceName = "MoreArmorBonuses.Data.ArmorEffects.json";
            var jsonString = AssetUtils.LoadTextFromResources(resourceName);
            if (string.IsNullOrEmpty(jsonString))
            {
                Plugin.Logger.LogWarning($"Failed to load armor effects JSON from resource: {resourceName}");
                return;
            }

            var data = JsonConvert.DeserializeObject<BonusEffect[]>(jsonString);
            if (data == null || data.Length == 0)
            {
                Plugin.Logger.LogWarning($"Failed to deserialize armor effects JSON from resource: {resourceName}");
                return;
            }

            foreach (var bonusEffect in data)
            {
                CreateArmorBonus(bonusEffect);
            }
        }
        else // If override stats, use them
        {
            foreach (var bonusEffect in OverrideArmorEffects)
            {
                CreateArmorBonus(bonusEffect);
            }
        }

        _initialized = true;
    }

    /// <summary>
    /// Creates an armour bonus for the given effect.
    /// </summary>
    /// <param name="effectInfo">The bonus effect information.</param>
    private static void CreateArmorBonus(BonusEffect effectInfo)
    {
        if (effectInfo == null)
        {
            Plugin.Logger.LogWarning("BonusEffect is null, skipping armor bonus creation.");
            return;
        }
        
        // Get the icon sprite for the status effect
        var icon = ObjectDB.instance.GetItemPrefab(effectInfo.IconPrefabName).GetComponent<ItemDrop>().m_itemData.GetIcon() ?? Hud.instance.m_buildSnappingIcon;
        var effect = CreateArmorEffect(effectInfo, icon);

        foreach (var prefabName in effectInfo.PrefabNames)
        {
            var prefab = ObjectDB.instance.GetItemPrefab(prefabName);
            if (prefab == null) continue;
            
            var itemDrop = prefab.GetComponent<ItemDrop>();
            if (itemDrop == null) continue;
            
            // Set the status effect on the items
            itemDrop.m_itemData.m_shared.m_setName = $"${effectInfo.SetBonusName}_set_name";
            itemDrop.m_itemData.m_shared.m_setSize = effectInfo.SetSize;
            itemDrop.m_itemData.m_shared.m_setStatusEffect = effect.StatusEffect;
        }
    }

    /// <summary>
    /// Creates a custom status effect for the given armour bonus.
    /// </summary>
    /// <param name="effectInfo">The bonus effect information.</param>
    /// <param name="icon">The icon sprite for the status effect.</param>
    /// <returns>The created custom status effect.</returns>
    private static CustomStatusEffect CreateArmorEffect(BonusEffect effectInfo, Sprite icon)
    {
        var effect = ScriptableObject.CreateInstance<SE_CustomSetBonus>();

        // Base Effect
        effect.name = $"SE_{effectInfo.SetBonusName}";
        effect.m_name = $"${effectInfo.SetBonusName}_effect";
        effect.m_tooltip = $"${effectInfo.SetBonusName}_tooltip";
        effect.m_icon = icon;
        effect.m_ttl = 0f;
        effect.m_flashIcon = false;
        effect.m_cooldownIcon = false;
        
        // Movement Related
        effect.m_speedModifier = effectInfo.MovementSpeedBonus;
        
        // Health Related
        effect.m_healthRegenMultiplier = effectInfo.HealthRegenerationBonus;
        effect.m_eitrRegenMultiplier = effectInfo.EitrRegenerationBonus;
        
        // Stamina Related
        effect.m_staminaRegenMultiplier = effectInfo.StaminaRegenModifier;
        effect.m_attackStaminaUseModifier = effectInfo.AttackStaminaModifier;
        effect.m_blockStaminaUseModifier = effectInfo.BlockStaminaModifier;
        effect.m_runStaminaUseModifier = effectInfo.RunStaminaModifier;
        effect.m_swimStaminaUseModifier = effectInfo.SwimStaminaModifier;
        effect.m_dodgeStaminaUseModifier = effectInfo.DodgeStaminaModifier;
        effect.m_jumpStaminaUseModifier = effectInfo.JumpStaminaModifier;
        effect.m_sneakStaminaUseModifier = effectInfo.SneakStaminaModifier;
        
        // Damage Related
        effect.m_percentigeDamageModifiers = new HitData.DamageTypes()
        {
            m_pierce = effectInfo.MeleeDamageBonus,
            m_slash = effectInfo.MeleeDamageBonus,
            m_blunt = effectInfo.MeleeDamageBonus,
            m_fire = effectInfo.MagicDamageBonus,
            m_poison = effectInfo.MagicDamageBonus,
            m_lightning = effectInfo.MagicDamageBonus,
            m_frost = effectInfo.MagicDamageBonus,
        };
        
        // Defense Related
        effect.m_damageReduction = effectInfo.DamageReduction;
        effect.m_addArmor= effectInfo.DefenseBonus;
        
        // Add Damage Modifiers
        var modifiers = new List<HitData.DamageModPair>
        {
            new() { m_modifier = effectInfo.FireDamageReduction, m_type = HitData.DamageType.Fire },
            new() { m_modifier = effectInfo.PoisonDamageReduction, m_type = HitData.DamageType.Poison },
            new() { m_modifier = effectInfo.FrostDamageReduction, m_type = HitData.DamageType.Frost },
            new() { m_modifier = effectInfo.LightningDamageReduction, m_type = HitData.DamageType.Lightning },
            new() { m_modifier = effectInfo.BluntDamageReduction, m_type = HitData.DamageType.Blunt },
            new() { m_modifier = effectInfo.PierceDamageReduction, m_type = HitData.DamageType.Pierce },
            new() { m_modifier = effectInfo.SlashDamageReduction, m_type = HitData.DamageType.Slash },
            new() { m_modifier = effectInfo.ChopDamageReduction, m_type = HitData.DamageType.Chop },
            new() { m_modifier = effectInfo.PickaxeDamageReduction, m_type = HitData.DamageType.Pickaxe },
            new() { m_modifier = effectInfo.SpiritDamageReduction, m_type = HitData.DamageType.Spirit },
        };

        effect.m_mods = modifiers
            .Where(x => x.m_modifier != HitData.DamageModifier.Normal)
            .ToList();
        
        // Other
        effect.m_addMaxCarryWeight = effectInfo.CarryWeightBonus;

        var customEffect = new CustomStatusEffect(effect, false);
        
        Jotunn.Managers.ItemManager.Instance.AddStatusEffect(customEffect);

        return customEffect;
    }
}