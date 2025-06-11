using KnightOnline.Domain.Game.Definitions; // If NpcDefinition is used directly
using KnightOnline.Domain.SharedKernel; // For Entity base class

namespace KnightOnline.Domain.Game.Npcs
{
    public class Npc : Entity<int> // Assuming Npc unique ID is int
    {
        public int NpcDefinitionId { get; private set; } // Foreign key to NpcDefinition
        // public NpcDefinition Definition { get; private set; } // Could be navigational property

        public string Name { get; private set; } // Could be from definition or overridden
        public int CurrentHp { get; private set; }
        public int MaxHp { get; private set; }
        // Positional data, current state, etc.

        private Npc(int id, int definitionId, string name, int maxHp) : base(id)
        {
            NpcDefinitionId = definitionId;
            Name = name;
            MaxHp = maxHp;
            CurrentHp = maxHp;
        }

        public static Npc Create(int id, int definitionId, string name, int maxHp)
        {
            // Add validation if necessary
            return new Npc(id, definitionId, name, maxHp);
        }

        // Methods to interact with NPC, e.g., TakeDamage, Die, etc.
    }
}
