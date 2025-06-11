namespace KnightOnline.Domain.Game.Definitions
{
    public class NpcDefinition
    {
        public int Id { get; set; } // Unique ID for this NPC definition (e.g., from XML or DB)
        public string Name { get; set; } // In-game name

        public int NpcType { get; set; } // e.g., Monster, Guard, Merchant
        public int Level { get; set; }
        public long MaxHealth { get; set; } // Use long for health/mana if values can be large
        public int MaxMana { get; set; }

        public int Attack { get; set; }
        public int Defense { get; set; }
        // Add other stats like Strength, Dexterity, Intelligence, etc. as needed
        // public int Strength { get; set; }
        // public int Dexterity { get; set; }
        // public int Intelligence { get; set; }

        public int MovementSpeed { get; set; }
        public int AttackSpeed { get; set; }
        public int AttackRange { get; set; }

        public int DropGroupId { get; set; } // ID linking to a table/XML for item drops
        public int ExperiencePoints { get; set; } // EXP given when killed

        // Constructor
        public NpcDefinition()
        {
            Name = string.Empty;
            // Initialize other properties to default values if necessary
            MaxHealth = 1; // Avoid division by zero if used as a divisor
            MaxMana = 0;
            Level = 1;
        }
    }
}
