# More Armor Bonuses

More Armour Bonuses adds armour set bonuses to armour sets in Valheim.

By default, the mod includes set bonuses for vanilla armour sets that do not already have one.

The mod also supports custom armour set bonuses through JSON. To add custom bonuses, create a folder named `Override` inside your `BepInEx/plugins` folder. Inside that folder, create a file named:

```text
MoreArmorBonuses.json
```

This file should contain an array of `BonusEffect` objects. Each object represents one armour set bonus.

## Example Bonus Effect

```json
{
  "SetBonusName": "MoreArmorBonuses_Carapace",
  "SetSize": 3,
  "DefenseBonus": 20,
  "DamageReduction": 0.12,
  "MeleeDamageBonus": 0.1,
  "HealthRegenerationBonus": 1.08,
  "PoisonDamageReduction": 1,
  "CarryWeightBonus": 125,
  "PrefabNames": [
    "HelmetCarapace",
    "ArmorCarapaceChest",
    "ArmorCarapaceLegs"
  ],
  "IconPrefabName": "HelmetCarapace"
}
```

## Example With All Possible Bonuses

```jsonc
{
  "SetBonusName": "MoreArmorBonuses_Carapace", // The name of the set bonus
  "SetSize": 3, // Number of armour pieces required
  "PrefabNames": [
    "HelmetCarapace",
    "ArmorCarapaceChest",
    "ArmorCarapaceLegs"
  ], // Armour piece prefab names
  "IconPrefabName": "HelmetCarapace", // Prefab used for the bonus icon, usually the helmet

  "DefenseBonus": 20, // Additional armour/defense
  "DamageReduction": 0.12, // Damage reduction, 0.12 = 12%

  "HealthRegenerationBonus": 1.08, // 1.08 = 108%
  "EitrRegenerationBonus": 1.04, // 1.04 = 104%

  "MeleeDamageBonus": 0.1, // 0.1 = 10%
  "MagicDamageBonus": 0.2, // 0.2 = 20%

  "PoisonDamageReduction": 0, // Normal
  "FireDamageReduction": 1, // Resistant
  "FrostDamageReduction": 2, // Weak
  "LightningDamageReduction": 3, // Immune
  "BluntDamageReduction": 4, // Ignore
  "PierceDamageReduction": 5, // Very resistant
  "SlashDamageReduction": 6, // Very weak
  "ChopDamageReduction": 7, // Slightly resistant
  "PickDamageReduction": 8, // Slightly weak
  "SpiritDamageReduction": 0, // Normal

  "MovementSpeedBonus": 0.1, // 0.1 = 10%

  "StaminaRegenModifier": 1.2, // 1.2 = 120%
  "AttackStaminaModifier": 1.2,
  "BlockStaminaModifier": 1.2,
  "DodgeStaminaModifier": 1.2,
  "JumpStaminaModifier": 1.2,
  "RunStaminaModifier": 1.2,
  "SwimStaminaModifier": 1.2,
  "SneakStaminaModifier": 1.2,

  "CarryWeightBonus": 125 // Additional carry weight
}
```
