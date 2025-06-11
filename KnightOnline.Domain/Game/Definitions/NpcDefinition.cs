namespace KnightOnline.Domain.Game.Definitions
{
    public class NpcDefinition
    {
        public int Id { get; set; } // Or some unique identifier
        public string Name { get; set; }
        // Add other properties like NpcType, Level, Stats, DropList, etc.
        // These would typically be loaded from XML or a database.

        public NpcDefinition()
        {
            Name = string.Empty;
        }
    }
}
