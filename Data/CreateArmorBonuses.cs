using Jotunn.Entities;
using UnityEngine;

namespace MoreArmorBonuses.Data;

public static class CreateArmorBonuses
{
    private static bool _initialized;
    private static readonly string[][] ArmorSetNames = new string[][]
    {
        ["ArmorRagsChest", "ArmorRagsLegs"],
        ["HelmetLeather", "ArmorLeatherChest", "ArmorLeatherLegs"],
        ["HelmetBronze", "ArmorBronzeChest", "ArmorBronzeLegs"],
        //["HelmetRoot", "ArmorRootChest", "ArmorRootLegs"],
        ["HelmetIron", "ArmorIronChest", "ArmorIronLegs"],
        ["HelmetDrake", "ArmorWolfChest", "ArmorWolfLegs"],
        ["HelmetPadded", "ArmorPaddedCuirass", "ArmorPaddedGreaves"],
        //["HelmetMage", "ArmorMageChest", "ArmorMageLegs"],
        ["HelmetCarapace", "ArmorCarapaceChest", "ArmorCarapaceLegs"],
        //["HelmetMage_Ashlands", "ArmorMageChest_Ashlands", "ArmorMageLegs_Ashlands"],
        ["HelmetFlametal", "ArmorFlametalChest", "ArmorFlametalLegs"]
    };

    private static readonly BonusEffect[] ArmorBonuses =
    [
        new() {SetBonusName = "MoreArmorBonuses_Rag", SetSize = 2, DefenseBonus = 2},
        new() {SetBonusName = "MoreArmorBonuses_Leather", SetSize = 3, DefenseBonus = 4, MovementSpeedBonus = 0.1f},
        new() {SetBonusName = "MoreArmorBonuses_Bronze", SetSize = 3, DefenseBonus = 6, DamageReduction = 0.02f, MeleeDamageBonus = 0.04f, HealthRegenerationBonus = 1.04f, CarryWeightBonus = 25},
        new() {SetBonusName = "MoreArmorBonuses_Iron", SetSize = 3, DefenseBonus = 10, DamageReduction = 0.12f, MeleeDamageBonus = 0.06f, PoisonDamageReduction = HitData.DamageModifier.SlightlyResistant, CarryWeightBonus = 50},
        new() {SetBonusName = "MoreArmorBonuses_Wolf", SetSize = 3, DefenseBonus = 10, DamageReduction = 0.06f, MeleeDamageBonus = 0.12f, FrostDamageReduction = HitData.DamageModifier.SlightlyResistant, CarryWeightBonus = 75},
        new () {SetBonusName = "MoreArmorBonuses_Padded", SetSize = 3, DefenseBonus = 15, DamageReduction = 0.13f, MeleeDamageBonus = 0.08f, FireDamageReduction = HitData.DamageModifier.SlightlyResistant, PoisonDamageReduction = HitData.DamageModifier.SlightlyResistant, CarryWeightBonus = 100},
        new () {SetBonusName = "MoreArmorBonuses_Carapace", SetSize = 3, DefenseBonus = 20, DamageReduction = 0.12f, MeleeDamageBonus = 0.1f, HealthRegenerationBonus = 1.08f, PoisonDamageReduction = HitData.DamageModifier.Resistant, CarryWeightBonus = 125, EitrBonus = 50},
        new () {SetBonusName = "MoreArmorBonuses_Flametal", SetSize = 3, DefenseBonus = 25, DamageReduction = 0.15f, MeleeDamageBonus = 0.15f, FireDamageReduction = HitData.DamageModifier.Resistant, HealthRegenerationBonus = 1.1f, CarryWeightBonus = 200, EitrBonus = 75}
    ];

    public static void Initialize()
    {
        if (_initialized) return;

        if (ArmorBonuses.Length != ArmorSetNames.Length)
        {
            Plugin.Logger.LogWarning("Armor bonuses and set names array lengths do not match. Skipping initialization.");
            return;
        }

        for (var i = 0; i < ArmorBonuses.Length; i++)
        {
            CreateArmorBonus(ArmorSetNames[i], ArmorBonuses[i]);
        }
        
        _initialized = true;
    }

    private static void CreateArmorBonus(string[] prefabNames, BonusEffect effectInfo)
    {
        var effect= CreateArmorEffect(effectInfo);

        foreach (var prefabName in prefabNames)
        {
            var prefab = ObjectDB.instance.GetItemPrefab(prefabName);
            if (prefab == null) continue;
            
            var itemDrop = prefab.GetComponent<ItemDrop>();
            if (itemDrop == null) continue;
            
            itemDrop.m_itemData.m_shared.m_setName = effectInfo.SetBonusName;
            itemDrop.m_itemData.m_shared.m_setSize = effectInfo.SetSize;
            itemDrop.m_itemData.m_shared.m_setStatusEffect = effect.StatusEffect;
        }
    }

    private static CustomStatusEffect CreateArmorEffect(BonusEffect effectInfo)
    {
        var effect = ScriptableObject.CreateInstance<SE_CustomSetBonus>();

        effect.name = $"SE_{effectInfo.SetBonusName}";
        effect.m_name = $"${effectInfo.SetBonusName}_effect";
        effect.m_startMessage = $"${effectInfo.SetBonusName}_start";
        effect.m_startMessageType = MessageHud.MessageType.Center;
        effect.m_stopMessage = $"${effectInfo.SetBonusName}_stop";
        effect.m_stopMessageType = MessageHud.MessageType.Center;
        effect.m_tooltip = $"${effectInfo.SetBonusName}_tooltip";
        effect.m_icon = Hud.instance.m_buildSnappingIcon; // TODO: Add custom icons
        effect.m_ttl = 0f;
        effect.m_flashIcon = false;
        effect.m_cooldownIcon = false;
        effect.m_damageReduction = effectInfo.DamageReduction;
        
        effect.m_addArmor= effectInfo.DefenseBonus; // Might need to change
        effect.m_speedModifier = effectInfo.MovementSpeedBonus; // Good
        effect.m_eitrUpFront = effectInfo.EitrBonus; // Might need to change
        effect.m_healthRegenMultiplier = effectInfo.HealthRegenerationBonus; // Good
        effect.m_percentigeDamageModifiers = new HitData.DamageTypes() // Good
        {
            m_pierce = effectInfo.PierceDamageBonus + effectInfo.MeleeDamageBonus,
            m_chop = effectInfo.MeleeDamageBonus,
            m_slash = effectInfo.MeleeDamageBonus,
            m_blunt = effectInfo.MeleeDamageBonus,
            m_pickaxe = effectInfo.MeleeDamageBonus,
        };
        effect.m_addMaxCarryWeight = effectInfo.CarryWeightBonus; // Good
        effect.m_mods = [
            new HitData.DamageModPair(){m_modifier = effectInfo.FireDamageReduction, m_type = HitData.DamageType.Fire},
            new HitData.DamageModPair(){m_modifier = effectInfo.PoisonDamageReduction, m_type = HitData.DamageType.Poison},
            new HitData.DamageModPair(){m_modifier = effectInfo.FrostDamageReduction, m_type = HitData.DamageType.Frost},
        ];

        var customEffect = new CustomStatusEffect(effect, false);
        
        Jotunn.Managers.ItemManager.Instance.AddStatusEffect(customEffect);

        return customEffect;
    }
}