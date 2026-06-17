using UnityEngine;
using UnityEngine.Serialization;

namespace MoreArmorBonuses.Data;

/// <summary>
/// Custom set bonus for damage reduction.
/// </summary>
public class SE_CustomSetBonus : SE_Stats
{
    /// <summary>
    /// The damage reduction amount.
    /// </summary>
    public float m_damageReduction;

    /// <summary>
    /// Applies damage reduction to the hit.
    /// </summary>
    /// <param name="hit">The hit data.</param>
    /// <param name="attacker">The attacker character.</param>
    public override void OnDamaged(HitData hit, Character attacker)
    {
        base.OnDamaged(hit, attacker);
    
        var multiplier = Mathf.Clamp01(1f - m_damageReduction);
        hit.m_damage.Modify(multiplier);
    }
}