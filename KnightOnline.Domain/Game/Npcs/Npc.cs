using KnightOnline.Domain.SharedKernel; // For Entity base class
// using KnightOnline.Domain.Game.Definitions; // Optional: if you want a direct reference to NpcDefinition object

namespace KnightOnline.Domain.Game.Npcs
{
    public class Npc : Entity<int> // Assuming Npc unique ID is int (e.g. runtime instance ID)
    {
        public int NpcDefinitionId { get; private set; } // ID from NpcDefinition.cs
        // public NpcDefinition Definition { get; private set; } // Optional: Direct reference to the definition. Loaded at runtime.

        public string Name { get; private set; } // Can be from Definition or overridden if needed

        public long CurrentHp { get; private set; } // Use long if HP can be large
        public long MaxHp { get; private set; }

        // World state
        public short CurrentZoneId { get; private set; }
        public float PositionX { get; private set; }
        public float PositionY { get; private set; }
        public float PositionZ { get; private set; }
        public short Direction { get; private set; } // Or float for more precision

        // TODO: Consider adding NPC state (e.g., Idle, Attacking, Dead, Roaming)
        // public NpcState CurrentState { get; private set; }

        // Constructor might take more parameters if an Npc instance is created with full state
        private Npc(int instanceId, int definitionId, string name, long maxHp, short zoneId, float x, float y, float z) : base(instanceId)
        {
            NpcDefinitionId = definitionId;
            Name = name; // This might come from the NpcDefinition based on definitionId
            MaxHp = maxHp; // This might also come from NpcDefinition
            CurrentHp = maxHp;
            CurrentZoneId = zoneId;
            PositionX = x;
            PositionY = y;
            PositionZ = z;
            Direction = 0;
        }

        // Factory method to create an NPC instance, potentially from its definition
        // This would typically involve looking up NpcDefinition by definitionId
        public static Npc Create(int instanceId, int definitionId, string nameFromDefinition, long maxHpFromDefinition, short initialZoneId, float initialX, float initialY, float initialZ)
        {
            // Add validation if necessary
            return new Npc(instanceId, definitionId, nameFromDefinition, maxHpFromDefinition, initialZoneId, initialX, initialY, initialZ);
        }

        public void UpdatePosition(float x, float y, float z, short direction)
        {
            PositionX = x;
            PositionY = y;
            PositionZ = z;
            Direction = direction;
            // TODO: Add Domain Event for position change if needed
        }

        public void TakeDamage(long amount)
        {
            if (amount <= 0) return;
            CurrentHp -= amount;
            if (CurrentHp < 0) CurrentHp = 0;
            // TODO: Add Domain Event for taking damage, or for HP change, or for death
        }

        public void Heal(long amount)
        {
            if (amount <= 0) return;
            CurrentHp += amount;
            if (CurrentHp > MaxHp) CurrentHp = MaxHp;
            // TODO: Add Domain Event
        }
    }
}
