namespace KnightOnline.Domain.Game.Definitions
{
    public enum SkillTargetType
    {
        Self,
        SingleEnemy,
        AreaOfEffect,
        Ally,
        Party
        // Add more as needed
    }

    public class SkillDefinition
    {
        public int Id { get; set; } // Skill ID
        public string Name { get; set; }
        public string Description { get; set; }

        public int RequiredLevel { get; set; }
        public int ManaCost { get; set; }
        public int CooldownMs { get; set; } // Cooldown in milliseconds

        public SkillTargetType TargetType { get; set; }
        public int Range { get; set; } // Skill range (e.g., 0 for self, >0 for others)
        public int AreaOfEffectRadius { get; set; } // For AoE skills

        // Basic damage/effect properties
        public int BaseDamage { get; set; } // Or BaseEffectValue for heals/buffs
        // public int DamageMultiplier { get; set; } // e.g., 1.5 for 150%

        // Could link to more complex effect systems
        // public List<int> EffectIds { get; set; } // IDs of status effects, buffs, debuffs applied

        public int RequiredSkillPoints { get; set; } // Points needed to learn/upgrade
        // public int RequiredClass { get; set; } // Class restriction

        public SkillDefinition()
        {
            Name = string.Empty;
            Description = string.Empty;
            // EffectIds = new List<int>();
        }
    }
}
