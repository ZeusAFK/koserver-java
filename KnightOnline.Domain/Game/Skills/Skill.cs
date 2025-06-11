// using KnightOnline.Domain.Game.Definitions; // Optional: if you want a direct reference to SkillDefinition

namespace KnightOnline.Domain.Game.Skills
{
    // Represents a skill known by a player/character, potentially with its own level or state.
    // If skills are just static definitions and players don't have "instances" of them
    // with varying levels, then this class might not be needed, and players would just
    // have a list of SkillDefinitionIds they know.
    // Assuming for now that a player can have skills at different levels or with cooldowns.
    public class Skill
    {
        public int SkillDefinitionId { get; private set; } // ID from SkillDefinition.cs
        // public SkillDefinition Definition { get; private set; } // Optional: Direct reference

        public int PlayerSkillLevel { get; private set; } // e.g., Level 1 to 10, or mastery points
        public System.DateTime CooldownEndTimeUtc { get; private set; } // When the skill can be used again

        public Skill(int definitionId, int initialLevel = 1)
        {
            SkillDefinitionId = definitionId;
            PlayerSkillLevel = initialLevel;
            CooldownEndTimeUtc = System.DateTime.MinValue; // Available initially
        }

        public void UseSkill(System.DateTime currentTimeUtc, int cooldownMsFromDefinition)
        {
            // Basic check, more complex logic would be in an Application service
            if (currentTimeUtc < CooldownEndTimeUtc)
            {
                // Optionally throw an exception or return a result indicating it's on cooldown
                return;
            }
            CooldownEndTimeUtc = currentTimeUtc.AddMilliseconds(cooldownMsFromDefinition);
            // TODO: Add Domain Event for skill usage if other parts of the domain need to react
        }

        public bool IsOnCooldown(System.DateTime currentTimeUtc)
        {
            return currentTimeUtc < CooldownEndTimeUtc;
        }

        public void LevelUpSkill(int points = 1)
        {
            // TODO: Add validation against max skill level from definition or game rules
            PlayerSkillLevel += points;
        }
    }
}
