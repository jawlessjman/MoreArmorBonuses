namespace MoreArmorBonuses.Data;

/// <summary>
/// Bonus effect information.
/// </summary>
public class BonusEffect
{
    // Set Info
    public string SetBonusName;
    public int SetSize;
    
    // Set Prefab Info
    public string[] PrefabNames;
    public string IconPrefabName;

    // Defense bonuses
    public int DefenseBonus = 0;
    public float DamageReduction = 1;
    
    // Health bonuses
    public float HealthRegenerationBonus = 1;
    public float EitrRegenerationBonus = 1;
    
    // Damage bonuses
    public float MeleeDamageBonus = 0;
    public float MagicDamageBonus = 0;
    
    // Damage Reduction
    public HitData.DamageModifier PoisonDamageReduction = HitData.DamageModifier.Normal;
    public HitData.DamageModifier FireDamageReduction = HitData.DamageModifier.Normal;
    public HitData.DamageModifier FrostDamageReduction = HitData.DamageModifier.Normal;
    public HitData.DamageModifier LightningDamageReduction = HitData.DamageModifier.Normal;
    public HitData.DamageModifier BluntDamageReduction = HitData.DamageModifier.Normal;
    public HitData.DamageModifier PierceDamageReduction = HitData.DamageModifier.Normal;
    public HitData.DamageModifier SlashDamageReduction = HitData.DamageModifier.Normal;
    public HitData.DamageModifier ChopDamageReduction = HitData.DamageModifier.Normal;
    public HitData.DamageModifier PickaxeDamageReduction = HitData.DamageModifier.Normal;
    public HitData.DamageModifier SpiritDamageReduction = HitData.DamageModifier.Normal;
    
    // Movement
    public float MovementSpeedBonus = 0;
    
    // Stamina
    public float StaminaRegenModifier = 1;
    public float AttackStaminaModifier = 0;
    public float BlockStaminaModifier = 0;
    public float DodgeStaminaModifier = 0;
    public float JumpStaminaModifier = 0;
    public float RunStaminaModifier = 0;
    public float SwimStaminaModifier = 0;
    public float SneakStaminaModifier = 0;
    
    // Other
    public float CarryWeightBonus = 0;
}