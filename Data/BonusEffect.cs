namespace MoreArmorBonuses.Data;

public class BonusEffect
{
    public string SetBonusName;
    public int SetSize;

    public int DefenseBonus;
    public int DamageBonus;
    public float DamageReduction;
    public float MeleeDamageBonus;
    public float HealthRegenerationBonus;
    public float CarryWeightBonus;
    public float PierceDamageBonus;
    public HitData.DamageModifier PoisonDamageReduction = HitData.DamageModifier.Normal;
    public HitData.DamageModifier FireDamageReduction = HitData.DamageModifier.Normal;
    public HitData.DamageModifier FrostDamageReduction = HitData.DamageModifier.Normal;
    public float MovementSpeedBonus;
    public float EitrBonus;
}