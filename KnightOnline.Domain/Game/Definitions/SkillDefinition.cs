namespace KnightOnline.Domain.Game.Definitions
{
    public class SkillDefinition
    {
        public int Id { get; set; } // Skill ID
        public string Name { get; set; }
        public int RequiredLevel { get; set; }
        public int ManaCost { get; set; }
        public int Cooldown { get; set; }
        // Add other properties like Damage, BuffType, TargetType, etc.

        public SkillDefinition()
        {
            Name = string.Empty;
        }
    }
}
