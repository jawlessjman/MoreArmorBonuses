namespace MoreArmorBonuses.Data;

public class SE_CustomSetBonus : SE_Stats
{
    public float m_damageReduction;

    public override void OnDamaged(HitData hit, Character attacker)
    {
        base.OnDamaged(hit, attacker);

        hit.ApplyModifier(1f - m_damageReduction);
    }
}