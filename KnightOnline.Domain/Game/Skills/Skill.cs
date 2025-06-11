using KnightOnline.Domain.Game.Definitions; // If SkillDefinition is used
using KnightOnline.Domain.SharedKernel;

namespace KnightOnline.Domain.Game.Skills
{
    public class Skill // May or may not be an Entity, depending on if it has its own lifecycle/identity beyond definition
    {
        public int SkillDefinitionId { get; private set; }
        // public SkillDefinition Definition { get; private set; }

        // Could store player-specific skill level or mastery if applicable
        // public int PlayerSkillLevel { get; private set; }

        public Skill(int definitionId)
        {
            SkillDefinitionId = definitionId;
        }

        // Methods related to skill usage, cooldowns specific to an instance etc.
    }
}
